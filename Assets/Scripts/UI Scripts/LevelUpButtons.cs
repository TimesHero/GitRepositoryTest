using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
using Fungus;

public class LevelUpButtons : MonoBehaviour
{
    private GameObject player;
    private PlayerHPManager playerHPManager;
    private EventSystem input;

    public GameObject firstButtonToSelect;
    public TextMeshProUGUI HPtext;
    public TextMeshProUGUI MPtext;
    public TextMeshProUGUI ATKtext;
    public TextMeshProUGUI DEFtext;
    public TextMeshProUGUI fragmentText;
    public AudioClip sound;
    public AudioClip backSound;
    public Flowchart fungusFlowchart;
    public GameObject panel;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHPManager = player.GetComponent<PlayerHPManager>();
        input = FindAnyObjectByType<EventSystem>();
        fragmentText.text = "Hope Fragments: " + playerHPManager.hopeFragments;
        input.SetSelectedGameObject(firstButtonToSelect);
    }

    public void SetInput()
    {
        input.SetSelectedGameObject(firstButtonToSelect);
        UpdateUIStats();
    }

    public void StatHPUp() { StatChange(1, "HP"); }
    public void StatMPUp() { StatChange(1, "MP"); }
    public void StatATKUp() { StatChange(1, "ATK"); }
    public void StatDEFUp() { StatChange(1, "DEF"); }

    public void StatHPRefund() { StatChange(-1, "HP"); }
    public void StatMPRefund() { StatChange(-1, "MP"); }
    public void StatATKRefund() { StatChange(-1, "ATK"); }
    public void StatDEFRefund() { StatChange(-1, "DEF"); }

   public void StatChange(int increment, string statType)
{
    bool changeSuccess = false;

    if (increment != 0)
    {
        if (increment > 0 && playerHPManager.hopeFragments <= 0)
        {
            Debug.LogWarning("Not enough hope fragments.");
            return;
        }
        switch (statType)
        {
            case "HP":
                if (increment > 0 && playerHPManager.HPLevel < 5)
                {
                    if (playerHPManager.hopeFragments > 0) 
                    {
                        playerHPManager.HPLevel += increment;
                        playerHPManager.HPLevel = Mathf.Clamp(playerHPManager.HPLevel, 0, 5);
                        changeSuccess = true;
                    }
                }
                else if (increment < 0 && playerHPManager.HPLevel > 0)
                {
                    playerHPManager.HPLevel += increment;
                    playerHPManager.HPLevel = Mathf.Clamp(playerHPManager.HPLevel, 0, 5);
                    changeSuccess = true;
                }
                break;

            case "MP":
                if (increment > 0 && playerHPManager.manaLevel < 5)
                {
                    playerHPManager.manaLevel += increment;
                    playerHPManager.manaLevel = Mathf.Clamp(playerHPManager.manaLevel, 0, 5);
                    changeSuccess = true;
                }
                else if (increment < 0 && playerHPManager.manaLevel > 0)
                {
                    playerHPManager.manaLevel += increment;
                    playerHPManager.manaLevel = Mathf.Clamp(playerHPManager.manaLevel, 0, 5);
                    changeSuccess = true;
                }
                break;

            case "ATK":
                if (increment > 0 && playerHPManager.atkLevel < 5)
                {
                    playerHPManager.atkLevel += increment;
                    playerHPManager.atkLevel = Mathf.Clamp(playerHPManager.atkLevel, 0, 5);
                    changeSuccess = true;
                }
                else if (increment < 0 && playerHPManager.atkLevel > 0)
                {
                    playerHPManager.atkLevel += increment;
                    playerHPManager.atkLevel = Mathf.Clamp(playerHPManager.atkLevel, 0, 5);
                    changeSuccess = true;
                }
                break;

            case "DEF":
                if (increment > 0 && playerHPManager.defLevel < 5)
                {
                    playerHPManager.defLevel += increment;
                    playerHPManager.defLevel = Mathf.Clamp(playerHPManager.defLevel, 0, 5);
                    changeSuccess = true;
                }
                else if (increment < 0 && playerHPManager.defLevel > 0)
                {
                    playerHPManager.defLevel += increment;
                    playerHPManager.defLevel = Mathf.Clamp(playerHPManager.defLevel, 0, 5);
                    changeSuccess = true;
                }
                break;
        }

        if (changeSuccess)
        {
            if (increment > 0)
            {
                playerHPManager.hopeFragments -= 1; 
                PlaySound(increment); 
            }
            else
            {
                // Refund the fragment for refunding the stat
                playerHPManager.hopeFragments += 1; 
                PlaySound(increment);
            }

            playerHPManager.LevelUp();
            UpdateUIStats();
        }
        else
        {
            Debug.LogWarning($"Stat change for {statType} failed.");
        }
    }
}

    private void UpdateUIStats()
    {
        HPtext.text = $"LVL {playerHPManager.HPLevel}:{playerHPManager.HPMax} HP";
        MPtext.text = $"LVL {playerHPManager.manaLevel}:{playerHPManager.manaMax} MP";
        ATKtext.text = $"LVL {playerHPManager.atkLevel}:{playerHPManager.damageMultiplier}X DMG";
        DEFtext.text = $"LVL {playerHPManager.defLevel}:{playerHPManager.defValue}X DEF";
        fragmentText.text = "Hope Fragments: " + playerHPManager.hopeFragments;
    }

    public void Exit()
    {
        fungusFlowchart.ExecuteBlock("EndLevelUp");
        panel.SetActive(false);
    }

    void PlaySound(int increment)
    {
        if (increment > 0)
            AudioManager.Instance.PlaySound(sound); 
        else
            AudioManager.Instance.PlaySound(backSound); 

        fragmentText.text = "Hope Fragments: " + playerHPManager.hopeFragments;
    }
}
