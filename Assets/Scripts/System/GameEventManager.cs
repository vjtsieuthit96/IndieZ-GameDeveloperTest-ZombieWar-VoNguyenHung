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
    // Joystick Events
    #region Joystick 
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
    #endregion
    // Player Stats Events
    #region
    public event Action<float, float> OnPlayerStatsChanged; 
    public event Action OnPlayerDied;
    public event Action OnArmorBroken;
    public event Action OnArmorEquipped;

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

}
