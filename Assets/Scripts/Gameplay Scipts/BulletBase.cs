using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class BulletBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float damage;
    float killTime;
    bool pierce; 
    bool enemyProjectile;
    Projectile bullet;
    Rigidbody2D myRB;
    public AudioSource sound;
    GameObject player; 

    void Start()
    {

    }
    public void PeramPass(Projectile currentProjectile)
    {
        bullet = currentProjectile;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyProjectile = currentProjectile.enemyProjectile;
        if (enemyProjectile==false)
        {
            damage = currentProjectile.damage * player.GetComponent<PlayerHPManager>().damageMultiplier;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = currentProjectile.bulletSprite;
        sound = GetComponent<AudioSource>();
        AudioClip clip = bullet.shootSound;
        sound.clip = clip;
        sound.Play();
        StartCoroutine(killTimer());
    }
    private IEnumerator killTimer()
    {
        yield return new WaitForSeconds(bullet.timeUntilDeath);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyProjectile==true)
        {
            if (other.tag == "Player")
            {
                Destroy(gameObject);
                other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(bullet.damage);

            }        
        }
        if (enemyProjectile==false)
        {
            if (other.tag == "Enemy")
            {
                    if (bullet.pierce==false)
                    {
                        Destroy(gameObject);
                    }
                    other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            if (other.tag == "Portal")
            {
                    if (bullet.pierce==false)
                    {
                        Destroy(gameObject);
                    }
                    other.gameObject.GetComponent<EnemySpawner>().TakeDamage(damage);
            }   
        }     
    }
}
