using UnityEngine;

public class CheckPlayerInRangeNode : Node
{
    private Transform zombie;
    private Transform playerTarget;
    private float range;

    public CheckPlayerInRangeNode(Transform zombie, Transform playerTarget, float range)
    {
        this.zombie = zombie;
        this.playerTarget = playerTarget;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        if (playerTarget == null)
        {
            return state = NodeState.FAILURE;
        }
        float distance = Vector3.Distance(zombie.position, playerTarget.position);
        state = (distance <= range) ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}
