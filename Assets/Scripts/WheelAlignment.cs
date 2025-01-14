using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelColliderSetup : MonoBehaviour
{
    public WheelCollider[] wheelColliders; // Array of WheelColliders for all wheels
    public Transform[] visualWheels;       // Array of visual wheels (meshes)

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
        // Align the WheelCollider's position to the visual wheel's position
        wheelCollider.transform.position = visualWheel.position;

        // You may need to adjust the height by a small amount, depending on your suspension settings
        wheelCollider.transform.position += new Vector3(0, wheelCollider.suspensionDistance / 2, 0);

        // Set the radius of the WheelCollider to match the visual wheel (assuming uniform scale)
        wheelCollider.radius = visualWheel.GetComponent<Renderer>().bounds.extents.y;
    }
}

