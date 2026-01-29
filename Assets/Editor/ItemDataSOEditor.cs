using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataSO))]
public class ItemDataSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemDataSO item = (ItemDataSO)target;
        
        item.itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", item.itemType);
        item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);

        // Show field by type
        switch (item.itemType)
        {
            case ItemType.Weapon:
                EditorGUILayout.LabelField("Weapon Stats", EditorStyles.boldLabel);
                item.damage = EditorGUILayout.FloatField("Damage", item.damage);
                item.fireRate = EditorGUILayout.FloatField("Fire Rate", item.fireRate);
                item.magazineSize = EditorGUILayout.IntField("Magazine Size", item.magazineSize);
                item.weaponPrefab = (GameObject)EditorGUILayout.ObjectField("Weapon Prefab", item.weaponPrefab, typeof(GameObject), false);
                item.icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.icon, typeof(Sprite), false);
                break;

            case ItemType.Ammo:
                EditorGUILayout.LabelField("Ammo Stats", EditorStyles.boldLabel);
                item.amountAmmo = EditorGUILayout.IntField("Amount Ammo", item.amountAmmo);
                break;

            case ItemType.Grenade:
                EditorGUILayout.LabelField("Grenade Stats", EditorStyles.boldLabel);
                item.amountGrenade = EditorGUILayout.IntField("Amount Grenade", item.amountGrenade);
                break;

            case ItemType.Armor:
                EditorGUILayout.LabelField("Armor Stats", EditorStyles.boldLabel);
                item.armorValue = EditorGUILayout.FloatField("Armor Value", item.armorValue);
                break;

            case ItemType.Heal:
                EditorGUILayout.LabelField("Heal Stats", EditorStyles.boldLabel);
                item.healAmount = EditorGUILayout.FloatField("Heal Amount", item.healAmount);
                break;
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(item);
        }
    }
}