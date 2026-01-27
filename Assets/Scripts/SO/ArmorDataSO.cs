using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Game/Armor")]
public class ArmorDataSO : ScriptableObject
{
    public string armorName;
    public float armorValue;
}