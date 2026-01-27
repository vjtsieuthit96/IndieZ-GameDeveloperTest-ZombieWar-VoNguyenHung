using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStatsWeaponDisplay : MonoBehaviour
{    
    [Header("Weapon")]
    [SerializeField] private Image weaponIcon;

    [Header("Stats")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider armorSlider;  

    [Header("Blink Settings")]
    [SerializeField] private Image hpFillImage;
    [SerializeField] private Color blinkColor = Color.white;
    [SerializeField] private float blinkSpeed = 5f;
    [SerializeField] private float lowHpThreshold = 30f;

    private Coroutine blinkCoroutine;
 
    private void OnEnable()
    {
        GameEventManager.Instance.OnWeaponChanged += UpdateWeaponUI;
        GameEventManager.Instance.OnPlayerStatsChanged += UpdateStatsUI;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnWeaponChanged -= UpdateWeaponUI;
        GameEventManager.Instance.OnPlayerStatsChanged -= UpdateStatsUI;
    }
    
    private void UpdateWeaponUI(ItemDataSO weaponData)
    {
        if (weaponData != null && weaponIcon != null)
        {
            weaponIcon.sprite = weaponData.icon;
        }
    }
    private void UpdateStatsUI(float hp, float armor)
    {
        if (hpSlider != null)
        {
            hpSlider.value = hp;           
            
            if (hp <= lowHpThreshold)
            {
                if (blinkCoroutine == null)
                    blinkCoroutine = StartCoroutine(BlinkFill());
            }
            else
            {
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                    blinkCoroutine = null;
                    hpFillImage.color = Color.red; 
                }
            }
        }

        if (armorSlider != null)
        {
            armorSlider.value = armor;           
        }
    }
    private IEnumerator BlinkFill()
    {
        Color baseColor = hpFillImage.color;
        while (true)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            hpFillImage.color = Color.Lerp(baseColor, blinkColor, t);
            yield return null;
        }
    }
}