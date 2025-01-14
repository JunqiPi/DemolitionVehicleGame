using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public GameObject player;
    public GameObject currentTarget;
    public GameObject currentDestination;

    public enum AIState
    {
        Patrolling,
        Chasing,
        Vulnerable
    }

    public AIState aiState;

    public GameObject[] patrolPoints;
    private int patrolIndex;

    public float patrolSpeed = 0.5f;
    public float chaseSpeed = 1.0f;

    private bool isKnockedBack;
    private Vector3 knockBackDirection;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = true;
        navAgent.updateRotation = true;

        aiState = AIState.Patrolling;

        patrolIndex = Random.Range(0, patrolPoints.Length - 1);
        SetDestination(patrolPoints[patrolIndex].transform.position);

        StartCoroutine(AIStateMachine());
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            navAgent.velocity = knockBackDirection * 10;
        }
    }

    private IEnumerator KnockBackRoutine()
    {
        isKnockedBack = true;
        navAgent.speed = 2;
        navAgent.acceleration = 10;

        yield return new WaitForSeconds(0.2f);

        ResetAgentAfterKnockBack();
        aiState = AIState.Vulnerable;
    }

    private IEnumerator AIStateMachine()
    {
        while (true)
        {
            switch (aiState)
            {
                case AIState.Patrolling:
                    Patrol();
                    break;
                case AIState.Chasing:
                    Chase();
                    break;
                case AIState.Vulnerable:
                    HandleVulnerability();
                    break;
            }
            yield return null;
        }
    }

    private void Patrol()
    {
        navAgent.speed = patrolSpeed;

        if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].transform.position) >= 2f)
        {
            SetDestination(patrolPoints[patrolIndex].transform.position);
        }
        else
        {
            patrolIndex = Random.Range(0, patrolPoints.Length);
            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
        }
    }

    private void Chase()
    {
        navAgent.speed = chaseSpeed;
        if (currentTarget != null)
        {
            SetDestination(currentTarget.transform.position);
        }
    }

    private void HandleVulnerability()
    {
        navAgent.speed = 150;
        SetDestination(patrolPoints[0].transform.position);
    }

    private void SetDestination(Vector3 destination)
    {
        navAgent.SetDestination(destination);
        //currentDestination = destination;
    }

    private void ResetAgentAfterKnockBack()
    {
        isKnockedBack = false;
        navAgent.speed = 25;
        navAgent.angularSpeed = 300;
        navAgent.acceleration = 30;
        //currentTarget = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            aiState = AIState.Chasing;
            currentTarget = other.gameObject;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            knockBackDirection = collision.transform.forward;
            StartCoroutine(KnockBackRoutine());
            patrolIndex = Random.Range(0, patrolPoints.Length - 1);
        }
    }
}
