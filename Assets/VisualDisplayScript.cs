using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDisplayScript : MonoBehaviour
{

    public Camera cam1;
    public Camera cam2;

    public float switchDelay = 10f;


    // Start is called before the first frame update
    void Start()
    {
        // Activate cam1 and deactivate cam2 at the start
        cam1.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);

        StartCoroutine(ToggleCamerasAfterDelay(switchDelay));
    }

    IEnumerator ToggleCamerasAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Toggle camera states
        bool cam1Active = cam1.gameObject.activeSelf;
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
    }

}
