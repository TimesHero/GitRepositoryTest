using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    Rigidbody2D myRB;
     void Start()
    {
        StartCoroutine(killTimer());
    }
     
    private IEnumerator killTimer()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                Destroy(gameObject);
                other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(1);

        }        
    }

}

