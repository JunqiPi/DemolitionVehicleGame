using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime = 1f;

    public GameObject menu;

    public int index = 1;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    LoadNextLevel();
        //}


        
    }

    public void LoadIndexNumberPlus(int index)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + index));
    }

    public void LoadSpecificIndex(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // play animation
        transition.SetTrigger("Start");

        // wait for anim
        yield return new WaitForSeconds(transitionTime);

        //change scene
        SceneManager.LoadScene(levelIndex);

    }
}
