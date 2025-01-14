using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameStart_Announcer : MonoBehaviour
{

    [Header("Audio Settings")]
    [SerializeField] private AudioSource announcerAudioSource;
    [SerializeField] private AudioClip[] countdownClips; // 3, 2, 1
    [SerializeField] private AudioClip startClip; // "destroy all other vehicle and Survive!"
    
    [Header("Text Display")]
    [SerializeField] private GameObject announcementText;
    [SerializeField] private float textDisplayDuration = 1f;
    [SerializeField] private float startTextDisplayDuration = 2f;
    [Header("OpeningAnimation")]
    [SerializeField] private OpeningAnimation openingAnimation;
    
    [Header("Events")]
    public UnityEvent onGameStart;
    
    public void Start()
    {
        // Ensure components are assigned
        if (announcerAudioSource == null)
            announcerAudioSource = gameObject.AddComponent<AudioSource>();
            
        if (announcementText == null)
            Debug.LogWarning("No text display assigned to DemoDerbyAnnouncer!");

        TriggerGameStart();
    }
    
    public void TriggerGameStart()
    {
        StartCoroutine(GameStartSequence());
    }
    
    private IEnumerator GameStartSequence()
    {
        
        // Play start announcement
        if (startClip != null)
        {
            announcerAudioSource.clip = startClip;
            announcerAudioSource.Play();
        }
        
        // Display start text
        if (announcementText != null)
        {
            announcementText.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(startTextDisplayDuration);
            announcementText.gameObject.SetActive(false);
            
            
        }
        yield return openingAnimation.Invoke();

        
        // Trigger game start event
        onGameStart?.Invoke();
    }
}
