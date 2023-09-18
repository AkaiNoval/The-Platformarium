using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TP.Resource;

public class ProceduralResourceGenerator : MonoBehaviour
{
    public void ResourcesGeneration(Cell[,] localGrid,
                            string resouceName,
                            List<GameObject> ResourcesPrefabs,
                            float resourceNoiseScale,
                            float resourceDensity,
                            int mapSize)
    {
        /* Responsible to hold all specific resource type*/
        GameObject resourcesParent = new GameObject();
        resourcesParent.name = resouceName;
        resourcesParent.transform.parent = transform;

        float[,] noiseMap = new float[mapSize, mapSize];
        float offset = 1000000;
        float randomX = Random.Range(-offset, offset);
        float randomZ = Random.Range(-offset, offset);
        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * resourceNoiseScale + randomX, z * resourceNoiseScale + randomZ);
                noiseMap[x, z] = noiseValue;
            }
        }
        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                Cell cell = localGrid[x, z];

                if (cell.cellType == CellType.Grass && !cell.IsThisCellOccupied())
                {
                    /* If there are any impossible case like the Cube of that Cell is deactived? DO NOT SPAWN RESOURCE ON THAT*/
                    if (!cell.GetCurrentCube().gameObject.activeSelf) continue;
                    float value = Random.Range(0f, resourceDensity);
                    if (noiseMap[x, z] < value)
                    {
                        GameObject resourcePrefab = ResourcesPrefabs[Random.Range(0, ResourcesPrefabs.Count)];
                        GameObject resource = Instantiate(resourcePrefab, transform);
                        resource.transform.position = new Vector3(x, 1, z);
                        resource.transform.rotation = Quaternion.Euler(0, RandomRotationValue(), 0);
                        resource.name = $"{resouceName}: ({x},{z})";
                        resource.transform.parent = resourcesParent.transform;
                        if (resource.TryGetComponent(out Resource r))
                        {
                            r.CellPosition = cell;
                        }
                        /* Succesfully created a resouce node, set this cell to occupied */
                        cell.UpdateOccupationStatus(true);
                    }
                }
            }
        }
    }
    int RandomRotationValue() => Random.Range(0, 4) * 90;
}

[Serializable]
public class ResourceData
{
    public string ResourcesName;
    public List<GameObject> ResourcesPrefabs;
    /* It determines the "clumpiness" of tree placement */
    /* Smaller values will lead to more scattered resources, while larger values will create denser clusters of resources.*/
    public float ResourceNoiseScale;
    /* Resource distribution*/
    [Range(0f, 1f)]
    public float ResourceDensity;
    public bool shouldSpawn;
}