using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable Objects/Projectile")]
public class Projectile : ScriptableObject
{
    public GameObject projectilePrefab;
    public int damage;
    public int velocity;
    public float manaCost; 
    public float timeBetweenShots; 
    public float atkSpeedUpTimeBetweenShots;
    public float timeUntilDeath;
    public GameObject VFx;
    public bool enemyProjectile;
    public bool pierce;
    public bool triple;
    public Sprite bulletSprite; 
    public GameObject particleEffect;
    public GameObject collideEffect;
    public AudioClip shootSound;
    public AudioClip collideSound;
    
}
