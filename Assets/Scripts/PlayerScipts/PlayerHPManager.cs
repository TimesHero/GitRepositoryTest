using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using TMPro;
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
    public Collider2D myCollider;

    void Start()
    {
        HPBar.value = HP;
        HPBar.maxValue = HP;
        LoadPlayerData();
        myCollider = GetComponent<Collider2D>();

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
    // Save player data, excluding HP and mana
public void SavePlayerData()
{
    PlayerPrefs.SetFloat("HopeFragments", hopeFragments); 
    PlayerPrefs.SetFloat("CurrentLevel", currentLevel);
    PlayerPrefs.SetFloat("Exp", exp); 
    PlayerPrefs.SetInt("HPLevel", HPLevel); 
    PlayerPrefs.SetInt("AtkLevel", atkLevel);
    PlayerPrefs.SetInt("DefLevel", defLevel); 
    PlayerPrefs.SetInt("ManaLevel", manaLevel); 
    PlayerPrefs.Save(); // Save the changes to PlayerPrefs
}

// Load player data, excluding HP and mana
public void LoadPlayerData()
{
    if (PlayerPrefs.HasKey("HopeFragments"))  
    {
        hopeFragments = PlayerPrefs.GetFloat("HopeFragments");
        currentLevel = PlayerPrefs.GetFloat("CurrentLevel");
        exp = PlayerPrefs.GetFloat("Exp");
        HPLevel = PlayerPrefs.GetInt("HPLevel");
        atkLevel = PlayerPrefs.GetInt("AtkLevel");
        defLevel = PlayerPrefs.GetInt("DefLevel");
        manaLevel = PlayerPrefs.GetInt("ManaLevel");
        print(HPLevel);
        print("LOAD");
    }
    else
    {
        // Default values if no saved data exists
        hopeFragments = 0;
        currentLevel = 1;
        exp = 0;
        HPLevel = 0;
        atkLevel = 0;
        defLevel = 0;
        manaLevel = 0;
        playerScore = 0;
    }
}

    public void LevelUp()
    {
        
        if (HPLevel == 0)//HPLVL--------------------------------------------------------------------------
        {
            HPMax=50;
        }
        else if (HPLevel > 0 && HPLevel <= 5)
        {
            HPMax = 50 + (HPLevel * 10);  // Add 10 HP for each level up
            HP=HPMax;
            print(HP+" HP");
            print(HPMax+" HPMAX");
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
             defValue = 1 + (defLevel * 0.2f);  // 2x at max damage
        }

        HPBar.maxValue = HPMax;
        HPBar.value = HPMax;
        manaBar.maxValue = manaMax;
        manaBar.value = manaMax;
        SavePlayerData();
    }
   public void DamageOrHeal(float damage)
{
    // If something needs to heal the player instead, use this function still but make the int variable passed a negative number
    if (invincible == true)
    {
        // Do nothing if the player is invincible
    }
    else
    {
        if (damage > 0)
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
        
        HP = Mathf.Clamp(HP, 0, HPMax);
        HPtext.text = "" + Mathf.Floor(HP) + "/" + Mathf.Floor(HPMax);
        StartCoroutine(iFrameTick());
    }
    HP = Mathf.Ceil(HP); 
    HPBar.value = HP;

    if (HP <= 0)
    {
        MakeDead();
        logicManager.gameObject.GetComponent<GameHandler>().GameOver(false);
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
        myCollider.enabled = false;
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
