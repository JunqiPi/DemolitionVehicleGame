using Ln;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticle : MonoBehaviour, IRecyclable
{
    public ParticleSystem particle;

    public bool IsRecycled { get; set; }

    public int MaxCacheSize => 100;

    public RecyclableType RecyclableType => RecyclableType.ExplosionParticle;
    private Coroutine destoryCoroutine;

    public void OnInitialize()
    {
        gameObject.SetActive(true);
        particle.Play();
        if (destoryCoroutine != null)
        {
            StopCoroutine(destoryCoroutine);
        }
        destoryCoroutine = StartCoroutine(DestroyLater());
    }

    public void OnRecycle()
    {
        gameObject.SetActive(false);
        particle.Stop();
    }



    private IEnumerator DestroyLater()
    {

        yield return new WaitUntil(() => !particle.isPlaying);
        if (!ObjectPoolManager.Instance.Recycle(this))
        {
            Destroy(gameObject);
        }
    }
}
