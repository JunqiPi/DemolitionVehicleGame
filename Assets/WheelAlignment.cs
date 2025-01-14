using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAlignment : MonoBehaviour
{
    public WheelCollider[] wheelColliders; 
    public Transform[] visualWheels;       

    void Start()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            if (wheelColliders[i] != null && visualWheels[i] != null)
            {
                AlignWheelColliderWithVisualWheel(wheelColliders[i], visualWheels[i]);
            }
            else
            {
                Debug.LogError("Missing WheelCollider or VisualWheel at index : " + i);
            }
        }
    }
    



    private void AlignWheelColliderWithVisualWheel(WheelCollider wheelCollider, Transform visualWheel)
    {
        
        wheelCollider.transform.position = visualWheel.position;

        
        wheelCollider.transform.position += new Vector3(0, wheelCollider.suspensionDistance / 2, 0);


        wheelCollider.radius = visualWheel.GetComponent<Renderer>().bounds.extents.y;
    }
}

