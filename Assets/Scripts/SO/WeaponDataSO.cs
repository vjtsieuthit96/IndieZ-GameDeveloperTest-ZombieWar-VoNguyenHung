using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float fireRate;
}