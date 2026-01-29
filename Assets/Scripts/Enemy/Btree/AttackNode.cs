using UnityEngine;

public class AttackNode : Node
{
    private Animator animator;
    private Transform player;
    private float attackRange;

    public AttackNode(Animator animator, Transform player, float attackRange)
    {
        this.animator = animator;
        this.player = player;
        this.attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        if (player == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance <= attackRange)
        {
            RotateTowardsTarget(animator.transform, player);
            int attackIndex = Random.Range(1, 4);
            switch (attackIndex)
            {
                case 1: animator.SetTrigger(AnimationHashes.Z_Attack1); break;
                case 2: animator.SetTrigger(AnimationHashes.Z_Attack2); break;
                case 3: animator.SetTrigger(AnimationHashes.Z_Attack3); break;
            }
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }

    private void RotateTowardsTarget(Transform self, Transform target, float rotateSpeed = 10f)
    {
        if (target == null) return;

        Vector3 direction = target.position - self.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            self.rotation = Quaternion.Slerp(self.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
    }
}