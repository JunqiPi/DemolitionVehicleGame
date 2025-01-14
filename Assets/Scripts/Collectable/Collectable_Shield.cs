using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Shield : MonoBehaviour
{
    // Start is called before the first frame update
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
        Vehicle_Status.vehicle_Status.Shielding();
        // Play sound first
        if (collectionSound != null)
        {
            audioSource.PlayOneShot(collectionSound, soundVolume);
            // Wait for the sound to finish playing
            gameObject.transform.localScale=Vector3.zero;
            yield return new WaitForSeconds(collectionSound.length);
        }
        gameObject.SetActive(false);
    }
}
