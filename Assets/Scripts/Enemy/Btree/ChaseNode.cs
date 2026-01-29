using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private NavMeshAgent agent;
    private Transform playerTarget;
    private float stoprange;

    public ChaseNode(NavMeshAgent agent, Transform playerTarget, float stoprange)
    {
        this.agent = agent;
        this.playerTarget = playerTarget;
        this.stoprange = stoprange;
    }

    public override NodeState Evaluate()
    {
        if (playerTarget == null || agent == null)
        {
            state = NodeState.FAILURE;
        }
        float distance = Vector3.Distance(agent.transform.position, playerTarget.position);
        if (distance > stoprange)
        {
            agent.isStopped = false;
            agent.SetDestination(playerTarget.position);
            state = NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            state = NodeState.SUCCESS;
        }
        return state;
    }
}
