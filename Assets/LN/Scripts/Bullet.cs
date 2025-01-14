using Ln;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, IRecyclable
{
    private readonly float damageValue = 30f;
    public float lifeTime = 2f;
    public int MaxCacheSize => 100;
    private bool isExploded;
    public RecyclableType RecyclableType => RecyclableType.Bullet;

    public bool IsRecycled { get; set; }
    public Coroutine destoryCoroutine;
    public ExplosionParticle explosionParticlePrefab;
    private void Start()
    {
        ObjectPoolManager.Instance.RegisterCreateFunc(RecyclableType.ExplosionParticle, () =>
        {
            ExplosionParticle explosionParticle = RecyclableFactory.Create(explosionParticlePrefab.gameObject) as ExplosionParticle;
            return explosionParticle;
        });
    }
    public void OnInitialize()
    {
        isExploded = false;
        gameObject.SetActive(true);
        if (destoryCoroutine != null)
        {
            StopCoroutine(destoryCoroutine);
        }
        destoryCoroutine = StartCoroutine(DestroyLater());
    }

    public void OnRecycle()
    {
        gameObject.SetActive(false);
    }



    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (!isExploded && !collision.gameObject.CompareTag("Player"))
        {
            Explosion();
            //
            if (collision.gameObject.TryGetComponent(out Enemy_Vehicle enemy))
            {
                enemy.TakeDamage(damageValue);
            }
            //
        }
    }
    void Explosion()
    {


        ExplosionParticle explosionParticle = ObjectPoolManager.Instance.PopAndInitializeObject(RecyclableType.ExplosionParticle) as ExplosionParticle;
        explosionParticle.gameObject.transform.position = transform.position;
        isExploded = true;
        Destroy();

    }
    void Destroy()
    {
        if (!ObjectPoolManager.Instance.Recycle(this))
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator DestroyLater()
    {

        yield return new WaitForSeconds(lifeTime);

        if (!IsRecycled && gameObject != null)
        {
            Destroy();
        }
    }
}
