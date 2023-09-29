using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UMovementController : MonoBehaviour
{
    public NavMeshAgent agent { get; set; }
    public Transform destination { get; set; }
    public Transform RequiredDestination { get; set; }
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
