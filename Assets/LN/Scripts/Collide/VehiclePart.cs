using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public abstract class VehiclePart : MonoBehaviour
{
    public IVehicle owner;
    public GameObject ownerGameObject;
    protected void Awake()
    {
        owner = ownerGameObject.GetComponent<IVehicle>();
        //Debug.Log(owner);
    }
}
