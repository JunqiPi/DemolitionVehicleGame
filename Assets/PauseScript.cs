using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{

    public static bool GAME_PAUSED = false;

    public Canvas canvas1;
    public Canvas canvas2;
    public GameObject panel1;
    public GameObject panel2;


    private void Start()
    {
        panel1 = canvas1.GetComponentInChildren<RectTransform>().gameObject;
        panel2 = panel2 == null ? canvas2.GetComponentInChildren<RectTransform>().gameObject : panel2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GAME_PAUSED)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Unpause()
    {
        print("unpausing");
        panel1.SetActive(true);
        panel2.SetActive(false);
        GAME_PAUSED = false;
        Time.timeScale = 1f;

    }

    void Pause()
    {
        print("pausing");
        panel1.SetActive(false);
        panel2.SetActive(true);
        GAME_PAUSED = true;
        Time.timeScale = 0f;
    }


}
