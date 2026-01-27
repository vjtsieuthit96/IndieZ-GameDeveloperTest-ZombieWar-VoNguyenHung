using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerDataSO playerData;

    [SerializeField]private float currentHP;
    [SerializeField]private float currentArmor;

    private void Start()
    {
        currentHP = playerData.maxHP;
        currentArmor = playerData.maxArmor;

        if (playerData.defaultArmor != null)
        {
            currentArmor = playerData.defaultArmor.armorValue;
        }

        NotifyStatsChanged();
    }

    private void Update()
    {
        if (currentHP <= 0) Die();
    }

    public void TakeDamage(float amount)
    {
        if (currentArmor > 0)
        {
            currentArmor -= amount;
            if (currentArmor < 0)
            {
                currentHP += currentArmor;
                currentArmor = 0;
            }
        }
        else
        {
            currentHP -= amount;
        }

        NotifyStatsChanged();        
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, playerData.maxHP);
        NotifyStatsChanged();
    }

    public void AddArmor(ArmorDataSO armor)
    {
        currentArmor += armor.armorValue;
        NotifyStatsChanged();
    }

    private void Die()
    {
        Debug.Log("Player died!");
        GameEventManager.Instance.InvokePlayerDied();
    }

    private void NotifyStatsChanged()
    {
        GameEventManager.Instance.InvokePlayerStatsChanged(currentHP, currentArmor);
    }
}