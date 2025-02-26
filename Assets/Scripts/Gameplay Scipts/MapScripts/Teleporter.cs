using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public GameObject fade; 
    private float opacity = 0; 
    private bool teleporting; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (teleporting==true)
        {
            opacity +=0.02f; 
            fade.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, opacity); 
        }
         if (teleporting==false&&opacity!=0)
        {
            opacity -=0.02f; 
            fade.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, opacity); 
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Teleport(other.gameObject));
        }        
    }
    private IEnumerator Teleport(GameObject player)
    {
        teleporting=true; 
        yield return new WaitForSeconds(0.4f);
        player.transform.position = new Vector3(0,-4,0); 
        yield return new WaitForSeconds(0.4f);
        teleporting=false; 
        
        
    }
}
