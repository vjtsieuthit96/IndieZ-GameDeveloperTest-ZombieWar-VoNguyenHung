using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ZombieType
{
    Stand,
    Crawl
}

public class ZombieAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float playerDetectRange;
    [SerializeField] private float attackRange;
    [SerializeField] private ZombieType zombieType;
    [SerializeField] private EnemyDataSO zombieData;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Collider hitCollider;


    private Node root;
    private bool isDead = false;
    private float savedSpeed;    
    //evaluation freq
    private float evalInterval = 0.15f;
    private float evalTimer = 0f;

    public void SetTarget(Transform target)
    {
        playerTarget = target;
        agent.speed = (zombieType == ZombieType.Crawl) ? zombieData.moveSpeed / 3f : zombieData.moveSpeed;
        playerDetectRange = zombieData.playerDetectRange;
        attackRange = zombieData.attackRange;
    }

    private void Start()
    {      
        root = new Sequence(new List<Node>
        {
            new ChaseNode(agent, playerTarget, playerDetectRange,animator,zombieType),
            new ScreamNode(animator),
            new ChaseNode(agent, playerTarget, attackRange,animator,zombieType),
            new AttackNode(animator, playerTarget, attackRange)
        });
    }

    private void Update()
    {
        if (isDead) return;
        animator.SetFloat(AnimationHashes.Z_Speed, agent.velocity.magnitude);
        evalTimer += Time.deltaTime;
        if (evalTimer >= evalInterval)
        {
            root.Evaluate();
            evalTimer = 0f;
        }
    }    

    private void OnEnable()
    {       
        agent.enabled = true;        
        animator.SetBool(AnimationHashes.Z_Die, false);
        isDead = false;
        hitCollider.enabled = true;
        agent.speed = zombieData.moveSpeed;
        GameEventManager.Instance.OnEnemyDie += HandleEnemyDie;
        GameEventManager.Instance.OnEnemyHit += HandleEnemyHit;       
    }
    private void OnDisable()
    {
        GameEventManager.Instance.OnEnemyDie -= HandleEnemyDie;
        GameEventManager.Instance.OnEnemyHit -= HandleEnemyHit;

    }
    private void HandleEnemyDie(GameObject enemy)
    {        
        if (enemy == gameObject)
        {
            isDead = true;
            if (root != null)
            {
                var screamNode = root.FindNode<ScreamNode>();
                var chaseNode = root.FindNode<ChaseNode>();
                if (screamNode != null)
                    screamNode.ResetScream();
                if (chaseNode != null)
                    chaseNode.ResetStanding();
            }
            hitCollider.enabled = false;
            agent.enabled = false;
            animator.SetBool(AnimationHashes.Z_Die, true);

        }
    }

    private void HandleEnemyHit(GameObject enemy)
    {
        if (enemy == gameObject)
        {
            agent.speed *= 2f;
        }
    }
    public void PauseNavMesh()
    {
        if (agent != null)
        {
            savedSpeed = agent.speed;  
            agent.speed = 0f;           
        }
    }

    public void ResumeNavMesh()
    {
        if (agent != null)
        {
            agent.speed = savedSpeed;   
        }
    }
    public void OnNormalSpeed()
    {
        agent.speed = zombieData.moveSpeed;
    }

}