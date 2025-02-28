using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using TMPro;
using System.Diagnostics;
using Fungus;
using UnityEngine.Rendering;
public class PlayerHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float HP;
    public float HPMax=50;
    public float mana;
    public float manaMax=100;
    public float hopeFragments;
    public float currentLevel; 
    public float exp;
    public float[] expNeededForLevel; 
    public float playerScore; 
    public float damageMultiplier; 
    public float defValue; 
    public GameObject logicManager;
    public Slider HPBar;
    public Slider manaBar;
    public Slider comboBar;
    public bool invincible = false; 
    public float comboTimer=0;
    public int comboCount=0;
    public float highestCombo;
    public float killCount;
    public bool combo;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    public int HPLevel;
    public int defLevel;
    public int atkLevel;
    public int manaLevel;
    float effectiveDamage;
    private float scoreMultipler = 1; 
    public TextMeshProUGUI HPtext;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI scoreText;
    public AudioClip dmgSound; 
    private float baseEnemyScore; 
    void Start()
    {
        HPBar.value = HP;
        HPBar.maxValue = HP;
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
                comboText.text = comboCount + " Combo";
                combo=false;
                scoreMultipler = 1; 
                multiplierText.text ="1.0x Score";
            }
        }
    }

    public void LevelUp()
    {
        if (hopeFragments>0)
        
        if (HPLevel == 0)//HPLVL--------------------------------------------------------------------------
        {
            HPMax=50;
        }
        else if (HPLevel > 0 && HPLevel <= 5)
        {
            HPMax = 50 + (HPLevel * 10);  // Add 10 HP for each level up
            HP=HPMax;
        }

        HP=HPMax;
        HPtext.text = "" + HP + "/" + HPMax;

        if (manaLevel == 0)//MANALVL--------------------------------------------------------------------------
        {
            manaMax=100;
        }
        else if (manaLevel > 0 && manaLevel <= 5)
        {
            manaMax = 100 + (manaLevel * 10);
        }

        mana = manaMax;
        manaText.text = "" + mana + "/" + manaMax;

        if (atkLevel == 0)//ATKLVL--------------------------------------------------------------------------
        {
            damageMultiplier = 1;
        }
        else if (atkLevel > 0 && atkLevel <= 5)
        {
             damageMultiplier = 1 + (atkLevel * 0.2f);  // 2x at max damage
        }

        if (defLevel == 0)//DEFLVL--------------------------------------------------------------------------
        {
            defValue = 1;
        }
        else if (defLevel > 0 && defLevel <= 5)
        {
             defValue = 1 + (defLevel * 0.1f);  // 2x at max damage
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
        if (damage>0)
        {
            float effectiveDamage = damage / defValue;
            effectiveDamage = Mathf.Max(0, effectiveDamage);
            HP -= effectiveDamage;
            AudioManager.Instance.PlaySound(dmgSound);
        }
        else
        {
            HP -= damage;
        }
        HPtext.text = "" + HP + "/" + HPMax;
        if (effectiveDamage > 0)
        {
        }
            StartCoroutine(iFrameTick());
        }
        HP = Mathf.Floor(HP);
        HPBar.value=HP;
        if (HP<=0)
        {
            MakeDead();
            logicManager.gameObject.GetComponent<GameHandler>().GameOver();
        }
    }
    public void UseMana(float consumption)
    {
        mana-=consumption;
        if(mana>manaMax)
        {
            mana=manaMax;
        }
        if(mana<0)
        {
            mana=0;
        }
        manaBar.value=mana;
        manaText.text = mana.ToString("0") + "/" + manaMax.ToString("0");
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
    public void ComboTrigger(float scoreFromEnemy)
    {
        combo=true;
        comboTimer=120;
        comboCount++;
        if (comboCount>highestCombo)
        {
            highestCombo=comboCount;
        }
        killCount++;
        comboText.text = comboCount + " Combo";
        mana+=3;
        manaBar.value = mana;

         // Check if the combo counter has reached a multiple of 10
        if (comboCount % 10 == 0)
        {
            scoreMultipler += 0.1f;
            multiplierText.text = scoreMultipler +"x Score";
            print(scoreMultipler);
        }
        //------SCORE INCREASE------
        //baseEnemyScore = scoreFromEnemy; 
        scoreFromEnemy = scoreFromEnemy * scoreMultipler; 

        playerScore += scoreFromEnemy; 
        playerScore = Mathf.Floor(playerScore);
        scoreText.text = "Score: " + playerScore; 
        

    }

}
