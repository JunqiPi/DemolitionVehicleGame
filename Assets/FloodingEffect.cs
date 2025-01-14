using System.Collections;
using UnityEngine;

public class FloodingEffect : MonoBehaviour
{
    public ParticleSystem[] waterfallParticles;
    public GameObject floodingObject; 
    public float floodSpeed = 0.001f;
    public float maxFloodHeight = 0f;

    private float particleDelay;
    private Vector3 initialFloodPosition;

    public float[] delayrange = new float[] { 3f, 7f };

    void Start()
    {
     

        maxFloodHeight = 0f;
        initialFloodPosition = floodingObject.transform.position;
        

        StartCoroutine(StartWaterfallAndFlood());
        
       
    }

    IEnumerator StartWaterfallAndFlood()
    {
      

        float delay = Random.Range(delayrange[0], delayrange[1]);
        yield return new WaitForSeconds(delay);

      
        foreach (ParticleSystem ps in waterfallParticles)
        {

            ps.Play();
            
        }

        yield return new WaitForSeconds(0.1f);

        
        yield return StartCoroutine(StartFlooding());


    }

    IEnumerator StartFlooding()
    {
       
        while (floodingObject.transform.position.y < maxFloodHeight)
        {
            
            float newY = Mathf.Min(floodingObject.transform.position.y + (floodSpeed * 0.001f), maxFloodHeight);
            floodingObject.transform.position = new Vector3(
                floodingObject.transform.position.x,
                newY,
                floodingObject.transform.position.z
            );

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        ResetFlood();
    }

    public void ResetFlood()
    {
        
        if (floodingObject != null)
        {
            //Debug.Log("Reset Flood " + floodingObject.transform.position.y);
            floodingObject.transform.position = initialFloodPosition;
        }
    }
}
