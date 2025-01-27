using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
public class PlayerHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP;
    public GameObject logicManager;
    public Slider HPBar;
    public bool invincible = false; 
    void Start()
    {
        HPBar.value = HP;
        HPBar.maxValue = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageOrHeal(int damage)
    // If something needs to heal the player instead, use this function still but make the int variable passed a negative number
    {
        if (invincible==true)
        {

        }
        else
        {
            HP-= damage;
            StartCoroutine(iFrameTick());
        }
        HPBar.value=HP;
        if (HP<=0)
        {
            Destroy(gameObject);
            logicManager.gameObject.GetComponent<GameHandler>().GameOver();
        }
    }

    private IEnumerator iFrameTick()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f); 
        invincible=true;
        yield return new WaitForSeconds(0.5f);
        invincible=false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}
