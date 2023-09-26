using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface Clickable, Buildable, Destroyable, NPCInteractable
public abstract class Building : MonoBehaviour
{
    [SerializeField] BuildingDataSO buildingDataSO;
    [SerializeField] List<Renderer> renderers;
    public List<Renderer> GetAllRenderersInTheBuilding() => renderers;
}
