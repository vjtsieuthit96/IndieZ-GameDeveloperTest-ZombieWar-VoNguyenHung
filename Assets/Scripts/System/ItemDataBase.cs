using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/ItemDatabase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] private List<ItemDataSO> allItems;

    private static ItemDataBase instance;

    public static void Init(ItemDataBase db)
    {
        instance = db;
    }

    public static ItemDataSO GetItemByName(string name)
    {
        if (instance == null) return null;
        return instance.allItems.Find(i => i.itemName == name);
    }
}