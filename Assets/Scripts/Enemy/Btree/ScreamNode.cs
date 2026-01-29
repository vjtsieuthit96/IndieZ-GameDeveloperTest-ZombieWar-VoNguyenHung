using UnityEngine;
public class ScreamNode : Node
{
    private Animator animator;
    private bool hasScreamed = false;
    private bool hasSetStanding = false;
    private float chance;
    private ZombieType type;

    public ScreamNode(Animator animator, ZombieType zombie, float chance = 0.3f)
    {
        this.animator = animator;
        this.type = zombie;
        this.chance = chance;       
    }

    public override NodeState Evaluate()
    {
        if (type == ZombieType.Crawl && !hasSetStanding)
        {
            animator.SetBool(AnimationHashes.Z_isStanding, true);
            hasSetStanding = true;
        }

        if (!hasScreamed)
        {
            if (Random.value <= chance)
            {
                animator.SetTrigger(AnimationHashes.Z_Scream);
            }
            hasScreamed = true;
        }      
        
        state = NodeState.SUCCESS;
        return state;
    }
    
    public void ResetScream()
    {
        hasScreamed = false;
        hasSetStanding = false;        
        if (type == ZombieType.Crawl)
        {
            animator.SetBool(AnimationHashes.Z_isStanding, false);
        }
    }
}