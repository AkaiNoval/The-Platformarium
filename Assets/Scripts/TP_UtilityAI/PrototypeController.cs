using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum FSMState
{
    Decide,
    Move,
    Perform
}
public class PrototypeController : MonoBehaviour
{
    public UMovementController MoveController { get; set; }
    public UBrain UtilityBrain { get; set; }

    //public ColonistStats Stats { get; set; }

    //public ColonistBehaviour Behaviour { get; set; }

    public FSMState currentState;
    private void Awake()
    {
        MoveController = GetComponent<UMovementController>();
        UtilityBrain = GetComponent<UBrain>();
    }
    private void Update()
    {
        FSMTick();
    }
    void FSMTick()
    {
        if (currentState == FSMState.Decide)
        {
            /* Make the AI decide on the best action */
            UtilityBrain.DecideBestAction();

            /* Calculate the distance between AI and the required destination of the best action */
            float distance = Vector3.Distance(MoveController.RequiredDestination.position, this.transform.position);

            if (distance < 1f)
            {
                /* If AI is close enough, switch to the Perform state */
                Debug.Log("If AI is close enough, Decide to the Perform state");
                currentState = FSMState.Perform;
            }
            else
            {
                /* If AI is not close enough, switch to the Move state */
                Debug.Log("If AI is still faraway, Decide to move");
                currentState = FSMState.Move;
            }
        }
        else if (currentState == FSMState.Move)
        {
            //MoveController.destination = UtilityBrain.bestAction.RequiredDestination;
            /* Calculate the distance between AI and the required destination of the best action */
            float distance = Vector3.Distance(MoveController.RequiredDestination.position, this.transform.position);

            /* Log destination and distance information */

            if (distance < 1f)
            {
                /* If AI is close enough, switch to the Perform state */
                Debug.Log("If AI is close enough, Move to the Perform state");
                currentState = FSMState.Perform;
            }
            else
            {
                /* If AI is not close enough, continue moving */
                Debug.Log("If AI is still faraway, keep moving");
                MoveController.MoveTo(MoveController.RequiredDestination.position);
            }
        }
        else if (currentState == FSMState.Perform)
        {
            if (!UtilityBrain.FinishedExecutingBestAction)
            {
                /* Execute the best action if it's not finished */
                Debug.Log("Execute the best action if it's not finished");
                UtilityBrain.bestAction.Execute(this);
            }
            else
            {
                /* If best action execution is finished, switch back to the Decide state */
                Debug.Log("If best action execution is finished, switch back to the Decide state");
                currentState = FSMState.Decide;
            }
        }
    }


}
