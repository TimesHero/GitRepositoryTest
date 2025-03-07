using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
public class GameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firstButtonToSelect;
    public GameObject firstButtonToSelectGameOver;
    private EventSystem input;
    public GameObject pausePanel;
    public GameObject endPanel;
    public AudioClip sound;
    public AudioClip backSound;
    public Slider expBar;
    public TextMeshProUGUI lvlText;
    public TextMeshProUGUI fragmentText;
    public TextMeshProUGUI highestCombo;
    public TextMeshProUGUI killCount;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI endScoreText;
    private GameObject player;
    private bool gameEnded = false; 
    private float expIncreaseValue = 0; 
    private InputActionMap playerActionMap;
    public InputActionAsset inputActions;
    void Start()
    {
        input = FindAnyObjectByType<EventSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerActionMap = inputActions.FindActionMap("Player");
        Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameEnded==true&&expIncreaseValue!=0)
        {
            
            expBar.value ++;
            Debug.Log(expBar.value);
            if (expBar.value==expBar.maxValue)
            {
                player.GetComponent<PlayerHPManager>().currentLevel++; 
                player.GetComponent<PlayerHPManager>().hopeFragments++; 
                lvlText.text = "Player Level: " + player.GetComponent<PlayerHPManager>().currentLevel; 
                fragmentText.text = "Hope Fragments: " + player.GetComponent<PlayerHPManager>().hopeFragments; 
                expBar.maxValue = player.GetComponent<PlayerHPManager>().expNeededForLevel[(int)player.GetComponent<PlayerHPManager>().currentLevel-1];
                expBar.value=0; 
            }
            expText.text = expBar.value + "/" + player.GetComponent<PlayerHPManager>().expNeededForLevel[(int)player.GetComponent<PlayerHPManager>().currentLevel-1];
            expIncreaseValue--;
            
        }
        if (gameEnded==true&&expIncreaseValue==0)
        {
            player.GetComponent<PlayerHPManager>().exp=expBar.value;
            player.GetComponent<PlayerHPManager>().SavePlayerData();
        }
    }
    public void Unpause()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        AudioManager.Instance.PlaySound(sound); 
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance.PlaySound(backSound); 
    }
    public void Retry()
    {
        SceneManager.LoadScene("MainLevelScene");
        AudioManager.Instance.PlaySound(sound); 

    }
    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySound(backSound); 
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        input.SetSelectedGameObject(firstButtonToSelect);
        AudioManager.Instance.PlaySound(sound); 
    }

    public void GameOver()
    {
        if (gameEnded==false)
        {
            endPanel.SetActive(true);
            input.SetSelectedGameObject(firstButtonToSelectGameOver);

            expBar.value = player.GetComponent<PlayerHPManager>().exp;
            print(player.GetComponent<PlayerHPManager>().exp);
            expBar.maxValue = player.GetComponent<PlayerHPManager>().expNeededForLevel[(int)player.GetComponent<PlayerHPManager>().currentLevel-1];
            expIncreaseValue = player.GetComponent<PlayerHPManager>().playerScore;
            expIncreaseValue = Mathf.Floor(expIncreaseValue);

            endScoreText.text = "Your Score: " + player.GetComponent<PlayerHPManager>().playerScore; 
            lvlText.text = "Player Level: " + player.GetComponent<PlayerHPManager>().currentLevel; 
            fragmentText.text = "Hope Fragments: " + player.GetComponent<PlayerHPManager>().hopeFragments; 
            killCount.text = "Enemies Killed: " + player.GetComponent<PlayerHPManager>().killCount; 
            highestCombo.text = "Highest Combo: " + player.GetComponent<PlayerHPManager>().highestCombo; 
            expText.text = player.GetComponent<PlayerHPManager>().exp + "/" + player.GetComponent<PlayerHPManager>().expNeededForLevel[(int)player.GetComponent<PlayerHPManager>().currentLevel-1];
            player.GetComponent<Collider2D>().enabled = false;
            //Time.timeScale = 0;
            playerActionMap.Disable();
            StartCoroutine(SetGameEnd());
        }
    }

    private IEnumerator SetGameEnd()
    {
        yield return new WaitForSeconds(0.5f);
        gameEnded=true; 
    }
}