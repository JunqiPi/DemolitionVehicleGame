using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> collectables;
    [SerializeField] private int maxPickupsToSpawn = 5;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 10f);
    [SerializeField] private float spawnHeight = 0.5f;
    
    [Header("Timing")]
    [SerializeField] private int generalCooldown=5;
    [SerializeField] private float initialSpawnDelay = 2f;
    [SerializeField] private float checkInterval = 0.5f;
    
    [Header("VFX Settings")]
    [SerializeField] private Color spawnEffectColor = new Color(0, 1, 0, 1);
    [SerializeField] private float spawnEffectRadius = 1f;
    [SerializeField] private int particleCount = 50;

    
    [Header("Debug")]
    [SerializeField] private bool showSpawnArea = true;

    private GameObject currentPickup;
    private int pickupsSpawned = 0;
    private bool isWaitingForCollection = false;
    private ParticleSystem spawnVFX;
    private ParticleSystem collectionVFX;

    private void Start()
    {
        SetupVFXSystems();
        StartCoroutine(InitialSpawnRoutine());
    }

    private void SetupVFXSystems()
    {
        // Setup spawn VFX
        GameObject spawnVFXObject = new GameObject("SpawnVFX");
        spawnVFXObject.transform.parent = transform;
        spawnVFX = spawnVFXObject.AddComponent<ParticleSystem>();

        spawnVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var mainSpawn = spawnVFX.main;
        mainSpawn.duration = 1f;
        mainSpawn.loop = false;
        mainSpawn.startLifetime = 1f;
        mainSpawn.startSpeed = spawnEffectRadius;
        mainSpawn.startSize = 0.2f;
        mainSpawn.maxParticles = particleCount;
        mainSpawn.startColor = spawnEffectColor;

        var emissionSpawn = spawnVFX.emission;
        emissionSpawn.rateOverTime = 0;
        emissionSpawn.SetBurst(0, new ParticleSystem.Burst(0f, particleCount));

        var shapeSpawn = spawnVFX.shape;
        shapeSpawn.shapeType = ParticleSystemShapeType.Sphere;
        shapeSpawn.radius = 0.1f;

        var velocitySpawn = spawnVFX.velocityOverLifetime;
        velocitySpawn.enabled = true;
        velocitySpawn.radial = -2f;

        // Setup collection VFX (pre-instantiated)
        GameObject collectionVFXObject = new GameObject("CollectionVFX");
        collectionVFXObject.transform.parent = transform;
        collectionVFX = collectionVFXObject.AddComponent<ParticleSystem>();

        collectionVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var mainCollection = collectionVFX.main;
        mainCollection.duration = 0.5f;
        mainCollection.loop = false;
        mainCollection.startLifetime = 0.5f;
        mainCollection.startSpeed = 3f;
        mainCollection.startSize = 0.2f;
        mainCollection.maxParticles = 20;
        mainCollection.startColor = Color.white;

        var emissionCollection = collectionVFX.emission;
        emissionCollection.SetBurst(0, new ParticleSystem.Burst(0f, 20));

        var shapeCollection = collectionVFX.shape;
        shapeCollection.shapeType = ParticleSystemShapeType.Sphere;
        shapeCollection.radius = 0.1f;
    }

    private IEnumerator InitialSpawnRoutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        SpawnNewPickup();
        StartCoroutine(CheckPickupRoutine());
    }

    private IEnumerator CheckPickupRoutine()
    {
        while (pickupsSpawned < maxPickupsToSpawn)
        {
            if (currentPickup != null)
            {
                if (!currentPickup.activeInHierarchy && !isWaitingForCollection)
                {
                    isWaitingForCollection = true;
                    PlayCollectionEffects(currentPickup.transform.position);
                    yield return new WaitForSeconds(generalCooldown);
                    
                    if (pickupsSpawned < maxPickupsToSpawn)
                    {
                        Destroy(currentPickup);
                        SpawnNewPickup();
                        isWaitingForCollection = false;
                    }
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void SpawnNewPickup()
    {
        if (pickupsSpawned >= maxPickupsToSpawn) return;

        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, 2f);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                spawnPosition = GetRandomSpawnPosition();
                hitColliders = Physics.OverlapSphere(spawnPosition, 2f);
                if (hitColliders.Length == 0) break;
            }
        }

        PlaySpawnEffect(spawnPosition);
        StartCoroutine(SpawnPickupWithAnimation(spawnPosition));
    }

    private IEnumerator SpawnPickupWithAnimation(Vector3 spawnPosition)
    {
        Vector3 startPosition = spawnPosition + Vector3.up * 2f;
        int temp=collectables.Count;

        currentPickup = Instantiate(collectables[Random.Range(0,temp)], startPosition, Quaternion.identity);
        currentPickup.transform.SetParent(transform);
        
        float elapsedTime = 0f;
        float duration = 0.5f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = 1f - (1f - t) * (1f - t);
            currentPickup.transform.position = Vector3.Lerp(startPosition, spawnPosition, t);
            yield return null;
        }
        
        currentPickup.transform.position = spawnPosition;
        pickupsSpawned++;
    }

    private void PlaySpawnEffect(Vector3 position)
    {
        spawnVFX.transform.position = position;
        spawnVFX.Play();
    }

    private void PlayCollectionEffects(Vector3 position)
    {
        // Play collection particle effect
        collectionVFX.transform.position = position;
        collectionVFX.Clear();
        collectionVFX.Play();
        
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnHeight,
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );
    }

    private void OnDrawGizmos()
    {
        if (showSpawnArea)
        {
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawCube(transform.position + Vector3.up * spawnHeight, 
                new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
        }
    }

}
