using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip reloadClip;
    public void OnReloadAnimFinished()
    {        
        GameEventManager.Instance.InvokeReloadFinished();        
    }

    public void PlayReload()
    {
        audioSource.PlayOneShot(reloadClip);
    }
}
