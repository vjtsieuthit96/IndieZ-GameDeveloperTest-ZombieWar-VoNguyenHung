using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimEvent : MonoBehaviour
{   
    public void OnReloadAnimFinished()
    {        
        GameEventManager.Instance.InvokeReloadFinished();        
    }
}
