using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementComponent : MonoBehaviour
{
    public WayPoint current;
    private LinkedListNode<WayPoint> currentNode;
    private bool isMovingForward = true;
    private WayPointManager route;
    
    private const float reachDistance = 1.5f;
    
    private NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (current != null)
            SetCurrent(current);
    }
    
    public void SetCurrent(WayPoint wp)
    {
        current = wp;
        route = wp.ownerRoute;
        currentNode = route.NodeOf(wp);

        agent.SetDestination(wp.transform.position);
    }

    void Update()
    {
        if (current == null || agent.pathPending)
            return;

        float dist = Vector3.Distance(transform.position, current.transform.position);
        Debug.Log(dist);

        if (dist < reachDistance)
        {
            ChooseNext();
        }
    }

    private void ChooseNext()
    {
        if (current.branches.Count > 0)
        {
            if (Random.value < 0.9f) 
            {
                WayPoint branchTarget = current.branches[Random.Range(0, current.branches.Count)];
                SetCurrent(branchTarget);
                return;
            }
        }

        if (isMovingForward)
        {
            if (currentNode.Next != null)
            {
                SetCurrent(currentNode.Next.Value);
            }
            else
            {
                isMovingForward = false;
                if (currentNode.Previous != null)
                    SetCurrent(currentNode.Previous.Value);
            }
        }
        else
        {
            if (currentNode.Previous != null)
            {
                SetCurrent(currentNode.Previous.Value);
            }
            else
            {
                isMovingForward = true;
                if (currentNode.Next != null)
                    SetCurrent(currentNode.Next.Value);
            }
        }
    }
}
