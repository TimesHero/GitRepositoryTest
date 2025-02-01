using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class BulletBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int damage;
    float killTime;
    bool pierce; 
    bool enemyProjectile;
    void Start()
    {
        
    }
    public void PeramPass(Projectile currentProjectile)
    {
        damage = currentProjectile.damage;
        killTime = currentProjectile.timeUntilDeath; 
        pierce = currentProjectile.pierce;
        enemyProjectile = currentProjectile.enemyProjectile;
        gameObject.GetComponent<SpriteRenderer>().sprite = currentProjectile.bulletSprite;
        StartCoroutine(killTimer());
    }
    private IEnumerator killTimer()
    {
        yield return new WaitForSeconds(killTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyProjectile==true)
        {
            if (other.tag == "Player")
            {
                Destroy(gameObject);
                other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(1);

            }        
        }
        if (enemyProjectile==false)
        {
            if (other.tag == "Enemy")
            {
                    if (pierce==false)
                    {
                        Destroy(gameObject);
                    }
                    other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            if (other.tag == "Portal")
            {
                    if (pierce==false)
                    {
                        Destroy(gameObject);
                    }
                    other.gameObject.GetComponent<EnemySpawner>().TakeDamage(damage);
            }   
        }     
    }
}
