using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chill", menuName = "UtilityAI/Actions/ActionChill")]
public class ActionChilling : UAction
{
    public override void Execute<T>(T colonist)
    {
        Debug.Log("Dancing and have some fun");
        colonist.PrototypeAnimController.PlayAnimation(animKey);
        colonist.UtilityBrain.FinishedExecutingBestAction = true;
    }

    public override void SetRequiredDestination<T>(T colonist)
    {
        colonist.MoveController.RequiredDestination = colonist.transform;
    }
}
