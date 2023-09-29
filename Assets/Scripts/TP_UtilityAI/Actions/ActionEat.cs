using System.Collections;
using System.Collections.Generic;
using TP.Resource;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ActionEat", menuName = "UtilityAI/Actions/ActionEat")]
public class ActionEat : UAction
{
    public override void Execute<T>(T colonist)
    {
        colonist.PrototypeAnimController.PlayAnimation(animKey);
        colonist.UtilityBrain.NearestActionTarget.TryGetComponent(out IHarvestable bush);
        bush.Harvest();
        colonist.Stats.Hunger += 50f;
        colonist.UtilityBrain.FinishedExecutingBestAction = true;
    }

    public override void SetRequiredDestination<T>(T colonist)
    {
        // Create a new list to store RTree objects
        List<RBerryBush> validBushes = new List<RBerryBush>();

        // Reference to the NavMeshAgent
        NavMeshAgent agent = colonist.MoveController.agent;

        // Iterate through all trees in the world
        foreach (var bush in WorldContext.Instance.AllBushesOnTheMap)
        {
            // Calculate the path to the tree
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, bush.transform.position, NavMesh.AllAreas, path);

            // Check if the path is valid and complete
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                // Calculate the actual distance based on the path length
                float distanceFromResource = GetPathLength(path);

                // If a valid path exists, add the tree to the validTrees list
                validBushes.Add(bush);
            }
            else
            {
                // Handle the case where no valid path is found (e.g., display a message or perform another action)
                //Debug.Log("No valid path to the nearest tree: " + tree.name);
            }
        }

        // Check if there are any valid trees in the list
        if (validBushes.Count == 0)
        {
            // Handle the case where no valid paths were found
            Debug.Log("No valid paths to any trees.");
            return;
        }

        // Find the nearest tree from the validTrees list
        Transform nearestBush = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var bush in validBushes)
        {
            // Calculate the path to the tree
            NavMeshPath treePath = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, bush.transform.position, NavMesh.AllAreas, treePath);

            // Calculate the actual distance based on the path length
            float distanceFromResource = GetPathLength(treePath);

            if (distanceFromResource < shortestDistance)
            {
                nearestBush = bush.transform;
                shortestDistance = distanceFromResource;
            }
        }

        // Set the required destination for the action to the nearest tree
        colonist.MoveController.RequiredDestination = nearestBush;
        colonist.UtilityBrain.NearestActionTarget = nearestBush.gameObject;

        // Calculate the path to the nearest tree
        NavMeshPath nearestBushPath = new NavMeshPath();
        NavMesh.CalculatePath(agent.transform.position, nearestBush.position, NavMesh.AllAreas, nearestBushPath);

        // Set the NavMesh agent's path to follow the path to the nearest tree
        agent.SetPath(nearestBushPath);
    }
}
