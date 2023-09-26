using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UMovementController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform destination;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /* Set destionation for the agent to move*/
    public void MoveTo(Vector3 position)
    {
        agent.destination = position;
    }
}
