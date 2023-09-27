using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UBrain : MonoBehaviour
{
    /* Best action to execute */
    public UAction bestAction { get; set; }
    public UAction bestActionName;
    public float bestActionScore;
    [SerializeField] private UAction[] actionsAvailable;
    public bool FinishedDeciding { get; set; }
    public bool FinishedExecutingBestAction { get; set; }
    private PrototypeController colonist;
    void Awake()
    {
        colonist = GetComponent<PrototypeController>();
        FinishedDeciding = false;
        FinishedExecutingBestAction = false;
    }
    /* Loop through all the available actions */
    /* Give the highest scoring action */
    public void DecideBestAction()
    {
        FinishedExecutingBestAction = false;
        /* Initialize variables to keep track of the best action and its score */
        float score = 0f;
        int nextBestActionIndex = 0;

        /* Loop through all the available actions to find the one with the highest score */
        for (int i = 0; i < actionsAvailable.Length; i++)
        {
            /* Get the score of the current action using the ScoreAction method*/
            float actionScore = ScoreAction(actionsAvailable[i]);

            /* Check if the current action has a higher score than the previous best action*/
            if (actionScore > score)
            {
                // Update the index of the next best action and the score*/
                nextBestActionIndex = i;
                score = actionScore;
            }
        }

        /* Store the best action in the 'bestAction' property*/
        bestAction = actionsAvailable[nextBestActionIndex];
        /* When we found the best action, set a target destination for it*/
        bestAction.SetRequiredDestination(colonist);
        //finishedDeciding = true;
        bestActionName = bestAction;
        bestActionScore = bestAction.Score;
    }



    #region Old Score Action
    //public float ScoreAction(UAction action)
    //{
    //    float score = 1f;
    //    for (int i = 0; i < action.considerations.Length; i++)
    //    {
    //        /* Individual considerations score */
    //        float considerationScore = action.considerations[i].ScoreConsideration(colonist);
    //        /* Accumulating each considerations score to the overall score */
    //        score *= considerationScore;
    //        /* if in an action there is a consideration that have zero score, mean the action is not ugrent*/
    //        /* Stop for loop and just spit out zero as score, no point of computing further*/
    //        if (score == 0)
    //        {
    //            action.Score = 0;
    //            return action.Score;
    //        }
    //    }

    //    /* Averaging scheme of overall score by Dave Mark */
    //    /* When you multiply values between 0 and 1 they tend to get smaller*/
    //    /* http://www.matt-versaggi.com/mit_open_courseware/GameAI/BehavioralMathematicalforGameAI.pdf */
    //    /* https://www.gdcvault.com/play/1021848/Building-a-Better-Centaur-AI 10.14 */
    //    float originalScore = score;
    //    float modFactor = 1 - (1 / action.considerations.Length);
    //    float makeupValue = (1 - originalScore) * modFactor;
    //    action.Score = originalScore + (makeupValue * originalScore);

    //    return action.Score;
    //}#region MyRegion

    #endregion
    /* Loop through all the considerations of the action */
    /* Score all the considerations */
    /* Average the consideration scores ==> overall action score */
    public float ScoreAction(UAction action)
    {
        float score = 1f;
        /* Averaging scheme of overall score by Dave Mark */
        /* This factor is used to compensate for the multiplication effect when averaging the overall score.  */
        /* It helps counteract the decreasing influence of multiple considerations with scores between 0 and 1. */
        float modificationFactor = 1 - (1f / action.considerations.Length);
        for (int i = 0; i < action.considerations.Length; i++)
        {
            /* Individual considerations score */
            float considerationScore = action.considerations[i].ScoreConsideration(colonist);

            /* The term "make up value" refers to the amount by which each individual consideration's score 
             * is adjusted during the averaging process*/
            /* The make-up value ensures that the final averaged score 
             * is closer to the original individual consideration score.*/
            float makeUpValue = (1 - considerationScore) * modificationFactor;
            float finalScore = considerationScore + (makeUpValue * considerationScore);

            /* Accumulating each consideration's score to the overall score */
            score *= finalScore;

            /* if in an action there is a consideration that has zero score, the action is not urgent */
            /* Stop the loop and just return zero as the score, no point in computing further */
            if (score == 0)
            {
                action.Score = 0;
                return action.Score;
            }
        }
        return action.Score = score;
    }

}
