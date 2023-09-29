using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrototypeData", menuName = "CreationSystem/Prototype")]
public class SOPrototype : ScriptableObject
{
    public string prototypeName;
    public int creationPoint;
    public GameObject BuildingPrefab;
    public float MaxHealth;
    public float MaxHunger;
    public float MaxEnergy;
}
