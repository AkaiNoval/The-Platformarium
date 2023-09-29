using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(fileName = "ActionSleepAtHouse", menuName = "UtilityAI/Actions/ActionSleepAtHouse")]
public class ActionSleepAtHouse : UAction
{
    public override void Execute<T>(T colonist)
    {
        Debug.Log("Sleeping in this house");
        colonist.Stats.Energy += 100f * Time.deltaTime;
        colonist.PrototypeAnimController.PlayAnimation(animKey);
        if (colonist.Stats.Energy >= 90*colonist.Stats.SOPrototypeStats.MaxEnergy/100)
        {
            colonist.UtilityBrain.FinishedExecutingBestAction = true;
        }
    }

    public override void SetRequiredDestination<T>(T colonist)
    {
        // Create a list to store valid residential houses
        List<ResidentialHouse> validHouses = new List<ResidentialHouse>();

        // Reference to the NavMeshAgent attached to the colonist
        NavMeshAgent agent = colonist.MoveController.agent;

        // Iterate through all residential houses in the world
        foreach (var house in WorldContext.Instance.AllHousesOnTheMap)
        {
            // Calculate the path from the colonist to the current house
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, house.transform.position, NavMesh.AllAreas, path);

            // Check if the path is valid and complete
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                // Add the house to the list of valid houses
                validHouses.Add(house);
            }
            else
            {
                // Handle the case where no valid path is found to the nearest house
                Debug.Log("No valid path to the nearest house: " + house.name);
            }
        }

        // Check if there are any valid houses in the list
        if (validHouses.Count == 0)
        {
            // Handle the case where no valid paths were found to any houses
            Debug.Log("No valid paths to any houses.");
            return;
        }

        // Find the nearest house from the list of valid houses
        Transform nearestHouse = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var house in validHouses)
        {
            // Calculate the path from the colonist to the current house
            NavMeshPath housePath = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, house.transform.position, NavMesh.AllAreas, housePath);

            // Calculate the actual distance based on the path length
            float distanceFromResource = GetPathLength(housePath);

            // Check if the current house is closer than the previously found nearest house
            if (distanceFromResource < shortestDistance)
            {
                nearestHouse = house.transform;
                shortestDistance = distanceFromResource;
            }
        }

        // Set the required destination for the action to the nearest house
        colonist.MoveController.RequiredDestination = nearestHouse;

        // Calculate the path to the nearest house
        NavMeshPath nearestHousePath = new NavMeshPath();
        NavMesh.CalculatePath(agent.transform.position, nearestHouse.position, NavMesh.AllAreas, nearestHousePath);

        // Set the NavMesh agent's path to follow the path to the nearest house
        agent.SetPath(nearestHousePath);
    }
}
