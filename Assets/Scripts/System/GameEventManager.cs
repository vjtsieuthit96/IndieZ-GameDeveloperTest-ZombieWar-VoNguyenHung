using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance;
   
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // UI Events
    #region UI     
    // Sound
    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;
   
    public void InvokeMusicVolumeChanged(float value)
    {
        OnMusicVolumeChanged?.Invoke(value);
    }

    public void InvokeSFXVolumeChanged(float value)
    {
        OnSFXVolumeChanged?.Invoke(value);
    }   
    // Joystick Events
    public event Action<Vector2>OnMoveJoystick;
    public event Action OnMoveRelease;
   
    public void TriggerMove(Vector2 input)
    {
        OnMoveJoystick?.Invoke(input);
    }
    public void TriggerMoveRelease()
    {
        OnMoveRelease?.Invoke();
    }
    // Button Events
    public event Action OnShootHold;
    public event Action OnShootRelease;
    public event Action OnReloadClicked;
    public event Action OnGrenadeClicked;

    public void TriggerShootHold()
    {
        OnShootHold?.Invoke();
    }

    public void TriggerShootRelease()
    {
        OnShootRelease?.Invoke();
    }

    public void TriggerReloadClicked()
    {
        OnReloadClicked?.Invoke();
    }
    public void TriggerGrenadeClicked()
    {
        OnGrenadeClicked?.Invoke();
    }   
    
    #endregion
    // Player Stats Events
    #region
    public event Action<float, float> OnPlayerStatsChanged; 
    public event Action OnPlayerDied;
    public event Action OnArmorBroken;
    public event Action OnArmorEquipped;
    public event Action<float> OnPlayerTakeDamage;

    public void InvokePlayerTakeDamage(float amount)
    {
        OnPlayerTakeDamage?.Invoke(amount);
    }

    public void InvokePlayerStatsChanged(float hp, float armor)
    {
        OnPlayerStatsChanged?.Invoke(hp, armor);
    }
    public void InvokeArmorBroken()
    {
        OnArmorBroken?.Invoke();
    }

    public void InvokeArmorEquipped()
    {
        OnArmorEquipped?.Invoke();
    }

    public void InvokePlayerDied()
    {
        OnPlayerDied?.Invoke();
    }
    #endregion
    // Weapon Events
    #region Weapon
    public event Action<ItemDataSO> OnWeaponChanged;
    public event Action<int, int> OnAmmoChanged;
    public event Action OnReloadStarted;
    public event Action OnReloadFinished;
    public event Action<int> OnReserveAmmoChanged;
    public event Action<int> OnGrenadeChanged;
    public void InvokeGrenadeChanged(int newValue)
    {
        OnGrenadeChanged?.Invoke(newValue);
    }
    public void InvokeReserveAmmoChanged(int reserveAmmo)
    {
        OnReserveAmmoChanged?.Invoke(reserveAmmo);
    }

    public void InvokeAmmoChanged(int current, int reserve)
    {
        OnAmmoChanged?.Invoke(current, reserve);
    }
    public void InvokeReloadStarted() 
    {
        OnReloadStarted?.Invoke(); 
    }
    public void InvokeReloadFinished()
    {
        OnReloadFinished?.Invoke();
    }   
    public void InvokeWeaponChanged(ItemDataSO weaponData)
    {
        OnWeaponChanged?.Invoke(weaponData);
    }        
    #endregion

    #region GAMEPLAY
    // Survival Timer Events
    public event Action<float> OnSurvivalTimeChanged;
    public event Action<float> OnSurvivalMilestone;

    public void InvokeSurvivalTimeChanged(float time)
    {
        OnSurvivalTimeChanged?.Invoke(time);
    }

    public void InvokeSurvivalMilestone(float milestone)
    {
        OnSurvivalMilestone?.Invoke(milestone);
    }

    #endregion

    #region EnemyEvents
    //Enemy Events
    public event Action<GameObject> OnEnemyDie;
    public event Action<GameObject> OnEnemyHit;
    public void InvokeEnemyDie(GameObject enemy)
    {
        OnEnemyDie?.Invoke(enemy);
    }
    public void InvokeEnemyHit(GameObject enemy)
    {
        OnEnemyHit?.Invoke(enemy);
    }
    #endregion

    #region NPC Events    
    public event Action OnHelicopterTakeOff;
    public event Action OnHelicopterArrived;   
    public void InvokeHelicopterArrived()
    {
        OnHelicopterArrived?.Invoke();
    }
    public void InvokeHelicopterTakeOff()
    {
        OnHelicopterTakeOff?.Invoke();
    }

    #endregion

}
