using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : Singleton<MapController>
{
    public ProceduralTerrainGenerator ProceduralTerrain { get; private set; }
    public ProceduralResourceGenerator ProceduralResource { get; private set; }
    public NavigationMeshBaker NavMeshBaker { get; private set; }
    [field: SerializeField] public MapData MapData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        ProceduralTerrain = GetComponentInChildren<ProceduralTerrainGenerator>();
        ProceduralResource = GetComponentInChildren<ProceduralResourceGenerator>();
        NavMeshBaker = GetComponentInChildren<NavigationMeshBaker>();
    }
    private void Start()
    {
        ProceduralTerrain.ProceduralTerrainGenertaing(MapData.MapSize, MapData.TerrainNoiseScale,MapData.WaterLevel,MapData.IsIsland);
        ProceduralTerrain.OnGenerationCompleted += StartGeneratingResources;
        ProceduralTerrain.OnGenerationCompleted += NavMeshBaker.BakeNavMeshWhenWorldCreated;
    }
    private void StartGeneratingResources()
    {
        foreach (var resource in MapData.ResourcesData)
        {
            Debug.Log($"RESOURCE: {resource.ResourcesName} is generating...");
            ProceduralResource.ResourcesGeneration(ProceduralTerrain.GetMapGrid(),
                                                   resource.ResourcesName,
                                                   resource.ResourcesPrefabs,
                                                   resource.ResourceNoiseScale,
                                                   resource.ResourceDensity,
                                                   MapData.MapSize);
        }
    }

    private void OnDisable()
    {
        ProceduralTerrain.OnGenerationCompleted -= StartGeneratingResources;
        ProceduralTerrain.OnGenerationCompleted -= NavMeshBaker.BakeNavMeshWhenWorldCreated;
    }

}

[Serializable]
public class MapData
{
    [Header("Terrain Config")]
    /* Size of the map (assumed to be square)*/
    public int MapSize;

    /* Scale factor for generating Perlin noise*/
    /* A smaller value create larger, smoother features in the terrain, 
     * while a larger value will create smaller, more detailed features.*/
    public float TerrainNoiseScale = .1f;

    /* Threshold value for determining water cells */
    public float WaterLevel = .4f;

    /* Should be an island or a continuous terrain 
    * that covers the entire map without any significant gaps or separations.*/
    public bool IsIsland;
    [Header("Resources Config")]
    /* All the resources should the map generate */
    public List<ResourceData> ResourcesData;
}
