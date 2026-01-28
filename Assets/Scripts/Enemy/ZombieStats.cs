using UnityEngine;
using System.Collections;

public class ZombieStats : MonoBehaviour
{
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private float currentHP;
    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshes;
    private bool isDead = false;

    private Coroutine hitEffectCoroutine; 

    private void OnEnable()
    {
        currentHP = enemyData.maxHP;
        isDead = false;

        foreach (var smr in skinnedMeshes)
        {
            smr.material.SetFloat("_DissolveStrength", 0);
            smr.material.SetFloat("_HitEffect", 0); 
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

        
        if (hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
        }
        
        hitEffectCoroutine = StartCoroutine(HitEffectRoutine());

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
        GameEventManager.Instance.InvokeEnemyDie(this.gameObject);
        isDead = true;
        StartCoroutine(DissolveAndReturn());
    }

    private IEnumerator HitEffectRoutine()
    {
        float timer = 0f;
        float duration = 5f;

        while (timer < duration)
        {
            float value = Mathf.PingPong(Time.time * 4f, 0.75f);
            foreach (var smr in skinnedMeshes)
            {
                smr.material.SetFloat("_HitEffect", value);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        foreach (var smr in skinnedMeshes)
        {
            smr.material.SetFloat("_HitEffect", 0);
        }

        hitEffectCoroutine = null;
    }
    private IEnumerator DissolveAndReturn()
    {
        float amount = 0;
        while (amount < 1)
        {
            amount += Time.deltaTime * 0.25f;
            foreach (var smr in skinnedMeshes)
            {
                smr.material.SetFloat("_DissolveStrength", amount);                
            }
            yield return null;
        }

        //ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
    
}