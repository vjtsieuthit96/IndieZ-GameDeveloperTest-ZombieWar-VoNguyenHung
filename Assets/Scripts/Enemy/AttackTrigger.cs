using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private AttackColliderManager manager;

    public void Init(AttackColliderManager mgr)
    {
        manager = mgr;
    }

    private void OnTriggerEnter(Collider other)
    {
        manager.OnHitPlayer(other);
    }
}