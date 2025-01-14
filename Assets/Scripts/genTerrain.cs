using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genTerrain : MonoBehaviour
{
    public Terrain terrain;
    public float bumps = 0.01f;
    public float broadness = 0.03f;
    private float xOffset;
    private float yOffset;

    public GameObject healthPack;
    public Vector2 arenaBottomLeft;
    public Vector2 arenaTopRight;
    public float packHeight = 1.0f;
    public int healthPacks = 5;



    void Start()
    {
       
        xOffset = Random.Range(0f, 10000f);
        yOffset = Random.Range(0f, 10000f);



        FlattenTerrain();
        GenerateRandomTerrain();
        
    }

   
    void FlattenTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        int heightMapWidth = terrainData.heightmapResolution;
        int heightMapHeight = terrainData.heightmapResolution;

        //https://discussions.unity.com/t/generating-a-simple-random-terrain/620870/4

        float[,] flats = new float[heightMapWidth, heightMapHeight];

        for (int x = 0; x < heightMapWidth; x++)
        {
            for (int y = 0; y < heightMapHeight; y++)
            {
                flats[x, y] = 0f; 
            }
        }

        terrainData.SetHeights(0, 0, flats);
    }

    void GenerateRandomTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        int heightMapWidth = terrainData.heightmapResolution;
        int heightMapHeight = terrainData.heightmapResolution;

        //https://discussions.unity.com/t/generating-a-simple-random-terrain/620870/4 <- source for starter code

        float[,] heights = terrainData.GetHeights(0, 0, heightMapWidth, heightMapHeight);

        for (int x = 0; x < heightMapWidth; x++)
        {
            for (int y = 0; y < heightMapHeight; y++)
            {
                float xCoord = x * broadness + xOffset;
                float yCoord = y * broadness + yOffset;

                
                float heightValue = Mathf.PerlinNoise(xCoord, yCoord) * bumps;

                heightValue += Mathf.PerlinNoise(xCoord * 0.5f, yCoord * 0.5f) * (bumps *0.5f);

            
                heightValue += Mathf.PerlinNoise(xCoord * 0.25f, yCoord * 0.25f) * (bumps*0.25f);

                heights[x, y] = heightValue;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }

    void SpawnHealth()
    {
        for (int i = 0; i < healthPacks; i++)
        {

            float xPos = Random.Range(arenaBottomLeft.x, arenaTopRight.x);
            float zPos = Random.Range(arenaBottomLeft.y, arenaTopRight.y);


            float yPos = terrain.SampleHeight(new Vector3(xPos, 0, zPos)) + packHeight;

            Vector3 healthPos = new Vector3(xPos, yPos, zPos);

        }

    }
}
