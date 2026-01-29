using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;      
    [SerializeField] private GameObject[] zombiePrefabs;
    [SerializeField] private GameObject player;

    private float spawnTimer;
    private float elapsedTime;
    
    [SerializeField] private float baseInterval = 10f;      
    [SerializeField] private int baseZombiesPerPoint = 1;   
    [SerializeField] private int maxZombiesPerPoint = 5;   

    private void OnEnable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged += HandleSurvivalTimeChanged;
        GameEventManager.Instance.OnSurvivalMilestone += HandleSurvivalMilestone;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged -= HandleSurvivalTimeChanged;
        GameEventManager.Instance.OnSurvivalMilestone -= HandleSurvivalMilestone;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= GetCurrentInterval())
        {
            SpawnWave();
            spawnTimer = 0f;
        }
    }

    private void SpawnWave()
    {
        int zombiesPerPoint = GetCurrentZombiesPerPoint();

        foreach (Transform point in spawnPoints)
        {
            for (int i = 0; i < zombiesPerPoint; i++)
            {
                Vector3 offset = Random.insideUnitSphere * 1.5f;
                offset.y = 0; 
                Vector3 spawnPos = point.position + offset;
                GameObject prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
                GameObject zom = ObjectPoolManager.SpawnObject(prefab, point.position, point.rotation);
                zom.GetComponent<ZombieAI>().SetTarget(player.transform);
                zom.transform.SetParent(point);
            }
        }
    }
    
    private void HandleSurvivalTimeChanged(float time)
    {
        elapsedTime = time;
    }
    
    private float GetCurrentInterval()
    {
        return Mathf.Max(0.5f, baseInterval / (1f + elapsedTime / 300f));
    }
    
    private int GetCurrentZombiesPerPoint()
    {
        int perPoint = baseZombiesPerPoint + (int)(elapsedTime / 60f);
        return Mathf.Min(perPoint, maxZombiesPerPoint);
    }
    
    private void HandleSurvivalMilestone(float milestone)
    {
        maxZombiesPerPoint += 5;        
    }
}