using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AssetsEnum;

public class GameAssets : MonoBehaviour
{
    #region Singleton
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            }
            return instance;
        }
        set => instance = value;
    } 
    #endregion

    [Header("Prefabs")]
    public List<PrefabsDictionary> MapElementPrefabsType;
    public List<PrefabsDictionary> CreationPrefabsType;
    public List<PrefabsDictionary> BuildingPrefabsType;
    public List<PrefabsDictionary> UIElementPrefabsType;
    [Header("ScriptableObjects")]
    public List<ScriptableObjectDictionary> BuildingSOs;
    public List<ScriptableObjectDictionary> PrototypeSOs;
    public List<ScriptableObjectDictionary> AnimalsSOs;

    public GameObject LoadPrefab(PrefabType prefabType, List<PrefabsDictionary> AllPrefabs)
    {
        foreach (PrefabsDictionary prefab in AllPrefabs)
        {
            if (prefab.prefabCatoragy == prefabType)
            {
                return prefab.prefabObject;
            }
        }  
        Debug.LogError("Prefab not found for type: " + prefabType);
        return null;
    }

    public ScriptableObject LoadScriptableObject(ScriptableObjectType objectType, List<ScriptableObjectDictionary> AllScriptableObjects)
    {
        foreach (ScriptableObjectDictionary soInfo in AllScriptableObjects)
        {
            if (soInfo.scriptableObjectType == objectType)
            {
                return soInfo.scriptableObject;
            }
        }
        Debug.LogError("ScriptableObject not found for type: " + objectType);
        return null;
    }
}

[Serializable]
public class PrefabsDictionary
{
    public PrefabType prefabCatoragy;
    //TODO: Enum?
    public string prefabName;
    public GameObject prefabObject;
}
[Serializable]
public class ScriptableObjectDictionary
{
    public ScriptableObjectType scriptableObjectType;
    public string scriptableObjectName;
    public ScriptableObject scriptableObject;
}

