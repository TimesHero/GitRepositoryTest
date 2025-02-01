using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class EnemyExplodingAttack : MonoBehaviour
{
    float currentInterval;
    float interval = 1.0f;
    public GameObject parentEnemy;
    public GameObject player;
    bool exploding = false;
    bool colliding=false;

    public float explosionTime = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame 
    void Update()
    {
       if (exploding==true)
       {
            player.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(2);
            Destroy(parentEnemy);
       }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            parentEnemy.gameObject.GetComponent<Enemy>().stationary=true;
            StartCoroutine(killTimer());
            colliding=true;
            
        }        
    }

    private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        parentEnemy.gameObject.GetComponent<Enemy>().stationary=false;
        StopCoroutine(killTimer());
        colliding=false;
    }
}
    
    private IEnumerator killTimer()
    {
        yield return new WaitForSeconds(explosionTime);
        if (colliding==true)
        {
            exploding=true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            Destroy(parentEnemy);
        }
    }
}
