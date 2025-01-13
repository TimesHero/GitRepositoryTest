using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
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
        if (other.tag == "Enemy")
        {
                Destroy(gameObject);//destroys itself
                other.gameObject.GetComponent<Enemy>().TakeDamage(1);//goes into the player perams and runs the take dmg function. 

        }        
    }

}

