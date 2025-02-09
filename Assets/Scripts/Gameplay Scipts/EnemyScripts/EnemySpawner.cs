using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject enemyBase;
    public EnemyBase[] enemyTypes;
    public float HP;
    bool dead = false;
    public bool temporary;
    private int spawnCount;
    public int spawnLimit;

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
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 100 * Time.deltaTime);
        if (dead==true)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.01f,transform.localScale.y - 0.01f,transform.localScale.z);
        }
    }

    private IEnumerator SpawnTimerNormal()
    {
        yield return new WaitForSeconds(4f);
        SpawnFunction();
        StartCoroutine(SpawnTimerNormal());
    }

     private IEnumerator SpawnTimerClumps()
    {
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
        int enemySpawnType = Random.Range(0,enemyTypes.Length);//grabs a random object from the array
        GameObject enemy = Instantiate(enemyBase, transform.position, Quaternion.identity);
        enemy.transform.localScale = enemyBase.transform.localScale;
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z - 1f);
        enemy.gameObject.GetComponent<Enemy>().PeramPass(enemyTypes[enemySpawnType]);
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
