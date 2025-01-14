using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBounce : MonoBehaviour
{

    public float extraBounce = 30f;

    void OnCollisionEnter(Collision collision)
    {

        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {


            Vector3 forceDirection = collision.contacts[0].normal;
            Vector3 reflectedVelocity = Vector3.Reflect(rb.velocity, forceDirection);


            Vector3 bounceForce = reflectedVelocity.normalized * extraBounce;


            rb.velocity = reflectedVelocity + bounceForce;

        }

    }
}

