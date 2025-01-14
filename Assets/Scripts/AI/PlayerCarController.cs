using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCarController : MonoBehaviour
{
    public WheelCollider FrontLeftWheelCollider;
    public WheelCollider FrontRightWheelCollider;
    public WheelCollider RearLeftWheelCollider;
    public WheelCollider RearRightWheelCollider;

    public Transform FrontLeftWheelTransform;
    public Transform FrontRightWheelTransform;
    public Transform RearLeftWheelTransform;
    public Transform RearRightWheelTransform;

    public Transform CenterOfMass;

    private Rigidbody carRigidbody;
    private bool isBraking = false;
    private float motorTorque = 10000f;

    public float currentSpeed = 0.0f;
    private Vector3 previousPosition;

    private Scene currentScene;
    private string currentSceneName;

    void Start()
    {
        InitializeCar();
    }

    void FixedUpdate()
    {
        UpdateMotorAndSteering();
        ApplyBrakes();
    }

    void Update()
    {
        UpdateSpeed();
        RotateWheels();
        UpdateWheelSteering();
    }

    private void InitializeCar()
    {
        previousPosition = transform.position;
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = CenterOfMass.localPosition;
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
    }

    private void UpdateMotorAndSteering()
    {
        if (!isBraking)
        {
            ResetBrakeTorque();
        }

        float motorInput = motorTorque * Input.GetAxis("Vertical");
        SetMotorTorque(motorInput);

        float steeringInput = 30f * Input.GetAxis("Horizontal");
        SetSteeringAngle(steeringInput);
    }

    private void SetMotorTorque(float torque)
    {
        RearLeftWheelCollider.motorTorque = torque;
        RearRightWheelCollider.motorTorque = torque;
        FrontLeftWheelCollider.motorTorque = torque;
        FrontRightWheelCollider.motorTorque = torque;
    }

    private void SetSteeringAngle(float angle)
    {
        FrontLeftWheelCollider.steerAngle = angle;
        FrontRightWheelCollider.steerAngle = angle;
    }

    private void ResetBrakeTorque()
    {
        FrontLeftWheelCollider.brakeTorque = 0;
        FrontRightWheelCollider.brakeTorque = 0;
        RearLeftWheelCollider.brakeTorque = 0;
        RearRightWheelCollider.brakeTorque = 0;
    }

    private void UpdateSpeed()
    {
        Vector3 movement = transform.position - previousPosition;
        currentSpeed = movement.magnitude / Time.deltaTime;
        previousPosition = transform.position;
    }

    private void RotateWheels()
    {
        RotateWheel(FrontLeftWheelTransform, FrontLeftWheelCollider);
        RotateWheel(FrontRightWheelTransform, FrontRightWheelCollider);
        RotateWheel(RearLeftWheelTransform, RearLeftWheelCollider);
        RotateWheel(RearRightWheelTransform, RearRightWheelCollider);
    }

    private void RotateWheel(Transform wheelTransform, WheelCollider wheelCollider)
    {
        wheelTransform.Rotate(wheelCollider.rpm / 60 * 360 * Time.deltaTime, 0, 0);
    }

    private void UpdateWheelSteering()
    {
        AdjustWheelSteering(FrontLeftWheelTransform, FrontLeftWheelCollider);
        AdjustWheelSteering(FrontRightWheelTransform, FrontRightWheelCollider);
    }

    private void AdjustWheelSteering(Transform wheelTransform, WheelCollider wheelCollider)
    {
        Vector3 wheelEulerAngles = wheelTransform.localEulerAngles;
        wheelEulerAngles.y = wheelCollider.steerAngle - wheelTransform.localEulerAngles.z;
        wheelTransform.localEulerAngles = wheelEulerAngles;
    }

    private void ApplyBrakes()
    {
        if (Input.GetButton("Jump"))
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }

        if (isBraking)
        {
            SetBrakeTorque(10000);
            SetMotorTorque(0);
        }
    }

    private void SetBrakeTorque(float torque)
    {
        RearLeftWheelCollider.brakeTorque = torque;
        RearRightWheelCollider.brakeTorque = torque;
        FrontLeftWheelCollider.brakeTorque = torque;
        FrontRightWheelCollider.brakeTorque = torque;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Debug.Log("COLLISION DETECTED");
        }
    }
}
