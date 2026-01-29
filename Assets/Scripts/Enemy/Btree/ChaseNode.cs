using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private NavMeshAgent agent;
    private Transform player;
    private float range;
    private Animator animator;
    private ZombieType type;
    private bool hasSetStanding = false;

    public ChaseNode(NavMeshAgent agent, Transform player, float range, Animator animator, ZombieType type)
    {
        this.agent = agent;
        this.player = player;
        this.range = range;
        this.animator = animator;
        this.type = type;
    }

    public override NodeState Evaluate()
    {
        if (player == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(agent.transform.position, player.position);
       
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (distance <= range)
        {           
            agent.isStopped = true;

            if (type == ZombieType.Crawl && !hasSetStanding)
            {
                hasSetStanding = true;
                animator.SetTrigger(AnimationHashes.Z_isStanding); 
            }

            state = NodeState.SUCCESS; 
        }
        else
        {
            state = NodeState.RUNNING; 
        }

        return state;
    }

    public void ResetStanding()
    {
        hasSetStanding = false;       
    }
}