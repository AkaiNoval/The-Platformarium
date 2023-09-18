using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavigationMeshBaker : MonoBehaviour
{
    NavMeshSurface navMeshSurface;
    private void Awake() => navMeshSurface = GetComponent<NavMeshSurface>();
    private void OnEnable() => ProceduralTerrainGenerator.OnGenerationCompleted += BakeNavMeshWhenWorldCreated;
    private void OnDisable() => ProceduralTerrainGenerator.OnGenerationCompleted -= BakeNavMeshWhenWorldCreated;
    void BakeNavMeshWhenWorldCreated()
    {
        if (navMeshSurface == null) return;
        navMeshSurface.BuildNavMesh();
        Debug.Log("Baked NavMesh");
    }
}
