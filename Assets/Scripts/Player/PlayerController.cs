using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private GameObject armor;
    [SerializeField] private ItemDataSO defaultWeapon;
    [SerializeField] private Animator animator;
    [SerializeField] private Inventory Inventory;

    private int shootHash = Animator.StringToHash("Shoot");
    private int reloadHash = Animator.StringToHash("Reload");
    private int isReloadingHash = Animator.StringToHash("isReloading");
    private GameObject currentWeapon;
    private bool isReloading = false;
    void OnEnable()
    {
        GameEventManager.Instance.OnArmorBroken += DisableArmor;
        GameEventManager.Instance.OnArmorEquipped += EnableArmor;
        GameEventManager.Instance.OnShootHold += HandleShootHold;
        GameEventManager.Instance.OnShootRelease += HandleShootRelease;
        GameEventManager.Instance.OnReloadStarted += HandleReloadStarted;
        GameEventManager.Instance.OnReloadFinished += HandleReloadFinished;
    }

    void Start()
    {
        if (defaultWeapon != null && currentWeapon == null)
        {
            currentWeapon = ObjectPoolManager.SpawnObject(defaultWeapon.weaponPrefab, weaponSocket, Quaternion.identity);
            currentWeapon.GetComponent<WeaponManager>().InitWeapon(defaultWeapon,Inventory);            
            GameEventManager.Instance.InvokeWeaponChanged(defaultWeapon);
        }
    }

    void OnDisable()
    {
        GameEventManager.Instance.OnArmorBroken -= DisableArmor;
        GameEventManager.Instance.OnArmorEquipped -= EnableArmor;
        GameEventManager.Instance.OnShootHold -= HandleShootHold;
        GameEventManager.Instance.OnShootRelease -= HandleShootRelease;
        GameEventManager.Instance.OnReloadStarted -= HandleReloadStarted;
        GameEventManager.Instance.OnReloadFinished -= HandleReloadFinished;
    }

    // ----- Armor -----
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

    // ----- Weapon -----
    public void EquipWeapon(ItemDataSO weaponData)
    {
        if (currentWeapon != null)
        {
            ObjectPoolManager.ReturnObjectToPool(currentWeapon);
        }

        if (weaponData.weaponPrefab != null)
        {
            currentWeapon = ObjectPoolManager.SpawnObject(weaponData.weaponPrefab, weaponSocket, Quaternion.identity);
            currentWeapon.GetComponent<WeaponManager>().InitWeapon(weaponData,Inventory);
        }

        GameEventManager.Instance.InvokeWeaponChanged(weaponData);
    }

    // ----- Shooting -----
    private void HandleShootHold()
    {
        if(!isReloading)
            animator.SetBool(shootHash, true);
    }

    private void HandleShootRelease()
    {
        animator.SetBool(shootHash, false);
    }

    // ----- Reload -----
    private void HandleReloadStarted()
    {
        isReloading = true;
        animator.SetBool(isReloadingHash, true);
        animator.SetTrigger(reloadHash); 
    }
    private void HandleReloadFinished()
    {
        isReloading = false;
        animator.SetBool(isReloadingHash, false);
    }


}