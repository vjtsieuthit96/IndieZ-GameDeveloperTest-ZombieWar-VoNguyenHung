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

        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

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