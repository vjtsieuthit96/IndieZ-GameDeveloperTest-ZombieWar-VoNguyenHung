using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIStatsWeaponDisplay : MonoBehaviour
{    
    [Header("Weapon")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TMP_Text magText;    
    [SerializeField] private TMP_Text reserveText;
    [SerializeField] private TMP_Text grenadeText;

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
        GameEventManager.Instance.OnAmmoChanged += UpdateAmmoUI;
        GameEventManager.Instance.OnReserveAmmoChanged += UpdateReserveOnlyUI;
        GameEventManager.Instance.OnGrenadeChanged += UpdateGrenadeUI;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnWeaponChanged -= UpdateWeaponUI;
        GameEventManager.Instance.OnPlayerStatsChanged -= UpdateStatsUI;
        GameEventManager.Instance.OnAmmoChanged -= UpdateAmmoUI;
        GameEventManager.Instance.OnReserveAmmoChanged -= UpdateReserveOnlyUI;
        GameEventManager.Instance.OnGrenadeChanged -= UpdateGrenadeUI;
    }


    private void UpdateWeaponUI(ItemDataSO weaponData)
    {
        if (weaponData != null && weaponIcon != null)
        {
            weaponIcon.sprite = weaponData.icon;
        }
    }
    private void UpdateAmmoUI(int current, int reserve)
    {
        magText.text = $"{current} /";
        reserveText.text = $" {reserve}";
    }

    private void UpdateReserveOnlyUI(int reserve)
    {
        reserveText.text = $" {reserve}";
    }
    private void UpdateGrenadeUI(int grenadeCount)
    {
        if (grenadeText != null)
        {
            grenadeText.text = $"{grenadeCount}";
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