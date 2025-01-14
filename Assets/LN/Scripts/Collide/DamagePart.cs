using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePart :VehiclePart
{   
    public float collisionDamageMultiplier=0.5f;
    readonly float MinDamageVelocity = 2.0f;
    readonly float damageCooldown = 2.0f;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hitting player");
        if (HasCommonAncestor(other.transform))
        {
            Debug.Log("returned");
            return;
        }
        VehiclePart vehiclePart = GetComponent<VehiclePart>();
        //Debug.Log(owner.Rigidbody.velocity.magnitude);
        if (!CheckAllowInflictDamage())
        {
            return;
        }
        // All parts other than front bumper is damage part
        //Debug.Log("collison name:"+other.transform.root.name);
        if (other.transform.root.CompareTag("Enemy"))
        {
            if (Time.time - vehiclePart.owner.lastDamageTime > damageCooldown)
            {   
                float damage=other.transform.root.GetComponent<VelocityReporter>().velocity.magnitude*collisionDamageMultiplier;
                
                Vehicle_Status.vehicle_Status.TakeDamage(damage);
                
                
                Debug.Log(vehiclePart.ownerGameObject.name + " take " + damage + " damage" + " The reason is that it collided with its " + other.gameObject.name);
                vehiclePart.owner.lastDamageTime = Time.time;
            }
            //Debug.Log();
        }
        
    }
    private float GetDamageValue()
    {
        return owner.Rigidbody.velocity.magnitude * collisionDamageMultiplier;
    }
    private bool CheckAllowInflictDamage()
    {
        bool isSufficientSpeed = owner.Rigidbody.velocity.magnitude > MinDamageVelocity;


        return isSufficientSpeed;
    }
    private bool HasCommonAncestor(Transform otherTransform)
    {
        Transform currentTransform = transform;

        while (currentTransform != null)
        {

            if (otherTransform.IsChildOf(currentTransform))
            {
                return true;
            }
            currentTransform = currentTransform.parent;
        }

        return false;
    }
}
