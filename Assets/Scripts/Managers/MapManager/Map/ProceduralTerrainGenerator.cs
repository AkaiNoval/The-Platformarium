using System;
using System.Collections;
using System.Collections.Generic;
using TP.Resource;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralTerrainGenerator : MonoBehaviour
{
    public static event Action OnGenerationCompleted;

    /* Size of the map (assumed to be square)*/

    //[SerializeField] int mapSize;

    /* Scale factor for generating Perlin noise*/
    /* A smaller value create larger, smoother features in the terrain, 
     * while a larger value will create smaller, more detailed features.*/
    //[SerializeField] float terrainNoiseScale = .1f;

    /* Threshold value for determining water cells */
    //[SerializeField] float waterLevel = .4f;

    /* Should be an island or a continuous terrain 
     * that covers the entire map without any significant gaps or separations.*/
    //[SerializeField] bool isIsland;

    [Header("Cell Cube Prefab")]
    [SerializeField] Cell cellPrefab;
    [SerializeField] Cube grassCubePrefab;
    Cell[,] grid;
    public void ProceduralTerrainGenertaing(int mapSize, float terrainNoiseScale, float waterLevel, bool isIsland)
    {
        grid = new Cell[mapSize, mapSize];
        FillAirCell(grid, mapSize);
        FillTerrainCell(grid, mapSize, terrainNoiseScale, waterLevel, isIsland);
        StartCoroutine(FillTerrainWithGrassCube(grid, mapSize, isIsland));
    }

    #region MapGeneration
    private void FillAirCell(Cell[,] localGrid, int mapSize)
    {

        /* Responsible to hold all cells*/
        GameObject Cells = new GameObject();
        Cells.name = "Cells";
        Cells.transform.parent = transform;
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                var newPos = new Vector3(x, 0, z);
                var cell = Instantiate(cellPrefab, newPos, Quaternion.identity);
                cell.name = $"Cell x: {x} z: {z}";
                cell.transform.SetParent(Cells.transform, false);
                cell.cellType = CellType.Air;
                localGrid[x, z] = cell;
            }
        }

    }
    private void FillTerrainCell(Cell[,] localGrid, int mapSize, float terrainNoiseScale, float waterLevel, bool isIsland)
    {
        // Generate Perlin noise map
        float[,] noiseMap = new float[mapSize, mapSize];

        float offset = 1000000;
        float randomX = Random.Range(-offset, offset);
        float randomZ = Random.Range(-offset, offset);

        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * terrainNoiseScale + randomX, z * terrainNoiseScale + randomZ);
                noiseMap[x, z] = noiseValue;
            }
        }
        // Create a 2D array to store the falloff map
        float[,] falloffMap = new float[mapSize, mapSize];
        if (isIsland)
        {
            // Loop through each cell of the falloffMap
            for (int z = 0; z < mapSize; z++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    // Calculate the normalized position within the range -1 to 1
                    float xv = x / (float)mapSize * 2 - 1;
                    float zv = z / (float)mapSize * 2 - 1;

                    // Calculate the maximum absolute value between xv and zv
                    // Represents the distance from the center of the map to the current cell
                    float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(zv));

                    // Apply the falloff function to determine the falloff value
                    falloffMap[x, z] = Mathf.Pow(v, 5f) / (Mathf.Pow(v, 5f) + Mathf.Pow(2.2f - 2.2f * v, 5f));
                }
            }
        }
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Cell cell = localGrid[x, z];
                float noiseValue = noiseMap[x, z];
                if (isIsland)
                {
                    noiseValue -= falloffMap[x, z];
                }
                // Determine if the cell is water based on noise value
                if (noiseValue < waterLevel)
                {
                    cell.cellType = CellType.Water;
                }
                else
                {
                    cell.cellType = CellType.Grass;
                }
                grid[x, z] = cell;
            }

        }

    }
    IEnumerator FillTerrainWithGrassCube(Cell[,] localGrid, int mapSize, bool isIsland)
    {
        GameObject Terrain = new GameObject("Terrain");
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Cell cell = localGrid[x, z];
                if (cell.cellType == CellType.Grass)
                {
                    Vector3 cellPos = new Vector3(x, 0, z);
                    Quaternion randomRotation = Quaternion.Euler(0, RandomRotationValue(), 0);
                    GameObject grassCube = Instantiate(GetGrassType(grid, x, z, mapSize, isIsland).gameObject, cellPos, randomRotation);
                    if (grassCube.TryGetComponent(out Cube cube))
                    {
                        cube.CurrentCell = cell;
                    }
                    grassCube.name = $"GrassCube x: {x} z: {z}";
                    grassCube.transform.parent = Terrain.transform;
                    yield return null;
                }
            }
        }
        //foreach (var resouceData in resourcePrefabsData)
        //{
        //    if (!resouceData.shouldSpawn) continue;
        //    Debug.Log("Generating..." + resouceData.ResourcesName);
        //    ResourcesGeneration(grid,
        //        resouceData.ResourcesName,
        //        resouceData.ResourcesPrefabs,
        //        resouceData.ResourceNoiseScale,
        //        resouceData.ResourceDensity);
        //}
        FillInactiveGrassCube(localGrid, Terrain, mapSize, isIsland);
        OnGenerationCompleted?.Invoke();
    }
    private void FillInactiveGrassCube(Cell[,] localGrid, GameObject Terrain, int mapSize, bool isIsland)
    {
        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                Cell cell = localGrid[x, z];
                if (cell.cellType == CellType.Water)
                {
                    Vector3 cellPos = new Vector3(x, 0, z);
                    Quaternion randomRotation = Quaternion.Euler(0, RandomRotationValue(), 0);
                    GameObject grassCube = Instantiate(GetGrassType(grid, x, z, mapSize, isIsland).gameObject, cellPos, randomRotation);
                    if (grassCube.TryGetComponent(out Cube cube))
                    {
                        cube.CurrentCell = cell;
                    }
                    grassCube.name = $"GrassCube x: {x} z: {z}";
                    grassCube.transform.parent = Terrain.transform;
                    grassCube.SetActive(false);
                }
            }
        }
    }

    Cube GetGrassType(Cell[,] localGrid, int x, int z, int mapSize, bool isIsland)
    {
        Cell cell = localGrid[x, z];
        Cube grassCubeType = grassCubePrefab;
        grassCubeType.SetActiveDirtPlatform(true);
        // Check if there is another grass cube on the left side
        if (x > 0)
        {
            Cell leftCell = grid[x - 1, z];
            if (leftCell.cellType != CellType.Grass)
            {
                return grassCubeType; // Early return if left cell is not grass
            }
        }
        // Check if there is another grass cube on the right side
        if (x < mapSize - 1)
        {
            Cell rightCell = grid[x + 1, z];
            if (rightCell.cellType != CellType.Grass)
            {
                return grassCubeType; // Early return if right cell is not grass
            }
        }
        // Check if there is another grass cube on the lower side
        if (z > 0)
        {
            Cell lowerCell = grid[x, z - 1];
            if (lowerCell.cellType != CellType.Grass)
            {
                return grassCubeType; // Early return if lower cell is not grass
            }
        }
        // Check if there is another grass cube on the upper side
        if (z < mapSize - 1)
        {
            Cell upperCell = grid[x, z + 1];
            if (upperCell.cellType != CellType.Grass)
            {
                return grassCubeType; // Early return if upper cell is not grass
            }
        }
        if (z == 0 || x == 0 || x == mapSize - 1 || z == mapSize - 1 && !isIsland)
        {
            return grassCubeType;
        }
        // If all surrounding cells are grass, assign grassSurfaceOnlyPrefab
        grassCubeType.SetActiveDirtPlatform(false);
        return grassCubeType;
    }
    int RandomRotationValue() => Random.Range(0, 4) * 90; // 0, 90, -90, or 180
    #endregion
    void ResourcesGeneration(Cell[,] localGrid,
                             string resouceName,
                             List<GameObject> ResourcesPrefabs,
                             float resourceNoiseScale,
                             float resourceDensity
        , int mapSize)
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

    #region Helper Methods

    public Cell[,] GetMapGrid() => grid;
    public void UpdateCellOccupationStatus(bool occupationStatus, List<Cell> cellsToUpdate)
    {
        foreach (var cell in cellsToUpdate)
        {
            cell.UpdateOccupationStatus(occupationStatus);
        }
    } 
    #endregion

    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying) return;
    //    if (!shouldGizmos) return;

    //    for (int x = 0; x < mapSize; x++)
    //    {
    //        for (int z = 0; z < mapSize; z++)
    //        {
    //            Cell cell = grid[x, z];
    //            switch (cell.cellType)
    //            {
    //                case CellType.Air:
    //                    Gizmos.color = Color.white;
    //                    return;
    //                case CellType.Water:
    //                    Gizmos.color = Color.blue;
    //                    break;
    //                case CellType.Grass:
    //                    Gizmos.color = Color.green;
    //                    break;
    //                default:
    //                    break;
    //            }
    //            Vector3 pos = new Vector3(x, 0, z);
    //            Gizmos.DrawWireCube(pos, Vector3.one);
    //        }

    //    }

    //}
}
