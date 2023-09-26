using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HarvestWoodConsideration", menuName = "UtilityAI/Considerations/HarvestWood Consideration")]
public class HarvestWoodConsideration : UConsideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(ColonistController colonist)
    {

        return GetScoreBasedOnTheStoragesCapacity();
    }
    float GetScoreBasedOnTheStoragesCapacity()
    {
        int allStorageCurrentCapacity = 0;
        int allStorageMaxCapacity = 0;
        List<Storage> storages = WorldContext.Instance.AllStorageInTheMap;
        foreach (Storage storage in storages)
        {
            allStorageCurrentCapacity += storage.CurrentCapacity;
            allStorageMaxCapacity += storage.MaxCapacity;
        }
        return Score = responseCurve.Evaluate(Mathf.Clamp01(allStorageCurrentCapacity / allStorageMaxCapacity));
    }
}
