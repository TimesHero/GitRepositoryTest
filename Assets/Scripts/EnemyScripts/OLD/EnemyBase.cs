
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBase", menuName = "Scriptable Objects/EnemyBase")]
public class EnemyBase : ScriptableObject
{
    public float HP;
    public float moveSpeed;
    public string target;
    public bool meleeAttack;
    public bool rangedAttack;
    public bool explodingAttack;
    public bool eightWayAttack;
    public bool stationary;
    public float attackInterval;
    public float attackRange;
    public Sprite enemySprite;
    public Projectile bulletType;

}
