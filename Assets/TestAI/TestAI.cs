using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    public EndRoom end;
    public NavMeshAgent agent;

    private void Awake()
    {
        end = FindObjectOfType<EndRoom>();

        agent = GetComponent<NavMeshAgent>();
        

       
    }

    private void Update()
    {
        agent.SetDestination(end.transform.position);
    }
}
