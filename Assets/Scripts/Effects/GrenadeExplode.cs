using UnityEngine;

public class GrenadeExplode : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float maxDamage = 100f;
    [SerializeField] private float minDamage = 20f;
    [SerializeField] private GameObject explosionEffectPrefab;

    [Header("Camera Shake Settings")]
    [SerializeField] private float maxShakeIntensity = 100f;
    [SerializeField] private float minShakeIntensity = 30f;

    private bool isExploded = false;

    private void OnEnable()
    {
        if (!isExploded)
        {
            isExploded = true;
            float delay = Random.Range(3f, 5f);
            Invoke(nameof(Explode), delay);
        }
    }
    

    private void OnDisable()
    {
        isExploded = false;       
    }
    private void Explode()
    {        
        ObjectPoolManager.SpawnObject(explosionEffectPrefab, transform.position, Quaternion.identity);
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {            
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            float t = Mathf.Clamp01(distance / explosionRadius);
            float damage = Mathf.Lerp(maxDamage, minDamage, t);

            if (hit.CompareTag("Enemy"))
            {
                ZombieStats enemy = hit.GetComponent<ZombieStats>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            else if (hit.CompareTag("Player"))
            {
                if (CameraShake.Instance != null)
                {
                    float shakeIntensity = Mathf.Lerp(maxShakeIntensity, minShakeIntensity, t);
                    float shakeFrequency = Mathf.Lerp(5f, 2f, t);
                    CameraShake.Instance.Shake(shakeIntensity, shakeFrequency, 0.3f);
                }
                PlayerStats player = hit.GetComponent<PlayerStats>();
                if (player != null)
                {
                    player.SelfDamage(damage);
                }
            }
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

}