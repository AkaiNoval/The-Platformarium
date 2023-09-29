using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavigationMeshBaker : MonoBehaviour
{
    public NavMeshSurface navMeshSurface { get; private set; }
    private void Awake() => navMeshSurface = GetComponent<NavMeshSurface>();
    public void BakeNavMeshWhenWorldChanged()
    {
        if (navMeshSurface == null) return;
        navMeshSurface.BuildNavMesh();
        Debug.Log("Baked NavMesh");
    }
}
