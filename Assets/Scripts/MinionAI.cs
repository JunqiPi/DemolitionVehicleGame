using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class MinionAI : MonoBehaviour
{
    public AIState currentState;
    public GameObject[] waypoints;
    public float patrolRadius = 100f;
    public float chaseRadius = 200f;
    public WheelCollider frontLeftW, frontRightW, rearLeftW,rearRightW;
    public Transform frontLeftT, frontRightT, rearLeftT, rearRightT;
    public float maxSteerAngle = 45;
    public float motorForce = 2500;
    public float reverseDir = -1f;
    
    private int currWaypoint = -1;
    private GameObject movingWaypoint;
    private float m_steeringAngle;
    private Rigidbody rb;
    private float timer;
    private FlameThrower flameThrower;


    private Vector3 lastCheckedPosition;
    private float positionCheckTimer = 0f;
    private float intervalCheck = 0.2f; 
    private float minMovement = 0.1f;

    public enum AIState
    {
        Patrol,
        Chase,
        Recoil,
        Destruction
    }

    private float AvoidObstacle(float angle)
    {
        Vector3 forward = transform.forward;
        // Raycast in direction, if wall, steer away
        Vector3 targetDir = Quaternion.AngleAxis(angle, forward) * forward;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, targetDir, out hit, 30))
        {
            if (hit.collider.tag != "Enemy" && hit.collider.tag != "Player")
            {
                // if forward no good, steer opposite direction
                if (Physics.Raycast(transform.position, forward, out hit, 30))
                {
                    if (hit.collider.tag != "Enemy" && hit.collider.tag != "Player")
                    {
                        float oppositeDir = Mathf.Sign(angle);
                        return maxSteerAngle * oppositeDir;
                    }
                    else {
                        return 0f;
                    }
                }
            }
        }
        return angle;
    }

    private void Steer(Vector3 _pos)
    {
        Vector3 targetDir = _pos - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
        if (angle < maxSteerAngle * -1f)
        {
            // turn right
            m_steeringAngle = AvoidObstacle(maxSteerAngle);
            frontLeftW.steerAngle = m_steeringAngle;
            frontRightW.steerAngle = m_steeringAngle;
        }
        else if (angle < 0f)
        {
            m_steeringAngle = AvoidObstacle(angle * -1f);
            frontLeftW.steerAngle = m_steeringAngle;
            frontRightW.steerAngle = m_steeringAngle;
        }
        else if (angle > maxSteerAngle)
        {
            // turn left
            m_steeringAngle = AvoidObstacle(maxSteerAngle * -1f);
            frontLeftW.steerAngle = m_steeringAngle;
            frontRightW.steerAngle = m_steeringAngle;
        }
        else if (angle > 0f)
        {
            m_steeringAngle = AvoidObstacle(angle * -1f);
            frontLeftW.steerAngle = m_steeringAngle;
            frontRightW.steerAngle = m_steeringAngle;
        }
        else
        {
            m_steeringAngle = AvoidObstacle(0f);
        }
    }

    private void Accelerate()
    {
        // rear wheel drive
        frontLeftW.motorTorque = motorForce * reverseDir * -1f;
        frontRightW.motorTorque = motorForce * reverseDir * -1f;
        rearLeftW.motorTorque = motorForce * reverseDir * -1f;
        rearRightW.motorTorque = motorForce * reverseDir * -1f;
    }

    private void Brake()
    {
        // Apply brake torque when motortorque > 0
        // front wheel drive
        frontLeftW.brakeTorque = motorForce;
        frontRightW.brakeTorque = motorForce;
        rearLeftW.brakeTorque = motorForce;
        rearRightW.brakeTorque = motorForce;
    }

    private void TurnBrake()
    {
        frontLeftW.brakeTorque = motorForce * 0.2f;
        frontRightW.brakeTorque = motorForce * 0.2f;
        rearLeftW.brakeTorque = motorForce * 0.2f;
        rearRightW.brakeTorque = motorForce * 0.2f;
    }

    private void ResetAcceleration()
    {
        // front wheel drive
        frontLeftW.motorTorque = 0f;
        frontRightW.motorTorque = 0f;
        rearLeftW.motorTorque = 0f;
        rearRightW.motorTorque = 0f;
    }

    private void ResetBrake()
    {
        // front wheel drive
        frontLeftW.brakeTorque = 0f;
        frontRightW.brakeTorque = 0f;
        rearLeftW.brakeTorque = 0f;
        rearRightW.brakeTorque = 0f;
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        if (_collider == null || _transform == null)
            return;
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void UpdateWheelPoses()

    {

        if (frontLeftT != null) UpdateWheelPose(frontLeftW, frontLeftT);
        if (frontRightT != null) UpdateWheelPose(frontRightW, frontRightT);
        if (rearLeftT != null) UpdateWheelPose(rearLeftW, rearLeftT);
        if (rearRightT != null) UpdateWheelPose(rearRightW, rearRightT);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.5f, 0);
        flameThrower = GetComponentInChildren<FlameThrower>();

        currentState = AIState.Patrol;
        SetNextWaypoint();
    }

    void FixedUpdate()
    {

        if (currentState == AIState.Destruction)
        {
            
            return;
        }

        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.Recoil:
                Recoil();
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "ground")
        {
            Debug.Log("Ignoring ground collision");
            return;
        }

        TransitionToState(AIState.Recoil);
        
    }

    void Patrol()
    {
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
             // If another vehicle in range, transition to chase state
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, patrolRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.root != transform.root)
                {
                    if (hitCollider.transform.root.CompareTag("Player"))
                    {
                        movingWaypoint = hitCollider.transform.root.GetChild(0).gameObject;
                        TransitionToState(AIState.Chase);
                        return;
                    }
                    // else if (hitCollider.transform.root.CompareTag("Enemy"))
                    // {
                    //     movingWaypoint = hitCollider.transform.root.gameObject;
                    //     TransitionToState(AIState.Chase);
                    //     return;
                    // }
                }
            }
        }

        // Error handling for no waypoints
        if (currWaypoint == -1) return;

        // If a waypoint is reached, move to a random different waypoint
        if (Vector3.Distance(this.transform.position, waypoints[currWaypoint].transform.position) < 2f)
        {
            SetNextWaypoint();
        }

        Steer(waypoints[currWaypoint].transform.position);
        Accelerate();
        UpdateWheelPoses();
    }


    void SetNextWaypoint()
    {
        // Error handling for no waypoints
        if (waypoints.Length == 0)
        {
            return;
        }
        // Pick a random waypoint to go to (different from one already chosen)
        int randWaypoint = Random.Range(0, waypoints.Length);
        while (randWaypoint == currWaypoint)
        {
            randWaypoint = Random.Range(0, waypoints.Length);
        }
        currWaypoint = randWaypoint;

        if (waypoints.Length == 0) 
        {
            return; 
        }
    }


    void Chase()
    {
        timer += Time.deltaTime;
        // If vehicle dies or error
        if (movingWaypoint == null)
        {
            flameThrower.StopShooting();
            TransitionToState(AIState.Patrol);
            return;
        }

        // Calculate the distance to the moving waypoint
        float distanceToMovingWaypoint = Vector3.Distance(transform.position, movingWaypoint.transform.position);

        // If distance from waypoint too far or chased for x seconds, switch back to patrol
        if (distanceToMovingWaypoint > chaseRadius || timer >= 3f)
        {
            flameThrower.StopShooting();
            TransitionToState(AIState.Patrol);
            return;
        }
        
        // Only predict one second ahead (to make it fair)
        float timeToReach = Mathf.Min(distanceToMovingWaypoint / rb.velocity.magnitude, 1f);
        // Predict future waypoint position
        Vector3 predictedPosition = PredictMovingWaypointPosition(timeToReach);

        Steer(predictedPosition);
        Accelerate();
        UpdateWheelPoses();
    }

    void Recoil()
    {
        timer += Time.deltaTime;
        positionCheckTimer += Time.deltaTime;

    
        if (positionCheckTimer >= intervalCheck)
        {
            float actualMovement = Vector3.Distance(transform.position, lastCheckedPosition);
           

            if (actualMovement < minMovement)
            {
                
                float steerAngle = Random.Range(-maxSteerAngle * 0.5f, maxSteerAngle * 0.5f);
                frontLeftW.steerAngle = steerAngle;
                frontRightW.steerAngle = steerAngle;

                float appliedTorque = (motorForce * reverseDir) *3f;
                frontLeftW.motorTorque = appliedTorque;
                frontRightW.motorTorque = appliedTorque;
                rearLeftW.motorTorque = appliedTorque;
                rearRightW.motorTorque = appliedTorque;

              
                timer = 0f; 
            }
            else if (timer > 1f)
            {
                
                TransitionToState(AIState.Patrol);
            }

           
            lastCheckedPosition = transform.position;
            positionCheckTimer = 0f;
        }

        UpdateWheelPoses();
    }

    Vector3 PredictMovingWaypointPosition(float timeToReach)
    {
        // Get the velocity from VelocityReporter
        Vector3 velocity = movingWaypoint.GetComponent<VelocityReporter>().velocity;

        // Predict future position using vel and dt
        Vector3 predictedPosition = movingWaypoint.transform.position + (velocity * timeToReach);

        // Clamp position to stay on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(predictedPosition, out hit, 2.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return predictedPosition;
    }


    public void TransitionToState(AIState newState)
    {
        currentState = newState;

        if (currentState == AIState.Patrol)
        {
            timer = 0f;
            ResetBrake();
            // patrol again
            SetNextWaypoint();
        }
        else if (currentState == AIState.Chase)
        {
            flameThrower.Shoot();
            timer = 0f;
            ResetBrake();
            // intercept waypoint
            // print("Switching to intercept.");
        }
        else if (currentState == AIState.Recoil)
        {
            timer = 0f;
            ResetAcceleration();
        }

        else if (currentState == AIState.Destruction)
        {
            ResetAcceleration();
            ResetBrake();
            flameThrower.StopShooting();
            Debug.Log("Switched to destruction state.");
        }
    }

}
