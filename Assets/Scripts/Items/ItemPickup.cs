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
            Inventory playerInventory = other.GetComponentInParent<Inventory>();

            switch (itemData.itemType)
            {
                case ItemType.Weapon:
                    if (playerController != null)
                    {                        
                        playerController.EquipWeapon(itemData);
                    }
                    break;

                case ItemType.Armor:
                    if (playerStats != null)
                    {                        
                        playerStats.AddArmor(itemData); 
                    }
                    break;

                case ItemType.Heal:
                    if (playerStats != null)
                    {                        
                        playerStats.Heal(itemData);
                    }
                    break;

                case ItemType.Ammo:
                    if (playerInventory != null)
                        playerInventory.AddAmmo(itemData.amountAmmo);
                    break;

            }
            Invoke(nameof(Respawn), respawnDelay);
            gameObject.SetActive(false);

        }
    }
    private void Respawn()
    {
        gameObject.SetActive(true);
    }

}