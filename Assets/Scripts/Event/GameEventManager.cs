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
    public event Action<float, float> OnPlayerStatsChanged; // HP, Armor
    public event Action OnPlayerDied;

    public void InvokePlayerStatsChanged(float hp, float armor)
    {
        OnPlayerStatsChanged?.Invoke(hp, armor);
    }

    public void InvokePlayerDied()
    {
        OnPlayerDied?.Invoke();
    }
    #endregion

}
