using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private int spawnedCount = 0;

    private void OnEnable()
    {
        GameEventManager.Instance.OnHelicopterArrived += HandleHelicopterArrived;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnHelicopterArrived -= HandleHelicopterArrived;
    }

    private void HandleHelicopterArrived()
    {
        SpawnNPC();
    }

    public void SpawnNPC()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        spawnedCount++;
        Debug.Log($"Spawned NPC #{spawnedCount} at {spawnPoint.name}");
    }

    public void SpawnNPCs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];
            Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        spawnedCount = count;       
    }

    public int GetSpawnedCount()
    {
        return spawnedCount;
    }  
}