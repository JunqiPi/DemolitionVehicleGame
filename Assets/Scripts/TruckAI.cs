using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAI : MonoBehaviour
{
    public GameObject[] waypoints;
    public float patrolRadius = 100f;
    public float chaseRadius = 200f;
    public AIState currentState;
    public WheelCollider frontLeftW, frontRightW, rearLeftW,rearRightW;
    public Transform frontLeftT, frontRightT, rearLeftT, rearRightT;
    public float maxSteerAngle = 45f;
    public float motorForce = 2500f;
    public float ramSpeed = 1f;
    public float ramTime = 5f;
    public float chargeTime = 3f;
    public float recoilTime = 3f;
    
    private int currWaypoint = -1;
    private float m_steeringAngle;
    private GameObject movingWaypoint;
    private Rigidbody rb;
    private float timer;
    private float flipTimer = 0f;

    public enum AIState
    {
        Patrol,
        PrepCharge,
        Charge,
        Ram,
        Recoil
    }

    private float AvoidObstacle(float angle)
    {
        Vector3 forward = transform.forward * -1f;
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
        Vector3 forward = transform.forward * -1f;
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
            frontLeftW.steerAngle = m_steeringAngle;
            frontRightW.steerAngle = m_steeringAngle;
        }
    }

    private void Accelerate()
    {
        // front wheel drive
        frontLeftW.motorTorque = motorForce * -1f;
        frontRightW.motorTorque = motorForce * -1f;
        rearLeftW.motorTorque = motorForce * -1f;
        rearRightW.motorTorque = motorForce * -1f;
    }

    private void AngleVehicle()
    {
        // Angle vehicle to face player
        transform.LookAt(transform.position - (movingWaypoint.transform.position - transform.position), transform.rotation * Vector3.up);
    }

    private void RamAtConstSpeed()
    {
        // Run at constant speed as long as all wheels grounded
        if (frontLeftW.isGrounded && frontRightW.isGrounded && rearLeftW.isGrounded && rearRightW.isGrounded)
        {
            rb.AddForce(transform.forward * ramSpeed * -1f, ForceMode.Force);
        }
        // front wheel drive
        frontLeftW.motorTorque = motorForce * -1f;
        frontRightW.motorTorque = motorForce * -1f;
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

    private void ResetSteeringAngle()
    {
        m_steeringAngle = 0;
        frontLeftW.steerAngle = m_steeringAngle;
        frontRightW.steerAngle = m_steeringAngle;
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(rearLeftW, rearLeftT);
        UpdateWheelPose(rearRightW, rearRightT);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.5f, 0);

        currentState = AIState.Patrol;
        SetNextWaypoint();
    }

    void FixedUpdate()
    {
        FlipMidairForce();
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;

            case AIState.PrepCharge:
                PrepCharge();
                break;

            case AIState.Charge:
                Charge();
                break;

            case AIState.Ram:
                Ram();
                break;

            case AIState.Recoil:
                Recoil();
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "ground" & collision.transform.tag != "projectile")
        {
            TransitionToState(AIState.Recoil);
        }
    }

    void TransitionToState(AIState newState)
    {
        currentState = newState;

        if (currentState == AIState.Patrol)
        {
            // patrol again
            ResetBrake();
            SetNextWaypoint();
        }
        else if (currentState == AIState.PrepCharge)
        {
            ResetAcceleration();
        }
        else if (currentState == AIState.Charge)
        {
            timer = 0f;
            ResetBrake();
        }
        else if (currentState == AIState.Ram)
        {
            timer = 0f;
            ResetBrake();
        }
        else if (currentState == AIState.Recoil)
        {
            ResetAcceleration();
        }
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

    void Patrol()
    {
        // If player in range, transition to charge state (change to line of sight later)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, patrolRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.root != transform.root)
            {
                if (hitCollider.transform.root.CompareTag("Player"))
                {
                    movingWaypoint = hitCollider.transform.root.GetChild(0).gameObject;
                    TransitionToState(AIState.PrepCharge);
                    return;
                }
            }
        }

        // / Error handling for no waypoints
        if (currWaypoint == -1) return;

        // If a waypoint is reached, move to a random different waypoint
        if (Vector3.Distance(this.transform.position, waypoints[currWaypoint].transform.position) < 5f)
        {
            SetNextWaypoint();
        }

        Steer(waypoints[currWaypoint].transform.position);
        Accelerate();
        UpdateWheelPoses();
    }

    void PrepCharge()
    {
        // brake until velocity is 0 (while angling towards player)
        Steer(movingWaypoint.transform.position);
        Brake();
        UpdateWheelPoses();
        if (rb.velocity.magnitude < 0.5f)
        {
            TransitionToState(AIState.Charge);
        }
    }

    void Charge()
    {
        // angle towards player for a few seconds
        ResetSteeringAngle();
        AngleVehicle();
        // Brake();
        UpdateWheelPoses();
        timer += Time.deltaTime;
        if (timer > chargeTime)
        {
            TransitionToState(AIState.Ram);
        }
    }

    void Ram()
    {
        // Attempt to ram into player for 5 seconds or until collision
        ResetSteeringAngle();
        RamAtConstSpeed();
        UpdateWheelPoses();
        timer += Time.deltaTime;
        if (timer > ramTime)
        {
            TransitionToState(AIState.Recoil);
        }
    }

    void Recoil()
    {
        // Once ram ends, brake, then return to patrol
        Brake();
        UpdateWheelPoses();
        if (rb.velocity.magnitude < 0.5f)
        {
            TransitionToState(AIState.Patrol);
        }
    }

    private void FlipCar()
    {
        Vector3 fwd = transform.forward;
        transform.rotation = Quaternion.identity;
        transform.forward = fwd;
    }

    private void FlipMidairForce()
    {
        float flipStrength = 1f;
        if (Mathf.Abs(transform.localRotation.eulerAngles.z) <= 280f && Mathf.Abs(transform.localRotation.eulerAngles.z) >= 80f) {
            // print(Mathf.Abs(transform.localRotation.eulerAngles.z));
            rb.AddRelativeTorque(0f, 0f, flipStrength, ForceMode.Acceleration);
            flipTimer += Time.deltaTime;
            // If been flipped too long, flip car
            if (flipTimer >= 3f)
            {
                FlipCar();
            }
        }
        else
        {
            flipTimer = 0f;
        }
    }
}
