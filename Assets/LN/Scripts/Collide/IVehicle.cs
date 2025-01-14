using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IVehicle
{

    public Rigidbody Rigidbody { get; }
    public float lastDamageTime { get; set; }
}
