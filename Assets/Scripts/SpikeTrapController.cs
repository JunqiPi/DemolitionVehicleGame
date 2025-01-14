using System.Collections;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    public Animator spikeAnimator;  // Drag the Animator component here in the Inspector
    public float minTime = 10f;      // Minimum time before spikes activate
    public float maxTime = 20f;      // Maximum time before spikes activate

    void Start()
    {
        // Start the coroutine to randomly play the animation
        
        StartCoroutine(ActivateSpikesRandomly());
        
    }

    IEnumerator ActivateSpikesRandomly()
    {
        while (true)
        {
            // Wait for a random time between minTime and maxTime
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            // Trigger the animation (assuming you use a trigger parameter)
            spikeAnimator.SetTrigger("Activate");
        }
    }
}
