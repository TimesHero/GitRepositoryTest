using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
public class LevelUpButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject player;
    private EventSystem input;
    public GameObject firstButtonToSelect;
    public TextMeshProUGUI HPtext;
    public TextMeshProUGUI MPtext;
    public TextMeshProUGUI ATKtext;
    public TextMeshProUGUI DEFtext;
    public AudioClip sound;
    public AudioClip backSound;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        input = FindAnyObjectByType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInput()
    {
         input.SetSelectedGameObject(firstButtonToSelect);
    }

    public void HPUp(int increment)
    {
        PlayerHPManager playerHPManager = player.GetComponent<PlayerHPManager>();
        playerHPManager.HPLevel = Mathf.Clamp(playerHPManager.HPLevel + increment, 0, 5);
        Debug.Log(playerHPManager.HPLevel);
        playerHPManager.LevelUp();
        HPtext.text = "LVL " + playerHPManager.HPLevel + ":" + playerHPManager.HPMax + "HP";
        if (increment==1) AudioManager.Instance.PlaySound(sound); 
        else AudioManager.Instance.PlaySound(backSound); 
    }

    public void MPUp(int increment)
    {
        PlayerHPManager playerHPManager = player.GetComponent<PlayerHPManager>();
        playerHPManager.manaLevel = Mathf.Clamp(playerHPManager.manaLevel + increment, 0, 5);
        playerHPManager.LevelUp();
        MPtext.text = "LVL " + playerHPManager.manaLevel + ":" + playerHPManager.manaMax + "MP";
        if (increment==1) AudioManager.Instance.PlaySound(sound); 
        else AudioManager.Instance.PlaySound(backSound); 
    }

    public void ATKUp(int increment)
    {
        PlayerHPManager playerHPManager = player.GetComponent<PlayerHPManager>();
        playerHPManager.atkLevel = Mathf.Clamp(playerHPManager.atkLevel + increment, 0, 5);
        playerHPManager.LevelUp();
        ATKtext.text = "LVL " + playerHPManager.atkLevel + ":" + playerHPManager.damageMultiplier + "X DMG";
        if (increment==1) AudioManager.Instance.PlaySound(sound); 
        else AudioManager.Instance.PlaySound(backSound);  
    }

    public void DEFUp(int increment)
    {
        PlayerHPManager playerHPManager = player.GetComponent<PlayerHPManager>();
        playerHPManager.defLevel = Mathf.Clamp(playerHPManager.defLevel + increment, 0, 5);
        playerHPManager.LevelUp();
        DEFtext.text = "LVL " + playerHPManager.defLevel + ":" + playerHPManager.defValue + "X DEF";
        if (increment==1) AudioManager.Instance.PlaySound(sound); 
        else AudioManager.Instance.PlaySound(backSound); 
    }


}
