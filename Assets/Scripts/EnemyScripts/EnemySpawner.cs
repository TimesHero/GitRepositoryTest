using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public  GameObject[] enemies;
    public float HP;
    bool dead = false;
    public bool temporary;
    private int spawnCount;
    public int spawnLimit;
    public AudioSource spawnSound;
    private GameObject logicManager; 
    void Start()
    {
        if (temporary==true)
        {
            StartCoroutine(SpawnTimerClumps());
        }
        else
        { 
            StartCoroutine(SpawnTimerNormal());
        }
        logicManager = GameObject.FindGameObjectWithTag("Manager");
    }

    // Update is called once per frame
    void Update()
    {
        if (dead==true)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.01f,transform.localScale.y - 0.01f,transform.localScale.z);
        }
        if (logicManager.GetComponent<GameHandler>().gameEnded==true)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator SpawnTimerNormal()
    {
        yield return new WaitForSeconds(4f);
        spawnSound.Play();
        yield return new WaitForSeconds(1f);
        SpawnFunction();
        StartCoroutine(SpawnTimerNormal());
    }

     private IEnumerator SpawnTimerClumps()
    {
        yield return new WaitForSeconds(1f);
        spawnSound.Play();
        yield return new WaitForSeconds(1f);
        SpawnFunction();
        spawnCount++;
        if (spawnCount==spawnLimit)
        {
           dead=true;
           StartCoroutine(KillTime());
        }
        StartCoroutine(SpawnTimerClumps());
    }
    public void SpawnFunction()
    {
        int enemySpawnType = Random.Range(0,enemies.Length);//grabs a random object from the array
        GameObject enemy = Instantiate(enemies[enemySpawnType], transform.position, Quaternion.identity);
        enemy.transform.localScale = enemies[enemySpawnType].transform.localScale;
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
    }

     public void TakeDamage(float damage)
    {
        HP-= damage;
        StartCoroutine(DmgFlash());
        if (HP<=0)
        {
           dead=true;
           StartCoroutine(KillTime());
        }
    }
    private IEnumerator KillTime()
    {
       yield return new WaitForSeconds(0.2f); 
       Destroy(gameObject);
    }
    private IEnumerator DmgFlash()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0f, 0f, 1f); 
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
