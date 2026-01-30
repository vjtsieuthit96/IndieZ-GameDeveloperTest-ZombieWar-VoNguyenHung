using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonShoot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip nobulletClip;
    private bool isHolding = false;
    private bool isReloading = false;

    private void OnEnable()
    {
        GameEventManager.Instance.OnReloadStarted += HandleReloadStarted;
        GameEventManager.Instance.OnReloadFinished += HandleReloadFinished;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnReloadStarted -= HandleReloadStarted;
        GameEventManager.Instance.OnReloadFinished -= HandleReloadFinished;
    }

    private void HandleReloadStarted()
    {
        isReloading = true;
        if (isHolding)
        {
            audioSource.PlayOneShot(nobulletClip);
        }

    }

    private void HandleReloadFinished()
    {
        isReloading = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        if (isReloading)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(nobulletClip);
        }
        else
        {
            GameEventManager.Instance.TriggerShootHold();
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        GameEventManager.Instance.TriggerShootRelease();        
    }
}