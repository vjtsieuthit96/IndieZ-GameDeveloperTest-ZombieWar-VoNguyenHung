using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    [SerializeField] private Collider[] handColliders;
    [SerializeField] private Collider[] legColliders;
    [SerializeField] private Collider headCollider;

    [SerializeField] private ZombieStats zomStats;

    private void Start()
    {        
        RegisterColliders(handColliders);
        RegisterColliders(legColliders);
        RegisterCollider(headCollider);  
    }

    private void OnEnable()
    {
        DisableHandColliders();
        DisableLegColliders();
        DisableHeadCollider();
    }
    private void RegisterColliders(Collider[] colliders)
    {
        foreach (var col in colliders)
        {
            var triggerHelper = col.gameObject.AddComponent<AttackTrigger>();
            triggerHelper.Init(this);
        }
    }

    private void RegisterCollider(Collider col)
    {
        if (col != null)
        {
            var triggerHelper = col.gameObject.AddComponent<AttackTrigger>();
            triggerHelper.Init(this);
        }
    }   
    public void OnHitPlayer(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerStats>()?.TakeDamage(zomStats.GetDamage());
        }
    }
    public void EnableHandColliders()
    {
        foreach (var col in handColliders) col.enabled = true;
    }
    public void DisableHandColliders()
    {
        foreach (var col in handColliders) col.enabled = false;
    }
    public void EnableLegColliders()
    {
        foreach (var col in legColliders) col.enabled = true;
    }
    public void DisableLegColliders()
    {
        foreach (var col in legColliders) col.enabled = false;
    }
    public void EnableHeadCollider()
    {
        if (headCollider != null) headCollider.enabled = true;
    }
    public void DisableHeadCollider()
    {
        if (headCollider != null) headCollider.enabled = false;
    }

}