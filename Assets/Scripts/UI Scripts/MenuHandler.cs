using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firstButtonToSelect;
    public GameObject firstPlayButtonToSelect;
    private EventSystem input;
    public AudioClip sound;
    public AudioClip backSound;
    public GameObject controlPanel;
    public GameObject creditsPanel;
    public GameObject backButton;
    public GameObject backCreditButton;
    public GameObject controlButton;
    public GameObject buttonLayout; 
    public GameObject playButtonLayout;
    void Start()
    {
        input = FindAnyObjectByType<EventSystem>();
        input.SetSelectedGameObject(firstButtonToSelect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        buttonLayout.SetActive(false);
        playButtonLayout.SetActive(true);
        input.SetSelectedGameObject(firstPlayButtonToSelect);
        AudioManager.Instance.PlaySound(sound); 
    }
    public void Back()
    {
        buttonLayout.SetActive(true);
        playButtonLayout.SetActive(false);
        input.SetSelectedGameObject(firstButtonToSelect);
        AudioManager.Instance.PlaySound(backSound); 
    }

    public void StartGame()
    {
        if (PlayerPrefs.GetInt("tutorialComplete?")==1)
        {
            SceneManager.LoadScene("MainLevelScene");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
        AudioManager.Instance.PlaySound(sound); 
    }
    public void Skip()
    {
        SceneManager.LoadScene("MainLevelScene");
        AudioManager.Instance.PlaySound(sound); 
    }
    public void ControlPanel()
    {
        controlPanel.SetActive(true);
        buttonLayout.SetActive(false);
        input.SetSelectedGameObject(backButton);
        AudioManager.Instance.PlaySound(sound); 
        
    }
    public void CreditsPanel()
    {
        creditsPanel.SetActive(true);
        buttonLayout.SetActive(false);
        input.SetSelectedGameObject(backCreditButton);
        AudioManager.Instance.PlaySound(sound); 
        AudioManager.Instance.PlaySound(backSound); 
    }
    public void back()
    {
        input.SetSelectedGameObject(controlButton);
        buttonLayout.SetActive(true);
        controlPanel.SetActive(false);
        creditsPanel.SetActive(false);
        AudioManager.Instance.PlaySound(backSound); 
    }
    
    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySound(backSound); 
    }
    public void survey()
    {
         //Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeStvDK9Qr6ouIKV7U9W95sZcq5y_0v9wSuHiePmb0iLoJLSA/viewform?usp=sharing");
         PlayerPrefs.DeleteAll();
         AudioManager.Instance.PlaySound(sound); 
    }
    public void BetaSurvery()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeStvDK9Qr6ouIKV7U9W95sZcq5y_0v9wSuHiePmb0iLoJLSA/viewform?usp=sharing");
    }
}
