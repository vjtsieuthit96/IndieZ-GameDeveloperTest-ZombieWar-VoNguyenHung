using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float respawnDelay = 30f;

    private void Update()
    {       
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && itemData != null)
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            PlayerStats playerStats = other.GetComponentInParent<PlayerStats>();

            switch (itemData.itemType)
            {
                case ItemType.Weapon:
                    if (playerController != null)
                    {
                        Debug.Log("Player picked up weapon: " + itemData.itemName);
                        playerController.EquipWeapon(itemData);
                    }
                    break;

                case ItemType.Armor:
                    if (playerStats != null)
                    {
                        Debug.Log("Player picked up armor: " + itemData.itemName);
                        playerStats.AddArmor(itemData); 
                    }
                    break;

                case ItemType.Heal:
                    if (playerStats != null)
                    {
                        Debug.Log($"Player healed: +{itemData.healAmount}");
                        playerStats.Heal(itemData.healAmount);
                    }
                    break;
            }
            
            StartCoroutine(RespawnAfterDelay(respawnDelay));
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(true);
    }
}