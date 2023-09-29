using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChillingConsideration", menuName = "UtilityAI/Considerations/Chilling Consideration")]
public class ConChilling : UConsideration
{

    public override float ScoreConsideration(PrototypeController prototype)
    {
        return 0.01f;
    }
}
