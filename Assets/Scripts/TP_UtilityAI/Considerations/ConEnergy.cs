using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyConsideration", menuName = "UtilityAI/Considerations/Energy Consideration")]
public class ConEnergy : UConsideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(PrototypeController prototype)
    {
        return GetScoreBasedOnPrototypeEnergy(prototype);
    }
    float GetScoreBasedOnPrototypeEnergy(PrototypeController prototype)
    {
        return Score = responseCurve.Evaluate(prototype.Stats.Energy / prototype.Stats.SOPrototypeStats.MaxEnergy);
    }

}
