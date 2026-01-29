using System.Collections;
using UnityEngine;

public class NPCHelper : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private float range = 25f;
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Animator animator;

    [Header("Impact Prefabs")]
    [SerializeField] private GameObject impactEnemy;
    [SerializeField] private GameObject bulletHoleEnemy;
    [SerializeField] private GameObject impactDefault;
    [SerializeField] private GameObject bulletHoleDefault;

    private float nextFireTime;
    private int shotsFired = 0;
    private bool isReloading = false;
    private int shootHash = Animator.StringToHash("Shoot");

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, zombieLayer);
        if (hits.Length > 0 && Time.time >= nextFireTime)
        {
            Transform target = GetVisibleZombie(hits);
            if (shotsFired >= 30 || isReloading)
                return;
            if (target != null)
            {
                RotateTowards(target);
                Shoot(target);
                animator.SetTrigger(shootHash);
                shootAudioSource.PlayOneShot(shootClip);
                if (!muzzleFlash.isPlaying)
                {
                    muzzleFlash.Play();
                }
                nextFireTime = Time.time + fireRate;
                shotsFired++;
                Debug.Log("Shots Fired: " + shotsFired);
                {
                    if(shotsFired >= 30)
                    {
                        StartCoroutine(ReloadDelay());
                    }
                }
            }
        }
    }

    private Transform GetVisibleZombie(Collider[] zombies)
    {
        foreach (var zombie in zombies)
        {
            Vector3 dir = (zombie.transform.position - firePoint.position).normalized;
            float dist = Vector3.Distance(firePoint.position, zombie.transform.position);

            if (Physics.Raycast(firePoint.position, dir, out RaycastHit hit, dist, zombieLayer | obstacleMask))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    return zombie.transform;
                }
            }
        }
        return null;
    }

    private void RotateTowards(Transform target)
    {
        Vector3 lookDir = (target.position - transform.position).normalized;
        lookDir.y = 0; 
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
    }

    private void Shoot(Transform target)
    {
        Vector3 dir = (target.position - firePoint.position).normalized;
        Vector3 start = firePoint.position;
        Vector3 end = firePoint.position + dir * 0.25f;

        if (Physics.CapsuleCast(start, end, radius, dir, out RaycastHit hit, range, zombieLayer))
        {          
            
            ZombieStats enemyStats = hit.collider.GetComponent<ZombieStats>() ?? hit.collider.GetComponentInParent<ZombieStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage);                
            }
            
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
        }
    }

    private IEnumerator ReloadDelay()
    {
        isReloading = true;
        float reloadTime = Random.Range(5f, 7f);
        yield return new WaitForSeconds(reloadTime);
        shotsFired = 0;
        isReloading = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}