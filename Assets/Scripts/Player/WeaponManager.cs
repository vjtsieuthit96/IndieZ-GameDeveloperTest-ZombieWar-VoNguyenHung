using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private int maxAmmo = 300;
    [Header("Impact Prefabs")]
    [SerializeField] private GameObject impactEnemy;
    [SerializeField] private GameObject bulletHoleEnemy;
    [SerializeField] private GameObject impactDefault;
    [SerializeField] private GameObject bulletHoleDefault;


    private float damage;
    private float fireRate;
    private float nextFireTime;
    private int magazineSize;
    private int currentAmmoInMag;
    private int reserveAmmo;
    private int reloadHash = Animator.StringToHash("Reload");
    private int recoilHash = Animator.StringToHash("Recoil");
    private bool isHolding;
    private bool isReloading;
    public void InitWeapon(ItemDataSO weaponData)
    {
        damage = weaponData.damage;
        fireRate = weaponData.fireRate;
        magazineSize = weaponData.magazineSize;      
        currentAmmoInMag = magazineSize;        
        reserveAmmo = Mathf.Max(0, maxAmmo - magazineSize);
        GameEventManager.Instance.InvokeAmmoChanged(currentAmmoInMag, reserveAmmo);
        Debug.Log($"Init weapon: Mag = {currentAmmoInMag}, Reserve = {reserveAmmo}");
    }
    void OnEnable()
    {
        GameEventManager.Instance.OnShootHold += HandleShootHold;
        GameEventManager.Instance.OnShootRelease += HandleShootRelease;
        GameEventManager.Instance.OnReloadClicked += HandleReloadClicked;
        GameEventManager.Instance.OnReloadFinished += HandleReloadFinished;
    }

    void OnDisable()
    {
        GameEventManager.Instance.OnShootHold -= HandleShootHold;
        GameEventManager.Instance.OnShootRelease -= HandleShootRelease;
        GameEventManager.Instance.OnReloadClicked -= HandleReloadClicked;
        GameEventManager.Instance.OnReloadFinished -= HandleReloadFinished;

    }

    private void HandleShootHold()
    {
        isHolding = true;
    }

    private void HandleShootRelease()
    {
        isHolding = false;      
    }
    private void HandleReloadClicked()
    {
        if (!isReloading) StartReload();
    }

    void Update()
    {
        if (isHolding && Time.time >= nextFireTime && currentAmmoInMag > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        if (currentAmmoInMag <= 0 && !isReloading && reserveAmmo > 0)
        {
            StartReload();
        }
    }

    private void Shoot()
    {
        if (currentAmmoInMag <= 0 || isReloading) return;        
        RaycastHit hit;
        if (!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 25f))
        {
            animator.SetTrigger(recoilHash);

            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                ObjectPoolManager.SpawnObject(impactEnemy, hit.point, Quaternion.LookRotation(hit.normal));
                ObjectPoolManager.SpawnObject(bulletHoleEnemy, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                ObjectPoolManager.SpawnObject(impactDefault, hit.point, Quaternion.LookRotation(hit.normal));
                ObjectPoolManager.SpawnObject(bulletHoleDefault, hit.point, Quaternion.LookRotation(hit.normal));
            }

            currentAmmoInMag--;            
            Debug.Log($"[Shoot] Ammo in mag: {currentAmmoInMag} / Reserve: {reserveAmmo}");
            GameEventManager.Instance.InvokeAmmoChanged(currentAmmoInMag, reserveAmmo);
        }
    }
    private void StartReload()
    {
        isReloading = true;
        animator.SetTrigger(reloadHash);
        GameEventManager.Instance.InvokeReloadStarted();
    }
    private void HandleReloadFinished()
    {        
        int needed = magazineSize - currentAmmoInMag;
        
        int toReload = Mathf.Min(needed, reserveAmmo);
        
        currentAmmoInMag += toReload;
        
        reserveAmmo -= toReload;

        isReloading = false;

        GameEventManager.Instance.InvokeAmmoChanged(currentAmmoInMag, reserveAmmo);
        Debug.Log($"Reloaded. Ammo in mag: {currentAmmoInMag} / Reserve: {reserveAmmo}");
    }
}
