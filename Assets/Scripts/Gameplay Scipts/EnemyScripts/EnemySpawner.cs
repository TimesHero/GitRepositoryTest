using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform [] positions;
    public GameObject[] enemies;
    public GameObject playerEN;

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
        yield return new WaitForSeconds(6);
        SpawnFunction();
        StartCoroutine(SpawnTimer());
    }
    public void SpawnFunction()
    {
        int enemyType = Random.Range(0,4);//grabs a random object from the array
        int whereSpawn = Random.Range(0,2);//grabs a random spawn postion
        GameObject enemy = Instantiate(enemies[enemyType], positions[whereSpawn].position, Quaternion.identity);
        enemy.transform.localScale = enemies[enemyType].transform.localScale;
        
    }
}
