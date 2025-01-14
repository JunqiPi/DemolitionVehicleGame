using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackPart : VehiclePart
{
    readonly float DamageMultiplier = 1.0f;
    readonly float MinDamageVelocity = 2.0f;
    //readonly float damageCooldown = 2.0f;

    
    private void OnTriggerEnter(Collider other)
    {
        if (HasCommonAncestor(other.transform))
        {
            return;
        }
        //VehiclePart vehiclePart = GetComponent<VehiclePart>();
        //Debug.Log(owner.Rigidbody.velocity.magnitude);
        // if (!CheckAllowInflictDamage())
        // {
        //     return;
        // }
        // All parts other than front bumper is damage part
        // if (vehiclePart is DamagePart || other.gameObject.CompareTag("Enemy"))
        // {
        //     if (Time.time - vehiclePart.owner.lastDamageTime > damageCooldown)
        //     {   
        //         float damage=GetDamageValue();
        //         if(other.transform.root.CompareTag("Enemy")){
        //             Vehicle_Status.vehicle_Status.TakeDamage(damage);
                    
        //         }
        //         if(other.transform.root.CompareTag("Player")){
        //             Enemy_Vehicle.enemy_Vehicle.TakeDamage(damage);
        //         }
                
        //         Debug.Log(vehiclePart.ownerGameObject.name + " take " + damage + " damage" + " The reason is that it collided with its " + other.gameObject.name);
        //         vehiclePart.owner.lastDamageTime = Time.time;
        //     }
        //     //Debug.Log();
        // }
        // else if (vehiclePart is AttackPart)
        // {
        //     Debug.Log("no damage " + " The reason is that it collided with its " + other.gameObject.name);

        // }
    }
    private float GetDamageValue()
    {
        return owner.Rigidbody.velocity.magnitude * DamageMultiplier;
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
