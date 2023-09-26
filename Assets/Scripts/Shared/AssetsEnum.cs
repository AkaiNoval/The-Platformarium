using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetsEnum 
{
    public enum PrefabType
    {
        /* MapelementPrefabType*/
        Cell,
        GrassCube,
        HighLightCell,
        /* CreationPrefabType */
        Prototype,
        Animal,
        /* BuildingPrefabType */
        Tent,
        BigTent,
        Farm,
        WoodStorage,
        StoneBigStorage,
        /* UIElementPrefabType */
        UIItemContent,
    }
    public enum ScriptableObjectType
    {
        /* CreationSOType */
        Prototype,
        Tent,
        BigTent,
        Farm,
        WoodStorage,
        StoneBigStorage
    }
}
