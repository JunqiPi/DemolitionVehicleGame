using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset; 
    public float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {

        Vector3 desiredPosition = target.position + offset;


        if (Vector3.Distance(transform.position, desiredPosition) > 0.01f)
        {
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
        }


        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed);

    }

}

