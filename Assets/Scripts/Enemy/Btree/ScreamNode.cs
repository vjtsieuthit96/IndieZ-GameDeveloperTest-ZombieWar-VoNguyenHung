using UnityEngine;
public class ScreamNode : Node
{
    private Animator animator;
    private bool hasScreamed = false;    
    private float chance;  

    public ScreamNode(Animator animator, float chance = 0.3f)
    {
        this.animator = animator;    
        this.chance = chance;       
    }

    public override NodeState Evaluate()
    {     
        if (!hasScreamed)
        {
            if (Random.value <= chance)
            {
                animator.SetTrigger(AnimationHashes.Z_Scream);
                Debug.Log("Scream!");
            }
            hasScreamed = true;
        }      
        
        state = NodeState.SUCCESS;
        return state;
    }
    
    public void ResetScream()
    {
        hasScreamed = false;        
    }
}