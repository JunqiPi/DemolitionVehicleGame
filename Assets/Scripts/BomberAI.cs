using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAI : MonoBehaviour
{
    public GameObject[] waypoints;
    public float patrolRadius = 100f;
    public float chaseRadius = 200f;
    public AIState currentState;
    public WheelCollider frontLeftW, frontRightW, rearLeftW,rearRightW;
    public Transform frontLeftT, frontRightT, rearLeftT, rearRightT;
    public float maxSteerAngle = 45f;
    public float motorForce = 10000f;
    public float ramSpeed = 180f;
    public float ramTime = 5f;
    public float chargeTime = 3f;
    public float recoilTime = 3f;
    
    private int currWaypoint = -1;
    private float m_steeringAngle;
    private GameObject movingWaypoint;
    private Rigidbody rb;
    private float timer;

    public enum AIState
    {
        Patrol,
        GetInRange,
        PrepBomb,
        LobBomb,    // Once bomb lobbed, wait a couple seconds before patrolling again
    }
}
