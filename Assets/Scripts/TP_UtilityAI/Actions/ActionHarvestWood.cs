using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HarvestWood", menuName = "UtilityAI/Actions/ActionHarvestWood")]
public class ActionHarvestWood : UAction
{
    public override void Execute<T>(T colonist)
    {
        //Logic for cutting wood, probaly
        Debug.Log("Cutting wood");
        //WorldContext.Instance.Wood++;
        colonist.UtilityBrain.FinishedExecutingBestAction = true;
    }

    public override void SetRequiredDestination<T>(T colonist)
    {
        /* Store the nearest tree's Transform */
        Transform nearestTree = null;
        /* Store the shortest distance to a tree */
        float shortestDistance = Mathf.Infinity;

        /* Iterate through all trees in the world*/


        foreach (var tree in WorldContext.Instance.AllTreesOnTheMap)
        {
            /* Calculate the distance between the colonist and the current tree */
            float distanceFromResource = Vector3.Distance(tree.transform.position, colonist.transform.position);

            /* Check if the current tree is closer than the previously found nearest tree */
            if (distanceFromResource < shortestDistance)
            {
                nearestTree = tree.transform;
                shortestDistance = distanceFromResource;
            }
        }


        Debug.Log(nearestTree.transform.position);
        /* Set the required destination for the action to the nearest tree */
        RequiredDestination = nearestTree;

        /* Set the NavMesh agent's destination to the nearest tree */

        /////////======>colonist.MoveController.destination = nearestTree;
    }

}
