using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Inventory inventory;
    [SerializeField] private SurvivalTimer survivalTimer;

    void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerHP"))
        {
            RestorePlayer();
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
        survivalTimer.SetElapsedTime(survivalTime);
        inventory.SetAmmo(reserveAmmo);
        inventory.SetGrenade(reserveGrenade);

        ItemDataSO weaponData = ItemDataBase.GetItemByName(currentWeaponName);
        if (weaponData != null)
        {
            playerController.EquipWeapon(weaponData);
            playerController.SetCurrentAmmoInMag(ammoInMag);
        }
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

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            int ammoInMag = playerController.GetCurrentAmmoInMag();
            string currentWeapon = playerController.GetCurrentWeaponName();

            SaveGame(ammoInMag, currentWeapon);
            Debug.Log("Auto-saved on pause");
        }
    }

    void OnApplicationQuit()
    {
        int ammoInMag = playerController.GetCurrentAmmoInMag();
        string currentWeapon = playerController.GetCurrentWeaponName();

        SaveGame(ammoInMag, currentWeapon);
        Debug.Log("Auto-saved on quit");
    }

    private void HandlePlayerDied()
    {
        PlayerPrefs.DeleteAll(); 
        Debug.Log("Player died, save cleared!");        
        SceneManager.LoadScene(0);
    }
}

