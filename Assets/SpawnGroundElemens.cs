using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundElemens : MonoBehaviour
{
    public GameObject barrels;
    public GameObject traps;
    public GameObject tires;
    public GameObject ground;

    public int minSpawnCount = 2;
    public int maxSpawnCount = 4;
    public float spawnAreaMin = -115f;
    public float spawnAreaMax = 115f;

    void Start()
    {
        SpawnGroundElements();
    }

    void SpawnGroundElements()
    {
        
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        if (barrels != null) SpawnObjects(barrels, spawnCount);
        if (traps != null) SpawnObjects(traps, spawnCount);
        if (tires != null) SpawnObjects(tires, spawnCount);
    }

    void SpawnObjects(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float rX = Random.Range(spawnAreaMin, spawnAreaMax);
            float rZ = Random.Range(spawnAreaMin, spawnAreaMax);

            float groundY = ground.transform.position.y;

            Vector3 spawnPosition;

            if (prefab == barrels)
            {
                spawnPosition = new Vector3(rX, groundY + 3f, rZ);
            }
            else
            {
                spawnPosition = new Vector3(rX, groundY + 0.15f, rZ);
            }

         
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}
