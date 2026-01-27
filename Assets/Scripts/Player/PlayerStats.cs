using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerDataSO playerData;

    [SerializeField] private float currentHP;
    [SerializeField] private float currentArmor;

    private bool isDead = false;
    private bool armorBroken = false;

    private void Start()
    {
        currentHP = playerData.maxHP;
        currentArmor = playerData.maxArmor;

        if (playerData.defaultArmor != null && playerData.defaultArmor.itemType == ItemType.Armor)
        {
            currentArmor = playerData.defaultArmor.armorValue;
            armorBroken = currentArmor <= 0;
            if (!armorBroken)
            {
                GameEventManager.Instance.InvokeArmorEquipped();
            }
        }

        NotifyStatsChanged();
    }

    private void Update()
    {        
        if (!isDead && currentHP <= 0)
        {
            Die();
        }
       
        if (!armorBroken && currentArmor <= 0)
        {
            armorBroken = true;
            GameEventManager.Instance.InvokeArmorBroken();
        }
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

                if (!armorBroken)
                {
                    armorBroken = true;
                    GameEventManager.Instance.InvokeArmorBroken();
                }
            }
        }
        else
        {
            currentHP -= amount;
        }

        NotifyStatsChanged();

        if (!isDead && currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, playerData.maxHP);
        NotifyStatsChanged();
    }

    public void AddArmor(ItemDataSO armor)
    {
        if (armor.itemType != ItemType.Armor) return;

        currentArmor += armor.armorValue;
        armorBroken = false;

        GameEventManager.Instance.InvokeArmorEquipped();
        NotifyStatsChanged();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player died!");
        GameEventManager.Instance.InvokePlayerDied();
    }

    private void NotifyStatsChanged()
    {
        GameEventManager.Instance.InvokePlayerStatsChanged(currentHP, currentArmor);
    }
}