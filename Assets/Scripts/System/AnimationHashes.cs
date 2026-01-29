using UnityEngine;

public static class AnimationHashes
{
    #region Enemy    
    public static readonly int Z_Scream = Animator.StringToHash("Scream");
    public static readonly int Z_Die = Animator.StringToHash("isDead");
    public static readonly int Z_Speed = Animator.StringToHash("Speed");
    public static readonly int Z_isStanding = Animator.StringToHash("isStanding");
    // Attack variations
    public static readonly int Z_Attack1 = Animator.StringToHash("Attack1");
    public static readonly int Z_Attack2 = Animator.StringToHash("Attack2");
    public static readonly int Z_Attack3 = Animator.StringToHash("Attack3");
    #endregion
}