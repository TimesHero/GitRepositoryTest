using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Audio;

public class BulletBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float damage;
    float killTime;
    bool pierce; 
    bool enemyProjectile;
    Projectile bullet;
    Rigidbody2D myRB;
    private AudioSource sound;
    public AudioMixer audioMixer;
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
        AudioManager.Instance.PlaySound(currentProjectile.shootSound);

        GameObject particles = Instantiate(currentProjectile.particleEffect, transform.position, transform.rotation);
        particles.transform.SetParent(transform);

        StartCoroutine(killTimer());
    }
    private IEnumerator killTimer()
    {
        yield return new WaitForSeconds(bullet.timeUntilDeath);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles") && !other.CompareTag("Void") || other.tag=="melee")
        {
            Instantiate(bullet.collideEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            AudioManager.Instance.PlaySound(bullet.collideSound); 
        }
        
        if (enemyProjectile==true)
        {
            if (other.tag == "Player")
            {
                Destroy(gameObject);
                other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(bullet.damage);
                Instantiate(bullet.collideEffect, transform.position, transform.rotation);
                AudioManager.Instance.PlaySound(bullet.collideSound); 

            }        
        }
        if (enemyProjectile==false)
        {
            if (other.tag == "Enemy")
            {
                    Instantiate(bullet.collideEffect, transform.position, transform.rotation);
                    AudioManager.Instance.PlaySound(bullet.collideSound); 
                    if (bullet.pierce==false)
                    {
                        Destroy(gameObject);
                    }
                    other.gameObject.GetComponent<EnemyHPManager>().TakeDamage(damage);
            }
            if (other.tag == "Portal")
            {
                    Instantiate(bullet.collideEffect, transform.position, transform.rotation);
                    AudioManager.Instance.PlaySound(bullet.collideSound); 
                    if (bullet.pierce==false)
                    {
                        Destroy(gameObject);
                    }
                    other.gameObject.GetComponent<EnemySpawner>().TakeDamage(damage);
            }  
            
        }     
    }
}
