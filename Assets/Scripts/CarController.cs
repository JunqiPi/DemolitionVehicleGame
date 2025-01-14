using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class CarController : MonoBehaviour, IVehicle
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;
    private float motorForce;

    [SerializeField] private float defaultMotorForce = 10000f;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;


    [SerializeField] private List<WheelCollider> rearWheels;
    [SerializeField] private List<Transform> rearWheelTransforms;


    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;



    [SerializeField] private float nitrousBoostForce = 50000f;
    [SerializeField] private float nitrousDuration = 2f;
    private bool isBoosting = false;
    private float boostTimer = 0f;

    [SerializeField] private ParticleSystem nitroEffect1;
    [SerializeField] private ParticleSystem nitroEffect2;

    [SerializeField] private TMP_Text speedometerText;

    private Rigidbody rb;
    private float flipTimer = 0f;
    public Rigidbody Rigidbody => rb;
    public float lastDamageTime { get; set;}
    public bool IsBoosting { get => isBoosting; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -2f, 0);
        motorForce = defaultMotorForce;
        // rb.drag = 0.3f;
        // rb.angularDrag = 0.1f;

        // rb.interpolation = RigidbodyInterpolation.Interpolate;

        // AdjustWheelFriction(frontLeftWheelCollider);
        // AdjustWheelFriction(frontRightWheelCollider);

        // AdjustSuspension(frontLeftWheelCollider);
        // AdjustSuspension(frontRightWheelCollider);


        // foreach (WheelCollider wheel in rearWheels)
        // {
        //     AdjustWheelFriction(wheel);
        //     AdjustSuspension(wheel);
        // }



    }

    private void FixedUpdate()
    {
        FlipMidairForce();
        GetInput();
        HandleSteering();
        HandleMotor();
        // ApplyCounterDrift();
        UpdateWheels();
        HandleNitrousBoost();
        UpdateSpeedometer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        // currentBreakForce = isBreaking ? breakForce : 0f;


        // foreach (WheelCollider wheel in rearWheels)
        // {
        //     if (verticalInput != 0)
        //     {

        //         if ((wheel.motorTorque > 0 && verticalInput < 0) || (wheel.motorTorque < 0 && verticalInput > 0))
        //         {
        //             wheel.brakeTorque = breakForce;
        //             wheel.motorTorque = 0f;
        //         }
        //         else
        //         {
        //             wheel.motorTorque = verticalInput * motorForce;
        //             wheel.brakeTorque = 0f;
        //         }
        //     }
        //     else
        //     {
        //         wheel.brakeTorque = breakForce * 0.5f;
        //         wheel.motorTorque = 0f;
        //     }
        // }


        foreach (WheelCollider frontWheel in new WheelCollider[] { frontLeftWheelCollider, frontRightWheelCollider })
        {
            if (isBreaking)
            {
                foreach (WheelCollider wheel in rearWheels)
                {
                    wheel.brakeTorque = breakForce;
                    wheel.motorTorque = 0f;
                }

                frontLeftWheelCollider.motorTorque = 0f;
                frontRightWheelCollider.motorTorque = 0f;
                frontLeftWheelCollider.brakeTorque = breakForce;
                frontRightWheelCollider.brakeTorque = breakForce;
            }
            else if (verticalInput != 0)
            {
                foreach (WheelCollider wheel in rearWheels)
                {
                    wheel.brakeTorque = 0f;
                    wheel.motorTorque = verticalInput * motorForce;
                }
                frontWheel.motorTorque = verticalInput * motorForce;
                frontWheel.brakeTorque = 0f;
            }
            else
            {
                foreach (WheelCollider wheel in rearWheels)
                {
                    wheel.brakeTorque = breakForce * 0.2f;
                    wheel.motorTorque = 0f;
                }
                frontWheel.brakeTorque = breakForce * 0.2f;
                frontWheel.motorTorque = 0f;
            }
        }

    }

    private void ApplyCounterDrift()
    {
        float sidewaysVel = Vector3.Dot(rb.velocity, transform.right);
        rb.AddForce(sidewaysVel * -1f * transform.right, ForceMode.VelocityChange);
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;

        // Apply steering to front wheels
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        // Update front wheels
        UpdateWheelTransform(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelTransform(frontRightWheelCollider, frontRightWheelTransform);

        // Update rear wheels
        for (int i = 0; i < rearWheels.Count; i++)
        {
            UpdateWheelTransform(rearWheels[i], rearWheelTransforms[i]);
        }
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);  // Get the wheel's position and rotation from the WheelCollider
        wheelTransform.position = pos;                 // Update the position of the wheel model
        wheelTransform.rotation = rot;                 // Update the rotation of the wheel model
    }

    private void HandleNitrousBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isBoosting)
        {
            isBoosting = true;
            boostTimer = nitrousDuration;
            motorForce = nitrousBoostForce;
            Debug.Log("Motor Force during boost: " + motorForce);
            Debug.Log("Nitrous");
            nitroEffect1.Play();
            nitroEffect2.Play();


        }


        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0f)
            {
                isBoosting = false;
                motorForce = defaultMotorForce;
                nitroEffect1.Stop();
                nitroEffect2.Stop();
            }
        }
    }

    private void AdjustWheelFriction(WheelCollider wheel)
    {

        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.extremumValue = 1.0f;
        forwardFriction.stiffness = 1.5f;
        forwardFriction.extremumSlip = 0.3f;
        forwardFriction.asymptoteSlip = 0.6f;
        forwardFriction.asymptoteValue = 1f;
        wheel.forwardFriction = forwardFriction;


        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.extremumValue = 1.0f;
        sidewaysFriction.extremumSlip = 0.2f;
        sidewaysFriction.stiffness = 2f;
        sidewaysFriction.asymptoteSlip = 0.5f;
        sidewaysFriction.asymptoteValue = 0.75f;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void AdjustSuspension(WheelCollider wheel)
    {
        JointSpring suspensionSpring = wheel.suspensionSpring;

        suspensionSpring.spring = 50000f;
        suspensionSpring.damper = 8500f;
        suspensionSpring.targetPosition = 0.5f;

        wheel.suspensionSpring = suspensionSpring;
        wheel.suspensionDistance = 0.1f;
    }

    private void UpdateSpeedometer()
    {

        float speedInMetersPerSecond = rb.velocity.magnitude;
        float speedInMPH = speedInMetersPerSecond * 2.237f;

        if(speedometerText!=null)
            speedometerText.text = Mathf.CeilToInt(speedInMPH).ToString()+" MPH";
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
