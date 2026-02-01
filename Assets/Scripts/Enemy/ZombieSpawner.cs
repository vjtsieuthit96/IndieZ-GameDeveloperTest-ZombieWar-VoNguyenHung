using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] zombiePrefabs;
    [SerializeField] private Transform player;

    private float spawnTimer;
    private float elapsedTime;

    [SerializeField] private float baseInterval = 10f;
    [SerializeField] private int baseZombiesPerPoint = 1;
    [SerializeField] private int maxZombiesPerPoint = 5;

    [Header("Global Limit")]
    [SerializeField] private int maxAliveZombies = 50;

    private int currentAliveZombies = 0;
    public static ZombieSpawner Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;           
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public int GetAliveCount()
    {
        return currentAliveZombies;
    }

    public void SetAliveCount(int count)
    {
        currentAliveZombies = count;
    }

    private void OnEnable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged += HandleSurvivalTimeChanged;
        GameEventManager.Instance.OnSurvivalMilestone += HandleSurvivalMilestone;
        GameEventManager.Instance.OnEnemyDie += HandleZombieDied; 
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged -= HandleSurvivalTimeChanged;
        GameEventManager.Instance.OnSurvivalMilestone -= HandleSurvivalMilestone;
        GameEventManager.Instance.OnEnemyDie -= HandleZombieDied;
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
        if (currentAliveZombies >= maxAliveZombies) return; 

        int zombiesPerPoint = GetCurrentZombiesPerPoint();

        foreach (Transform point in spawnPoints)
        {
            for (int i = 0; i < zombiesPerPoint; i++)
            {
                if (currentAliveZombies >= maxAliveZombies) return;

                Vector3 offset = Random.insideUnitSphere * 1.5f;
                offset.y = 0;
                Vector3 spawnPos = point.position + offset;

                GameObject prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
                GameObject zom = ObjectPoolManager.SpawnObject(prefab, spawnPos, point.rotation);
                zom.GetComponent<ZombieAI>().SetTarget(player);
                currentAliveZombies++; 
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
        maxZombiesPerPoint += 2;
        maxAliveZombies += 15;
    }

    private void HandleZombieDied(GameObject zombie)
    {
        currentAliveZombies = Mathf.Max(0, currentAliveZombies - 1);
    }
}