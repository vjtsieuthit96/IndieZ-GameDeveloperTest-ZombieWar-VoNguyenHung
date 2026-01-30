using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private Image bloodImage;
    [SerializeField] private Sprite[] bloodSprites;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float bloodCooldown = 3f;
    private float lastBloodTime = -999f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] painSounds;


    private Coroutine currentFade;
    private bool isDead = false;

    private void Start()
    {
        bloodImage.enabled = false;
        bloodImage.raycastTarget = false;
    }

    private void OnEnable()
    {
        GameEventManager.Instance.OnPlayerTakeDamage += ShowBlood;
        GameEventManager.Instance.OnPlayerDied += HandleDeath;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnPlayerTakeDamage -= ShowBlood;
        GameEventManager.Instance.OnPlayerDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        isDead = true;    
    }

    private void ShowBlood(float damageAmount)
    {
        if (isDead) return;
        if (Time.time - lastBloodTime < bloodCooldown) return;
        lastBloodTime = Time.time;
        
        int index = Random.Range(0, bloodSprites.Length);
        bloodImage.sprite = bloodSprites[index];
        
        RectTransform parentRect = bloodImage.transform.parent as RectTransform;
        if (parentRect != null)
        {
            float x = Random.Range(-parentRect.rect.width / 2f, parentRect.rect.width / 2f);
            float y = Random.Range(-parentRect.rect.height / 2f, parentRect.rect.height / 2f);
            bloodImage.rectTransform.anchoredPosition = new Vector2(x, y);
        }
        
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        
        audioSource.PlayOneShot(painSounds[Random.Range(0, painSounds.Length)]);
        
        currentFade = StartCoroutine(BloodFade());
    }

    private IEnumerator BloodFade()
    {
        bloodImage.enabled = true;
        Color c = bloodImage.color;
        c.a = 1f;
        bloodImage.color = c;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            bloodImage.color = c;
            yield return null;
        }

        bloodImage.enabled = false;
        currentFade = null;
    }
}