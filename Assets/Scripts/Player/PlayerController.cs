using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private GameObject armor;    
    [SerializeField] private ItemDataSO defaultWeapon;

    private GameObject currentWeapon;

    void OnEnable()
    {
        GameEventManager.Instance.OnArmorBroken += DisableArmor;
        GameEventManager.Instance.OnArmorEquipped += EnableArmor;
    }
    
    void Start()
    {
        if (defaultWeapon != null && currentWeapon == null)
        {
            currentWeapon = ObjectPoolManager.SpawnObject(defaultWeapon.weaponPrefab, weaponSocket, Quaternion.identity);
        }
    }
    void OnDisable()
    {
        GameEventManager.Instance.OnArmorBroken -= DisableArmor;
        GameEventManager.Instance.OnArmorEquipped -= EnableArmor;
    }

    public void DisableArmor()
    {
        if (armor != null)
            armor.SetActive(false);
    }

    public void EnableArmor()
    {
        if (armor != null)
            armor.SetActive(true);
    }

    public void EquipWeapon(ItemDataSO weaponData)
    {
        if (currentWeapon != null)
        {
            ObjectPoolManager.ReturnObjectToPool(currentWeapon);
        }

        if (weaponData.weaponPrefab != null)
        {
            currentWeapon = ObjectPoolManager.SpawnObject(weaponData.weaponPrefab, weaponSocket, Quaternion.identity);
        }
    }
}
