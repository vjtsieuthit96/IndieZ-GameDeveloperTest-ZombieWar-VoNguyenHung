using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;

    [SerializeField] private float currentHP;
    [SerializeField] private float currentArmor;
    public float CurrentHP => currentHP;
    public float CurrentArmor => currentArmor;
    private bool restoredFromSave = false;

    public void SetStats(float hp, float armor)
    {
        currentHP = hp;
        currentArmor = armor;
        restoredFromSave = true;        
        if (currentArmor <= 0)
            GameEventManager.Instance.InvokeArmorBroken();
        else
            GameEventManager.Instance.InvokeArmorEquipped();

        NotifyStatsChanged();

    }

    private bool isDead = false;
    private bool armorBroken = false;

    private void Start()
    {
        if (!restoredFromSave)
        {
            currentHP = playerData.maxHP;
            currentArmor = playerData.maxArmor;

            if (playerData.defaultArmor != null && playerData.defaultArmor.itemType == ItemType.Armor)
            {
                currentArmor = playerData.defaultArmor.armorValue;
                if (currentArmor > 0)
                    GameEventManager.Instance.InvokeArmorEquipped();
            }
            NotifyStatsChanged();
        }

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

        GameEventManager.Instance.InvokePlayerTakeDamage(amount);
        NotifyStatsChanged();

        if (!isDead && currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(ItemDataSO healitem)
    {
        currentHP = Mathf.Min(currentHP + healitem.healAmount, playerData.maxHP);
        NotifyStatsChanged();
    }

    public void AddArmor(ItemDataSO armor)
    {
        if (armor.itemType != ItemType.Armor) return;        
        currentArmor += armor.armorValue;
        if (currentArmor >= 50)
            currentArmor = 50;
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