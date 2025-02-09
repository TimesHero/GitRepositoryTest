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
    public float HP;
    public float HPMax=50;
    public float manaMax=100;
    public float damageMultiplier; 
    public float defValue; 
    public GameObject logicManager;
    public Slider HPBar;
    public Slider manaBar;
    public Slider comboBar;
    public bool invincible = false; 
    public float comboTimer=0;
    public int comboCount=0;
    public bool combo;
    public TextMeshProUGUI comboText;
    public int HPLevel;
    public int defLevel;
    public int atkLevel;
    public int manaLevel;
    AudioSource sound;
    void Start()
    {
        HPBar.value = HP;
        HPBar.maxValue = HP;
        sound = gameObject.GetComponent<AudioSource>();
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (combo==true && Time.time>0)
        {
            comboTimer-=0.2f;
            comboBar.value=comboTimer;
            if (comboTimer<=0)
            {
                comboCount=0;
                comboText.text = comboCount + "X";
                combo=false;
            }
        }
    }

    void LevelUp()
    {
        if (HPLevel == 0)
        {
            HPMax=50;
        }
        else if (HPLevel > 0 && HPLevel <= 5)
        {
            HPMax = 50 + (HPLevel * 10);  // Add 10 HP for each level up
            HP=HPMax;
        }
        if (HPLevel == 0)
        {
            manaMax=100;
        }
        else if (manaLevel > 0 && manaLevel <= 5)
        {
            manaMax = 100 + (manaLevel * 10);  // Add 10 HP for each level up
            gameObject.GetComponent<InputScript>().maxMana=manaMax;
            gameObject.GetComponent<InputScript>().mana=manaMax;
        }

        if (atkLevel == 0)
        {
            damageMultiplier = 1;
        }
        else if (atkLevel > 0 && atkLevel <= 5)
        {
             damageMultiplier = 1 + (atkLevel * 0.2f);  // 2x at max damage
        }

        if (defLevel == 0)
        {
            defValue = 1;
        }
        else if (defLevel > 0 && defLevel <= 5)
        {
             defValue = 1 + (defLevel * 0.1f);  // 2x at max damage
             Debug.Log(defValue);
        }

        HPBar.maxValue = HP;
        HPBar.value = HP;
        manaBar.maxValue = manaMax;
        manaBar.value = manaMax;
    }
    public void DamageOrHeal(float damage)
    // If something needs to heal the player instead, use this function still but make the int variable passed a negative number
    {
        if (invincible==true)
        {

        }
        else
        {
        float effectiveDamage = damage / defValue;
        effectiveDamage = Mathf.Max(0, effectiveDamage);
        HP -= effectiveDamage;
        if (effectiveDamage > 0)
        {
            sound.Play();
        }
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
        yield return new WaitForSeconds(0.1f);
        invincible=false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    public void ComboTrigger()
    {
        combo=true;
        comboTimer=120;
        comboCount++;
        comboText.text = comboCount + "X";
    }

}
