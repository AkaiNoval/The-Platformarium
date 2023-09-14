using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BuildingData", menuName ="PlacementSystem/Building")]
public class BuildingDataSO : ScriptableObject
{
    public string BuildingName;
    public Vector2Int BuildingSize;
    public List<MaterialsForBuilding> BuildingMaterialsCost;
    /* How many hits to contrust this building, hitting, ya know, thats why it is in int */
    public int BuildingHittingTime;
    public Building BuildingPrefab;

}

[System.Serializable]
public class MaterialsForBuilding
{
    public MaterialDataSO Material;
    public int Amount;
}
