using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Recording_SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip EnterClip;
    [SerializeField] private float minCollisionForce = 2f;
    [SerializeField] private float maxCollisionForce = 20f;
    
    // Only keeping pitch and echo controls
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;
    [SerializeField] private float echoDuration = 0.5f;

    void Start()
    {   

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        StartCoroutine(Entersound());
    }
    private IEnumerator Entersound()
    {
        yield return new WaitForSeconds(0.5f);
        
        // Add null check for safety
        if (audioSource != null && EnterClip != null)
        {
            audioSource.PlayOneShot(EnterClip, 1);
        }
        else
        {
            Debug.LogWarning("AudioSource or EnterClip is missing!");
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.relativeVelocity.magnitude;
        
        if (collisionForce >= minCollisionForce)
        {
            float normalizedForce = (collisionForce - minCollisionForce) / (maxCollisionForce - minCollisionForce);
            float volume = Mathf.Clamp01(normalizedForce) * 3;
            
            StartCoroutine(PlayEchoSound(volume, normalizedForce));
            
            Debug.Log($"Collision Force: {collisionForce}, Volume: {volume}");
        }
    }
    private IEnumerator PlayEchoSound(float volume, float normalizedForce)
    {
        if (audioSource != null && collisionSound != null)
        {
            // Initial impact with pitch variation
            audioSource.pitch = Mathf.Lerp(maxPitch, minPitch, normalizedForce);
            audioSource.PlayOneShot(collisionSound, volume);
            
            // Echo effect
            float elapsed = 0f;
            while (elapsed < echoDuration)
            {
                elapsed += Time.deltaTime;
                float echoVolume = volume * (1f - (elapsed / echoDuration));
                
                if (echoVolume > 0.1f)
                {
                    audioSource.pitch = Mathf.Lerp(maxPitch, minPitch, normalizedForce * (1f - elapsed/echoDuration));
                    audioSource.PlayOneShot(collisionSound, echoVolume);
                }
                
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void CollisionSound(float collisionIntensity)
    {
        if (audioSource != null && collisionSound != null)
        {
            float volume = Mathf.Clamp01(collisionIntensity);
            audioSource.PlayOneShot(collisionSound, volume * 3);
        }
    }
}