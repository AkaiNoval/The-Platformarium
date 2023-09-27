using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tester : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform destination;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        InvokeRepeating("SetDestination", 0f,0.5f);
    }
    /* Set destionation for the agent to move*/
    public void MoveTo(Vector3 position)
    {
        agent.destination = position;
    }

    void SetDestination()
    {
        MoveTo(WorldContext.Instance.AllTreesOnTheMap[1].transform.position);
    }
}
