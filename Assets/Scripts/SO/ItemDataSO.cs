using UnityEngine;
public enum ItemType
{
    Weapon,
    Armor,
    Heal,
    Ammo,
    Grenade
}
[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemDataSO : ScriptableObject
{
    public ItemType itemType;   
    public string itemName;    
      
    [Header("Weapon Stats")]
    public float damage;
    public float fireRate;
    public int magazineSize;   
    public GameObject weaponPrefab;
    public Sprite icon;

    [Header("Ammo")]
    public int amountAmmo;

    [Header("Grenade")]
    public int amountGrenade;

    [Header("Armor Stats")]
    public float armorValue;

    [Header("Heal Stats")]
    public float healAmount;
}