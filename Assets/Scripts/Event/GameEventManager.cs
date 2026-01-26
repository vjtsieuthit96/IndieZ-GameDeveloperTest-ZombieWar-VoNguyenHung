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
    // Event for joystick movement
    public event Action<Vector2>OnMoveJoystick;
    public event Action OnMoveRelease;


    //Triger Methods
    public void TriggerMove(Vector2 input)
    {
        OnMoveJoystick?.Invoke(input);
    }
    public void TriggerMoveRelease()
    {
        OnMoveRelease?.Invoke();
    }
}
