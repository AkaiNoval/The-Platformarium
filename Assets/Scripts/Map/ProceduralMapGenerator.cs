using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using Random = UnityEngine.Random;

public class ProceduralMapGenerator : MonoBehaviour
{
    private static ProceduralMapGenerator instance;
    public static ProceduralMapGenerator Instance 
    { 
        get => instance; 
        set => instance = value; 
    }
    public static event Action OnGenerationCompleted;

    /* Size of the map (assumed to be square)*/
    [SerializeField] int mapSize;
    //public int mapHeight;

    /* Scale factor for generating Perlin noise*/
    /* A smaller value create larger, smoother features in the terrain, 
     * while a larger value will create smaller, more detailed features.*/
    [SerializeField] float terrainNoiseScale = .1f;

    /* Threshold value for determining water cells */
    [SerializeField] float waterLevel = .4f;

    /* Should be an island or a continuous terrain 
     * that covers the entire map without any significant gaps or separations.*/
    [SerializeField] bool isIsland;
    [SerializeField] bool shouldGizmos;

    [Header("Cell Cube Prefab")]
    [SerializeField] Cell cellPrefab;
    [SerializeField] GameObject grassCubePrefab;
    [SerializeField] GameObject grassSurfaceOnlyPrefab;
    [Header("Resource Prefabs")]
    [SerializeField] List<ResourcePrefabData> resourcePrefabsData;
    Cell[,] grid;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance= this;
        }
    }
    private void Start()
    {
        grid = new Cell[mapSize, mapSize];
        FillAirCell(grid);
        FillTerrainCell(grid);
        StartCoroutine(FillTerrainWithGrassCube(grid));
    }
    #region MapGeneration
    void FillAirCell(Cell[,] localGrid)
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
    void FillTerrainCell(Cell[,] localGrid)
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

    IEnumerator FillTerrainWithGrassCube(Cell[,] localGrid)
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
                        GameObject grassCube = Instantiate(GetGrassType(grid, x, z), cellPos, randomRotation);
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
        foreach (var resouceData in resourcePrefabsData)
        {
            if (!resouceData.shouldSpawn) continue;
            Debug.Log("Generating..." + resouceData.ResourcesName);
            ResourcesGeneration(grid,
                resouceData.ResourcesName,
                resouceData.ResourcesPrefabs,
                resouceData.ResourceNoiseScale,
                resouceData.ResourceDensity);
        }
        OnGenerationCompleted?.Invoke();
    }
    GameObject GetGrassType(Cell[,] localGrid, int x, int z)
    {
        Cell cell = localGrid[x, z];
        // Check if there is another grass cube on the left side
        if (x > 0)
        {
            Cell leftCell = grid[x - 1, z];
            if (leftCell.cellType != CellType.Grass)
            {
                return grassCubePrefab; // Early return if left cell is not grass
            }
        }
        // Check if there is another grass cube on the right side
        if (x < mapSize - 1)
        {
            Cell rightCell = grid[x + 1, z];
            if (rightCell.cellType != CellType.Grass)
            {
                return grassCubePrefab; // Early return if right cell is not grass
            }
        }
        // Check if there is another grass cube on the lower side
        if (z > 0)
        {
            Cell lowerCell = grid[x, z - 1];
            if (lowerCell.cellType != CellType.Grass)
            {
                return grassCubePrefab; // Early return if lower cell is not grass
            }
        }
        // Check if there is another grass cube on the upper side
        if (z < mapSize - 1)
        {
            Cell upperCell = grid[x, z + 1];
            if (upperCell.cellType != CellType.Grass)
            {
                return grassCubePrefab; // Early return if upper cell is not grass
            }
        }
        if (z == 0 || x == 0 || x == mapSize - 1 || z == mapSize - 1 && !isIsland)
        {
            return grassCubePrefab;
        }
        // If all surrounding cells are grass, assign grassSurfaceOnlyPrefab
        return grassSurfaceOnlyPrefab;
    }
    int RandomRotationValue()
    {
        int[] rotationValues = { 0, 90, -90, 180 };
        int randomIndex = Random.Range(0, rotationValues.Length);
        return rotationValues[randomIndex];
    }
    #endregion

    void ResourcesGeneration(Cell[,] localGrid,
                             string resouceName,
                             List<GameObject> ResourcesPrefabs,
                             float resourceNoiseScale,
                             float resourceDensity)
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
        for(int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                Cell cell = localGrid[x, z];

                if(cell.cellType == CellType.Grass && !cell.IsThisCellOccupied())
                {
                    float value = Random.Range(0f, resourceDensity);
                    if (noiseMap[x, z] < value)
                    {
                        GameObject resourcePrefab = ResourcesPrefabs[Random.Range(0, ResourcesPrefabs.Count)];
                        GameObject resource = Instantiate(resourcePrefab, transform);
                        resource.transform.position = new Vector3(x, 1, z);
                        resource.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        resource.name = $"{resouceName}: ({x},{z})";
                        resource.transform.parent = resourcesParent.transform;
                        /* Succesfully created a resouce node, set this cell to occupied */
                        cell.UpdateOccupationStatus(true);
                    }
                }
            }
        }
    }

    public Cell[,] GetMapGrid() => grid;
    public void UpdateCellOccupationStatus(bool occupationStatus, List<Cell> cellsToUpdate)
    {
        foreach (var cell in cellsToUpdate)
        {
            cell.UpdateOccupationStatus(occupationStatus);
        }
    }
    // Method to get the position of a Cell in the grid
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (!shouldGizmos) return;

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Cell cell = grid[x, z];
                switch (cell.cellType)
                {
                    case CellType.Air:
                        Gizmos.color = Color.white;
                        return;
                    case CellType.Water:
                        Gizmos.color = Color.blue;
                        break;
                    case CellType.Grass:
                        Gizmos.color = Color.green;
                        break;
                    default:
                        break;
                }
                Vector3 pos = new Vector3(x, 0, z);
                Gizmos.DrawWireCube(pos, Vector3.one);
            }

        }

    }
}

[Serializable]
public class ResourcePrefabData
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
