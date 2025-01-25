using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] enemies;

    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(8);
        SpawnFunction();
        StartCoroutine(SpawnTimer());
    }
    public void SpawnFunction()
    {
        int enemyType = Random.Range(0,4);//grabs a random object from the array
        GameObject enemy = Instantiate(enemies[enemyType], transform.position, Quaternion.identity);
        enemy.transform.localScale = enemies[enemyType].transform.localScale;
        
    }
}
