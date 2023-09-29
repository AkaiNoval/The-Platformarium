using System.Collections;
using System.Collections.Generic;
using System.IO;
using TP.Resource;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "HarvestWood", menuName = "UtilityAI/Actions/ActionHarvestWood")]
public class ActionHarvestWood : UAction
{

    public override void Execute<T>(T colonist)
    {
        //Logic for cutting wood, probaly
        Debug.Log("Cutting wood");
        colonist.PrototypeAnimController.PlayAnimation(animKey);
        colonist.UtilityBrain.NearestActionTarget.TryGetComponent(out IHarvestable tree);
        tree.Harvest();
        colonist.UtilityBrain.FinishedExecutingBestAction = true;
    }

    public override void SetRequiredDestination<T>(T colonist)
    {
        // Create a new list to store RTree objects
        List<RTree> validTrees = new List<RTree>();

        // Reference to the NavMeshAgent
        NavMeshAgent agent = colonist.MoveController.agent;

        // Iterate through all trees in the world
        foreach (var tree in WorldContext.Instance.AllTreesOnTheMap)
        {
            // Calculate the path to the tree
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, tree.transform.position, NavMesh.AllAreas, path);

            // Check if the path is valid and complete
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                // Calculate the actual distance based on the path length
                float distanceFromResource = GetPathLength(path);

                // If a valid path exists, add the tree to the validTrees list
                validTrees.Add(tree);
            }
            else
            {
                // Handle the case where no valid path is found (e.g., display a message or perform another action)
                //Debug.Log("No valid path to the nearest tree: " + tree.name);
            }
        }

        // Check if there are any valid trees in the list
        if (validTrees.Count == 0)
        {
            // Handle the case where no valid paths were found
            Debug.Log("No valid paths to any trees.");
            return;
        }

        // Find the nearest tree from the validTrees list
        Transform nearestTree = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var tree in validTrees)
        {
            // Calculate the path to the tree
            NavMeshPath treePath = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, tree.transform.position, NavMesh.AllAreas, treePath);

            // Calculate the actual distance based on the path length
            float distanceFromResource = GetPathLength(treePath);

            if (distanceFromResource < shortestDistance)
            {
                nearestTree = tree.transform;
                shortestDistance = distanceFromResource;
            }
        }

        // Set the required destination for the action to the nearest tree
        colonist.MoveController.RequiredDestination = nearestTree;
        colonist.UtilityBrain.NearestActionTarget = nearestTree.gameObject;

        // Calculate the path to the nearest tree
        NavMeshPath nearestTreePath = new NavMeshPath();
        NavMesh.CalculatePath(agent.transform.position, nearestTree.position, NavMesh.AllAreas, nearestTreePath);

        // Set the NavMesh agent's path to follow the path to the nearest tree
        agent.SetPath(nearestTreePath);
    }
}
