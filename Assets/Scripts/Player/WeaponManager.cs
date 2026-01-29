using UnityEngine;
using UnityEngine.Rendering;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private float laserMaxDistance = 50f;
    [SerializeField] private LayerMask weaponLayer;
    [SerializeField] private Inventory playerInventory;
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
    private int reloadHash = Animator.StringToHash("Reload");
    private int recoilHash = Animator.StringToHash("Recoil");
    private bool isHolding;
    private bool isReloading;

    public void InitWeapon(ItemDataSO weaponData,Inventory inventory)
    {
        playerInventory = inventory;
        damage = weaponData.damage;
        fireRate = weaponData.fireRate;
        magazineSize = weaponData.magazineSize;
        currentAmmoInMag = magazineSize;
        laserLine.enabled = true;
        GameEventManager.Instance.InvokeAmmoChanged(currentAmmoInMag, playerInventory.GetAmmo());
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

    private void Start()
    {
        laserLine.enabled = true;
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
        if (!isReloading && currentAmmoInMag < magazineSize) StartReload();
    }

    void Update()
    {
        if (isHolding && Time.time >= nextFireTime && currentAmmoInMag > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        if (currentAmmoInMag <= 0 && !isReloading && playerInventory.GetAmmo() > 0)
        {
            StartReload();
        }
        UpdateLaser();
    }

    private void Shoot()
    {
        if (currentAmmoInMag <= 0 || isReloading) return;

        RaycastHit hit;
        if (!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        // Capsule bullet parameters
        Vector3 start = firePoint.position;
        Vector3 end = firePoint.position + firePoint.forward * 0.25f;
        float radius = 0.3f;
        int mask = ~weaponLayer;

        if (Physics.CapsuleCast(start, end, radius, firePoint.forward, out hit, 25f, mask))
        {
            animator.SetTrigger(recoilHash);

            if (hit.collider.CompareTag("Enemy"))
            {
                ZombieStats enemyStats = hit.collider.GetComponent<ZombieStats>();
                if (enemyStats == null)
                {
                    enemyStats = hit.collider.GetComponentInParent<ZombieStats>();
                }
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(damage);
                }
                ObjectPoolManager.SpawnObject(impactEnemy, hit.point, Quaternion.LookRotation(hit.normal));
                ObjectPoolManager.SpawnObject(bulletHoleEnemy, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                ObjectPoolManager.SpawnObject(impactDefault, hit.point, Quaternion.LookRotation(hit.normal));
                ObjectPoolManager.SpawnObject(bulletHoleDefault, hit.point, Quaternion.LookRotation(hit.normal));
            }

            currentAmmoInMag--;
            GameEventManager.Instance.InvokeAmmoChanged(currentAmmoInMag, playerInventory.GetAmmo());
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
        int toReload = playerInventory.UseAmmo(needed);

        currentAmmoInMag += toReload;
        isReloading = false;

        GameEventManager.Instance.InvokeAmmoChanged(currentAmmoInMag, playerInventory.GetAmmo());
    }

    private void UpdateLaser()
    {
        laserLine.SetPosition(0, firePoint.position);

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, laserMaxDistance))
        {
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, firePoint.position + firePoint.forward * laserMaxDistance);
        }
    }
}