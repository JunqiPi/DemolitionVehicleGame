using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMerger : MonoBehaviour
{
    public GameObject[] objectsToMerge;
    //public string gameObjectName = "Merged Mesh";
    void Start()
    {
        MergeMeshes();
    }

    void MergeMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0)
        {
            return;
        }

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh mergedMesh = new Mesh();
        mergedMesh.CombineMeshes(combine);

        //GameObject mergedObject = new GameObject(gameObjectName);
        //mergedObject.transform.SetParent(transform);
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mergedMesh;
        GetComponent<MeshCollider>().sharedMesh = mergedMesh;
        //mergedObject.AddComponent<MeshRenderer>().material = meshFilters[0].GetComponent<MeshRenderer>().material;

        foreach (var meshFilterToDestroy in meshFilters)
        {
            if (meshFilterToDestroy.transform == transform)
            {
                return;
            }
            Destroy(meshFilterToDestroy.gameObject);
        }
    }
}
