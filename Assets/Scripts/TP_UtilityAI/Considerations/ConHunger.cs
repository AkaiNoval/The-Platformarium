using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "ConHunger", menuName = "UtilityAI/Considerations/ConHunger Consideration")]
public class ConHunger : UConsideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(PrototypeController prototype)
    {
        return GetScoreBasedOnPrototypeEnergy(prototype);
    }
    float GetScoreBasedOnPrototypeEnergy(PrototypeController prototype)
    {
        return Score = responseCurve.Evaluate(prototype.Stats.Hunger / prototype.Stats.SOPrototypeStats.MaxHunger);
    }
}
