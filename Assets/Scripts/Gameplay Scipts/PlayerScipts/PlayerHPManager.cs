using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using TMPro;
public class PlayerHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP;
    public GameObject logicManager;
    public Slider HPBar;
    public Slider comboBar;
    public bool invincible = false; 
    public float comboTimer=0;
    public int comboCount=0;
    public bool combo;
    public TextMeshProUGUI comboText;
    void Start()
    {
        HPBar.value = HP;
        HPBar.maxValue = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (combo==true)
        {
            comboTimer-=0.2f;
            comboBar.value=comboTimer;
            if (comboTimer<=0)
            {
                print("comboEnd");
                comboCount=0;
                comboText.text = comboCount + "X";
                combo=false;
            }
        }
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
            MakeDead();
            logicManager.gameObject.GetComponent<GameHandler>().GameOver();
        }
    }
    public void MakeDead()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }
    private IEnumerator iFrameTick()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f); 
        invincible=true;
        yield return new WaitForSeconds(0.5f);
        invincible=false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    public void ComboTrigger()
    {
        combo=true;
        comboTimer=120;
        comboCount++;
        print(comboCount);
        comboText.text = comboCount + "X";
    }

}
