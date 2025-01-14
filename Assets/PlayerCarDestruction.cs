using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarDestruction : MonoBehaviour
{
    [SerializeField] private Transform bodyRoot;
    [SerializeField] private Transform wheels; 
    [SerializeField] private GameObject fireFab;
    [SerializeField] private AudioClip explosion;

    private AudioSource audioSource;

    void Start()
    {
        
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        //Need to change here to get called on collision damage <= 0
        
    }

    public IEnumerator DeconstructCar()
    {
        // Detach and add force to body parts
        PlayExplosionSound();
        DetachAllParts(bodyRoot, 50f, bodyRoot.position);

        yield return new WaitForSeconds(2f);

        Destroy(wheels.gameObject);
        Debug.Log("Wheels destroyed.");

        //foreach (Transform wheel in wheels)
        // {
        // DetachAndStopWheel(wheel, 10f);
        //}
    }

    private void DetachAllParts(Transform parent, float mass, Vector3 explosionOrigin)
    {
        foreach (Transform part in parent.GetComponentsInChildren<Transform>())
        {
            part.SetParent(null);
            Debug.Log("Detaching part: " + part.name);

            if (part == null)
            {
                Debug.LogWarning("Null child encountered in DetachAllParts");
                continue;
            }

            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = part.gameObject.AddComponent<Rigidbody>();
            }
            rb.mass = mass;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.AddExplosionForce(10f, explosionOrigin, 5f);

            if (part.GetComponent<MeshCollider>() == null)
            {
                MeshCollider meshCollider = part.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true; 
            }

            
            if (fireFab != null)
            {
                GameObject fireEffect = Instantiate(fireFab, part.position, Quaternion.identity);
                fireEffect.transform.SetParent(part);
            }
        }
    }

    private void PlayExplosionSound()
    {
        if (explosion != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosion);
        }
    }

    /*
    private void DetachAndStopWheel(Transform wheel, float mass)
    {
        
        wheel.SetParent(null);
       

       
        Rigidbody rb = wheel.gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false; 
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        
        MeshCollider meshCollider = wheel.gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        WheelCollider associatedWheelCollider = wheel.GetComponentInChildren<WheelCollider>();
        if (associatedWheelCollider != null)
        {
            Destroy(associatedWheelCollider);
        }
    }
    */
}
