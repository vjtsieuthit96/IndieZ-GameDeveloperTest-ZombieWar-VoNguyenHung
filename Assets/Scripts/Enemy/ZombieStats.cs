using UnityEngine;
using System.Collections;

public class ZombieStats : MonoBehaviour
{
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private float currentHP;
    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshes;
    private bool isDead = false;

    private void OnEnable()
    {        
        currentHP = enemyData.maxHP;
        isDead = false;
        
        foreach (var smr in skinnedMeshes)
        {
            smr.material.SetFloat("_DissolveStrength", 0);           
        }
    }

    private void Update()
    {
        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
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

    public float GetDamage() => enemyData.damage;
    public float GetMoveSpeed() => enemyData.moveSpeed;

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        StartCoroutine(DissolveAndReturn());
    }

    private IEnumerator DissolveAndReturn()
    {
        float amount = 0;
        while (amount < 1)
        {
            amount += Time.deltaTime * 0.4f;
            foreach (var smr in skinnedMeshes)
            {
                Material mat = smr.material;
                smr.material.SetFloat("_DissolveStrength", amount);
            }
            yield return null;
        }

        //ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}