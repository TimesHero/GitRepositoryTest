using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HubGate : MonoBehaviour
{
    public GameObject fade; 
    private float opacity = 0; 
    private bool teleporting; 
    void Update()
    {
        if (teleporting==true)
        {
            opacity +=0.02f; 
            fade.GetComponent<Image>().color = new Color(0f, 0f, 0f, opacity); 
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
        SceneManager.LoadScene("MainLevelScene");
        
        
    }
}
