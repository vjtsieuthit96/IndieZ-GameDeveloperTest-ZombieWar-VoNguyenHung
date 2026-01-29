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
                        playerController.PlayAudio(0);
                    }
                    break;

                case ItemType.Armor:
                    if (playerStats != null)
                    {                        
                        playerStats.AddArmor(itemData);
                        playerController.PlayAudio(0);
                    }
                    break;

                case ItemType.Heal:
                    if (playerStats != null)
                    {                        
                        playerStats.Heal(itemData);
                        playerController.PlayAudio(1);
                    }
                    break;

                case ItemType.Ammo:
                    if (playerInventory != null)
                        playerInventory.AddAmmo(itemData.amountAmmo);
                    playerController.PlayAudio(0);
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