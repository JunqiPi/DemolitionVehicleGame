using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontLeftW, frontRightW, rearLeft1W, rearLeft2W, rearRight1W, rearRight2W;
    public Transform frontLeftT, frontRightT, rearLeft1T, rearLeft2T, rearRight1T, rearRight2T;
    public float maxSteerAngle = 30;
    public float motorForce = 300;

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }
    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontLeftW.steerAngle = m_steeringAngle;
        frontRightW.steerAngle = m_steeringAngle;
    }
    private void Accelerate()
    {
        // front wheel drive
        frontLeftW.motorTorque = m_verticalInput * motorForce;
        frontRightW.motorTorque = m_verticalInput * motorForce;

        // rear wheel drive
        // rearLeft1W.motorTorque = m_verticalInput * motorForce;
        // rearLeft2W.motorTorque = m_verticalInput * motorForce;
        // rearRight1W.motorTorque = m_verticalInput * motorForce;
        // rearRight2W.motorTorque = m_verticalInput * motorForce;
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
        UpdateWheelPose(rearLeft1W, rearLeft1T);
        UpdateWheelPose(rearLeft2W, rearLeft2T);
        UpdateWheelPose(rearRight1W, rearRight1T);
        UpdateWheelPose(rearRight2W, rearRight2T);
    }
    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }
}
