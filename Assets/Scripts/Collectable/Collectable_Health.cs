using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectable_Health : MonoBehaviour
{
    // Start is called before the first frame update
    //bool Pink=false;
    public float cooldown = 5f;
    [SerializeField] private AudioClip collectionSound;
    [SerializeField][Range(0f, 1f)] private float soundVolume = 0.5f;
    private AudioSource audioSource;

    private void SetupAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = soundVolume;
    }

    public void Start()
    {
        SetupAudio();
    }

    public void Update()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(CollectItem());
        }
    }

    private IEnumerator CollectItem()
    {
        Vehicle_Status.vehicle_Status.RegenerateHealth(20);
        // Play sound first
        if (collectionSound != null)
        {
            audioSource.PlayOneShot(collectionSound, soundVolume);
            gameObject.transform.localScale=Vector3.zero;
            // Wait for the sound to finish playing
            yield return new WaitForSeconds(collectionSound.length);
        }

        // Disable the object after playing the sound
        gameObject.SetActive(false);
        
        // Wait for cooldown
        yield return new WaitForSeconds(cooldown);
        
        // Reactivate the object
        gameObject.SetActive(true);
    }
}
