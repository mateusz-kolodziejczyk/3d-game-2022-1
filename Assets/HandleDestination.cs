using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HandleDestination : MonoBehaviour
{
    private Transform destination;
    public Transform Destination
    {
        get => destination;
        set
        {
            destination = value;
            if (agent != null)
            {
                agent.SetDestination(destination.position);
            }
        }
    }

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void ClearDestination()
    {
        agent.ResetPath();
    }

}
