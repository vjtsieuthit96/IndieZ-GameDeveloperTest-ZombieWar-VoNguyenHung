using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game/Player")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float maxArmor = 0f;

    [Header("Default Equipment")]
    public ItemDataSO defaultWeapon;
    public ItemDataSO defaultArmor;
}