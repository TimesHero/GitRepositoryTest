using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerBulletTriple : MonoBehaviour
{

    Rigidbody2D myRB;
     void Start()
    {
        StartCoroutine(killTimer());
    }
     
    private IEnumerator killTimer()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
                Destroy(gameObject);
                other.gameObject.GetComponent<Enemy>().TakeDamage(1);

        }        
    }

}

