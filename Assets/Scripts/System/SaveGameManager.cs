using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerController playerController;
    private Inventory inventory;

    [SerializeField] private SurvivalTimer survivalTimer;
    [SerializeField] private GameObject Player;
    [SerializeField] private NPCManager npcManager;

    void Awake()
    {
        playerStats = Player.GetComponent<PlayerStats>();
        playerController = Player.GetComponent<PlayerController>();
        inventory = Player.GetComponent<Inventory>();

        if (PlayerPrefs.HasKey("PlayerHP"))
        {
            RestorePlayer();
            SurvivalLeaderboard.Instance.LoadRecords();
        }
        else
        {
            Debug.Log("No save data found, starting fresh.");
        }
    }

    void OnEnable()
    {
        GameEventManager.Instance.OnSurvivalMilestone += HandleSurvivalMilestone;
        GameEventManager.Instance.OnPlayerDied += HandlePlayerDied;
    }

    void OnDisable()
    {
        GameEventManager.Instance.OnSurvivalMilestone -= HandleSurvivalMilestone;
        GameEventManager.Instance.OnPlayerDied -= HandlePlayerDied;
    }

    // ---------------- SAVE ----------------
    public void SaveGame(int ammoInMag, string currentWeapon)
    {
        PlayerPrefs.SetInt("SceneIndex", SceneManager.GetActiveScene().buildIndex);

        PlayerPrefs.SetFloat("PlayerHP", playerStats.CurrentHP);
        PlayerPrefs.SetFloat("PlayerArmor", playerStats.CurrentArmor);
        PlayerPrefs.SetFloat("SurvivalTime", survivalTimer.ElapsedTime);

        PlayerPrefs.SetInt("AmmoInMag", ammoInMag);
        PlayerPrefs.SetInt("ReserveAmmo", inventory.GetAmmo());
        PlayerPrefs.SetInt("ReserveGrenade", inventory.GetGrenade());
        PlayerPrefs.SetString("CurrentWeapon", currentWeapon);
                
        int spawnedCount = npcManager.GetSpawnedCount();
        PlayerPrefs.SetInt("SpawnedNPCCount", spawnedCount);

        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    // ---------------- RESTORE ----------------
    public void RestorePlayer()
    {
        float hp = PlayerPrefs.GetFloat("PlayerHP", 100);
        float armor = PlayerPrefs.GetFloat("PlayerArmor", 0);
        float survivalTime = PlayerPrefs.GetFloat("SurvivalTime", 0);

        int reserveAmmo = PlayerPrefs.GetInt("ReserveAmmo", 0);
        int reserveGrenade = PlayerPrefs.GetInt("ReserveGrenade", 0);
        int ammoInMag = PlayerPrefs.GetInt("AmmoInMag", 0);
        string currentWeaponName = PlayerPrefs.GetString("CurrentWeapon", "None");

        playerStats.SetStats(hp, armor);
        inventory.SetAmmo(reserveAmmo);
        inventory.SetGrenade(reserveGrenade);

        ItemDataSO weaponData = ItemDataBase.GetItemByName(currentWeaponName);
        if (weaponData != null)
        {
            playerController.EquipWeapon(weaponData);
            Debug.Log(weaponData.itemName + " equipped on restore.");
            playerController.SetCurrentAmmoInMag(ammoInMag);
        }
        
        int spawnedCount = PlayerPrefs.GetInt("SpawnedNPCCount", 0);
        npcManager.SpawnNPCs(spawnedCount);

        survivalTimer.SetElapsedTime(survivalTime, new List<int>());
        Debug.Log("Player restored from save!");
    }

    // ---------------- AUTO SAVE ----------------
    private void HandleSurvivalMilestone(float milestone)
    {
        int ammoInMag = playerController.GetCurrentAmmoInMag();
        string currentWeapon = playerController.GetCurrentWeaponName();

        SaveGame(ammoInMag, currentWeapon);
        Debug.Log("Auto-saved at milestone: " + milestone);
    }

    private void HandlePlayerDied()
    {
        ClearPlayerSaved();
    }

    // ---------------- CLEAR ----------------
    public void ClearPlayerSaved()
    {
        int lbCount = PlayerPrefs.GetInt("LeaderboardCount", 0);
        List<float> leaderboard = new List<float>();
        for (int i = 0; i < lbCount; i++)
        {
            leaderboard.Add(PlayerPrefs.GetFloat("Leaderboard_" + i, 0f));
        }

        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("LeaderboardCount", leaderboard.Count);
        for (int i = 0; i < leaderboard.Count; i++)
        {
            PlayerPrefs.SetFloat("Leaderboard_" + i, leaderboard[i]);
        }

        PlayerPrefs.SetFloat("MusicVolume", musicVol);
        PlayerPrefs.SetFloat("SFXVolume", sfxVol);
        PlayerPrefs.Save();
    }
}