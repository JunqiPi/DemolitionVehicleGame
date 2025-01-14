using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Damage : MonoBehaviour
{
     // Start is called before the first frame update
    //public GameObject gameObject;
    void Start (){

    }
    public float collisionDamageMultiplier=0.5f;
    public void OnCollisionEnter(Collision collision){

        float collisionDamage=1f;

        if(collision.gameObject.tag=="Enemy"){

            float impactSpeed=collision.gameObject.GetComponent<VelocityReporter>().velocity.magnitude; //translate the velocity to magnitude
            Debug.Log($"The magnitude of enemy is {impactSpeed}.");
            collisionDamage=impactSpeed*collisionDamageMultiplier; //then apply the collision damage
            gameObject.GetComponent<Vehicle_Status>().TakeDamage(collisionDamage);
        }

    }
}
