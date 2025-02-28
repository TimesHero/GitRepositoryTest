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
            return;
        }

        if (increment < 0)
        {
            switch (statType)
            {
                case "HP":
                    if (playerHPManager.HPLevel <= 0) return;
                    break;
                case "MP":
                    if (playerHPManager.manaLevel <= 0) return;
                    break;
                case "ATK":
                    if (playerHPManager.atkLevel <= 0) return;
                    break;
                case "DEF":
                    if (playerHPManager.defLevel <= 0) return;
                    break;
            }
        }

        // Perform stat change
        switch (statType)
        {
            case "HP":
                if (increment > 0 && playerHPManager.HPLevel < 5)
                {
                    playerHPManager.HPLevel = Mathf.Clamp(playerHPManager.HPLevel + increment, 0, 5);
                    HPtext.text = $"LVL {playerHPManager.HPLevel}:{playerHPManager.HPMax} HP";
                    changeSuccess = true;
                }
                else if (increment < 0 && playerHPManager.HPLevel > 0)
                {
                    playerHPManager.HPLevel = Mathf.Clamp(playerHPManager.HPLevel + increment, 0, 5);
                    HPtext.text = $"LVL {playerHPManager.HPLevel}:{playerHPManager.HPMax} HP";
                    changeSuccess = true;
                }
                break;

            case "MP":
                if (increment > 0 && playerHPManager.manaLevel < 5)
                {
                    playerHPManager.manaLevel = Mathf.Clamp(playerHPManager.manaLevel + increment, 0, 5);
                    MPtext.text = $"LVL {playerHPManager.manaLevel}:{playerHPManager.manaMax} MP";
                    changeSuccess = true;
                }
                else if (increment < 0 && playerHPManager.manaLevel > 0)
                {
                    playerHPManager.manaLevel = Mathf.Clamp(playerHPManager.manaLevel + increment, 0, 5);
                    MPtext.text = $"LVL {playerHPManager.manaLevel}:{playerHPManager.manaMax} MP";
                    changeSuccess = true;
                }
                break;

            case "ATK":
                if (increment > 0 && playerHPManager.atkLevel < 5)
                {
                    playerHPManager.atkLevel = Mathf.Clamp(playerHPManager.atkLevel + increment, 0, 5);
                    ATKtext.text = $"LVL {playerHPManager.atkLevel}:{playerHPManager.damageMultiplier}X DMG";
                    changeSuccess = true;
                }
                else if (increment < 0 && playerHPManager.atkLevel > 0)
                {
                    playerHPManager.atkLevel = Mathf.Clamp(playerHPManager.atkLevel + increment, 0, 5);
                    ATKtext.text = $"LVL {playerHPManager.atkLevel}:{playerHPManager.damageMultiplier}X DMG";
                    changeSuccess = true;
                }
                break;

            case "DEF":
                if (increment > 0 && playerHPManager.defLevel < 5)
                {
                    playerHPManager.defLevel = Mathf.Clamp(playerHPManager.defLevel + increment, 0, 5);
                    DEFtext.text = $"LVL {playerHPManager.defLevel}:{playerHPManager.defValue}X DEF";
                    changeSuccess = true;
                }
                else if (increment < 0 && playerHPManager.defLevel > 0)
                {
                    playerHPManager.defLevel = Mathf.Clamp(playerHPManager.defLevel + increment, 0, 5);
                    DEFtext.text = $"LVL {playerHPManager.defLevel}:{playerHPManager.defValue}X DEF";
                    changeSuccess = true;
                }
                break;
        }
        if (changeSuccess)
        {
            if (increment > 0) // Level-up (fragments deducted)
            {
                playerHPManager.hopeFragments -= 1;
            }
            else if (increment < 0) // Refund (fragments added)
            {
                playerHPManager.hopeFragments += 1;
            }

            playerHPManager.LevelUp(); 
            PlaySound(increment);
        }
    }
}

    public void Exit()
    {
        fungusFlowchart.ExecuteBlock("EndLevelUp");
        panel.SetActive(false);
    }

    void PlaySound(int increment)
    {
        if (increment > 0)
            AudioManager.Instance.PlaySound(sound); // Level-up sound
        else
            AudioManager.Instance.PlaySound(backSound); // Refund sound

        fragmentText.text = "Hope Fragments: " + playerHPManager.hopeFragments;
    }
}
