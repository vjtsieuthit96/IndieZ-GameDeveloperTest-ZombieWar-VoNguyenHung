using UnityEngine;
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemy")]

public class EnemyDataSO : ScriptableObject
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float damage;
    public float moveSpeed;
}
