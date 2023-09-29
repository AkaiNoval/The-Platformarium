using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HarvestWoodConsideration", menuName = "UtilityAI/Considerations/HarvestWood Consideration")]
public class HarvestWoodConsideration : UConsideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(PrototypeController colonist)
    {

        return GetScoreBasedOnTheStoragesCapacity();
    }
    float GetScoreBasedOnTheStoragesCapacity()
    {
        List<Storage> storages = WorldContext.Instance.AllStorageOnTheMap;

        if (storages.Count == 0)
        {
            return 0;
        }

        int allStorageCurrentCapacity = 0;
        int allStorageMaxCapacity = 0;

        foreach (Storage storage in storages)
        {
            allStorageCurrentCapacity += storage.CurrentCapacity;
            allStorageMaxCapacity += storage.MaxCapacity;
        }

        float normalizedCapacity = (float)allStorageCurrentCapacity / allStorageMaxCapacity;
        return Score = responseCurve.Evaluate(Mathf.Clamp01(normalizedCapacity));
    }

}
