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
     
      private IEnumerator killTimer(){
         yield return new WaitForSeconds(1);
         Destroy(gameObject);
      }

       private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemy"){//runs if the obstacle runs into the player
                Destroy(gameObject);//destroys itself
                other.gameObject.GetComponent<Enemy>().TakeDamage(1);//goes into the player perams and runs the take dmg function. 

            }        
        }

}

