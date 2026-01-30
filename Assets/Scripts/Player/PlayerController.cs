using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private GameObject armor;
    [SerializeField] private ItemDataSO defaultWeapon;
    [SerializeField] private Animator animator;
    [SerializeField] private Inventory Inventory;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip[] pickUpClip;
    [SerializeField] private AudioClip grenadeThrowClip;
    [SerializeField] private Transform grenadeSpawnPoint;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private Transform myTarget;

    private int shootHash = Animator.StringToHash("Shoot");
    private int reloadHash = Animator.StringToHash("Reload");
    private int isReloadingHash = Animator.StringToHash("isReloading");
    private int grenadeHash = Animator.StringToHash("Grenade");
    private GameObject currentWeapon;
    private bool isReloading = false;
    private bool restoredFromSave = false;

    private ItemDataSO equippedWeapon;
    void Start()
    {
        if (defaultWeapon != null && currentWeapon == null && !restoredFromSave)
        {
            currentWeapon = ObjectPoolManager.SpawnObject(defaultWeapon.weaponPrefab, weaponSocket, Quaternion.identity);
            currentWeapon.GetComponent<WeaponManager>().InitWeapon(defaultWeapon, Inventory);
            equippedWeapon = defaultWeapon;
            GameEventManager.Instance.InvokeWeaponChanged(defaultWeapon);
        }
        GameEventManager.Instance.InvokeGrenadeChanged(Inventory.GetGrenade());

    }
    void OnEnable()
    {
        GameEventManager.Instance.OnArmorBroken += DisableArmor;
        GameEventManager.Instance.OnArmorEquipped += EnableArmor;
        GameEventManager.Instance.OnShootHold += HandleShootHold;
        GameEventManager.Instance.OnShootRelease += HandleShootRelease;
        GameEventManager.Instance.OnReloadStarted += HandleReloadStarted;
        GameEventManager.Instance.OnReloadFinished += HandleReloadFinished;
        GameEventManager.Instance.OnGrenadeClicked += HandleGrenadeClicked;

    }

    void OnDisable()
    {
        GameEventManager.Instance.OnArmorBroken -= DisableArmor;
        GameEventManager.Instance.OnArmorEquipped -= EnableArmor;
        GameEventManager.Instance.OnShootHold -= HandleShootHold;
        GameEventManager.Instance.OnShootRelease -= HandleShootRelease;
        GameEventManager.Instance.OnReloadStarted -= HandleReloadStarted;
        GameEventManager.Instance.OnReloadFinished -= HandleReloadFinished;
        GameEventManager.Instance.OnGrenadeClicked -= HandleGrenadeClicked;

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
        equippedWeapon = weaponData;
        GameEventManager.Instance.InvokeWeaponChanged(weaponData);
        restoredFromSave = true;
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
    // ----- Grenade -----
    private void HandleGrenadeClicked()
    {
        if (Inventory != null && Inventory.UseGrenade() && !isReloading)
        {
            animator.SetTrigger(grenadeHash);
            GameObject grenade = ObjectPoolManager.SpawnObject(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation);

            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 targetPoint = myTarget.position;
                Vector3 dir = (targetPoint - grenadeSpawnPoint.position);
                dir.y = 0; 
                float angle = 45f; 
                Vector3 throwDir = Quaternion.AngleAxis(angle, grenadeSpawnPoint.right) * dir.normalized;
                rb.linearVelocity = throwDir * throwForce;
            }

            AudioSource.PlayOneShot(grenadeThrowClip);
        }
        else
        {
            Debug.Log("No grenades left!");
        }
    }


    #region Properties
    // ----- Properties -----
    public string GetCurrentWeaponName()
    {
        if (currentWeapon != null)
            return equippedWeapon.itemName;
        return "None";
    }
    public int GetCurrentAmmoInMag()
    {
        if (currentWeapon != null)
        {
            WeaponManager wm = currentWeapon.GetComponent<WeaponManager>();
            if (wm != null)
                return wm.CurrentAmmoInMag;
        }
        return 0;
    }
    public void SetCurrentAmmoInMag(int amount)
    {
        if (currentWeapon != null)
        {
            WeaponManager wm = currentWeapon.GetComponent<WeaponManager>();
            if (wm != null)
                wm.SetAmmoInMag(amount);
        }
    }
    public void PlayAudio(int index)
    {       
        AudioSource.PlayOneShot(pickUpClip[index]);
    }
    #endregion 

}