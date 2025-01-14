using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EntryScript : MonoBehaviour
{
     [Header("Video Settings")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject levelLoader;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject music;
    [SerializeField] private bool canSkip = true;
    
    // Optional events for when video starts/ends
    public System.Action onVideoStart;
    public System.Action onVideoEnd;

    private void Start()
    {
        videoPlayer.targetCamera = Camera.main;
        levelLoader.SetActive(false);
        music.SetActive(false);
        UI.SetActive(false);
        // If VideoPlayer component isn't assigned, try to get it
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            
            if (videoPlayer == null)
            {
                Debug.LogError("No VideoPlayer component found! Adding one...");
                videoPlayer = gameObject.AddComponent<VideoPlayer>();
            }
        }

        // Setup video player then play on start
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.started += OnVideoStarted;
        
        PlayVideo();
    }

    public void Update()
    {

        // Skip video if allowed and user presses any key or touches screen
        if (canSkip && videoPlayer.isPlaying && (Input.anyKeyDown || Input.touchCount > 0))
        {
            StopVideo();
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            onVideoEnd?.Invoke();
        }
    }

    private void OnVideoStarted(VideoPlayer vp)
    {
        onVideoStart?.Invoke();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        onVideoEnd?.Invoke();
        levelLoader.SetActive(true);
        music.SetActive(true);
        UI.SetActive(true);
        Destroy(this.gameObject);
    }

}
