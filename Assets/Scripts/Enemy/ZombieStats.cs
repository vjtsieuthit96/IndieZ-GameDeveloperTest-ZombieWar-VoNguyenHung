using UnityEngine;

public class ZombieStats : MonoBehaviour
{
    [SerializeField] private EnemyDataSO enemyData;

    [SerializeField] private float currentHP;
    private bool isDead = false;

    private void Start()
    {
        currentHP = enemyData.maxHP;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= amount;
        Debug.Log($"Zombie took {amount} damage. Current HP: {currentHP}");
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public float GetDamage()
    {
        return enemyData.damage;
    }

    public float GetMoveSpeed()
    {
        return enemyData.moveSpeed;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;           
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}