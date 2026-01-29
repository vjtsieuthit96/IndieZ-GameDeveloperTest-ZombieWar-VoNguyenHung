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
    [SerializeField] private float playerDetectRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private ZombieType zombieType;
    [SerializeField] private EnemyDataSO zombieData;

    private Transform playerTarget;
    private Node root;
    private bool isDead = false;

    //evaluation freq
    private float evalInterval = 0.5f;
    private float evalTimer = 0f;

    public void SetTarget(Transform target)
    {
        playerTarget = target;
        agent.speed = zombieData.moveSpeed;
    }

    private void Start()
    {
        root = new Sequence(new List<Node>
        {
            new ChaseNode(agent, playerTarget, playerDetectRange),
            new ScreamNode(animator, zombieType),
            new ChaseNode(agent, playerTarget, attackRange),
            new AttackNode(animator, playerTarget, attackRange)
        });
    }

    private void Update()
    {
        if (isDead) return;
        evalTimer += Time.deltaTime;
        if (evalTimer >= evalInterval)
        {
            root.Evaluate();
            evalTimer = 0f;
        }
    }    

    private void OnEnable()
    {        
        var screamNode = root.FindNode<ScreamNode>();
        screamNode.ResetScream();
        agent.enabled = true;        
        animator.SetBool(AnimationHashes.Z_Die, false);
        isDead = false;
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
}