using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePrefabs : MonoBehaviour
{
    private static GamePrefabs instance;

    public static GamePrefabs Instance 
    {
        get 
        { 
            if(instance == null)
            {
                instance = (Instantiate(Resources.Load("GamePrefabs")) as GameObject).GetComponent<GamePrefabs>();
            }
            return instance; 
        }
        set => instance = value; 
    }

    [Header("Prefabs")]
    public List<PrefabsDictionary> AllPrefabs;

    public GameObject LoadPrefab(string prefabName)
    {
        PrefabsDictionary prefabInfo = AllPrefabs.Find(p => p.prefabName == prefabName);
        if (prefabInfo != null)
        {
            return prefabInfo.prefabObject;
        }
        else
        {
            Debug.LogError("Prefab not found: " + prefabName);
            return null;
        }
    }
}

[Serializable]
public class PrefabsDictionary
{
    public string prefabName;
    public GameObject prefabObject;
}

