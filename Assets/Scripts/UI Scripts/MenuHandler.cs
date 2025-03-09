using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firstButtonToSelect;
    private EventSystem input;
    public AudioClip sound;
    public AudioClip backSound;
    public GameObject controlPanel;
    public GameObject creditsPanel;
    public GameObject backButton;
    public GameObject backCreditButton;
    public GameObject controlButton;
    void Start()
    {
        input = FindAnyObjectByType<EventSystem>();
        input.SetSelectedGameObject(firstButtonToSelect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainLevelScene");
        AudioManager.Instance.PlaySound(sound); 
    }
    public void ControlPanel()
    {
        controlPanel.SetActive(true);
        input.SetSelectedGameObject(backButton);
    }
    public void CreditsPanel()
    {
        creditsPanel.SetActive(true);
        input.SetSelectedGameObject(backCreditButton);
    }
    public void back()
    {
        input.SetSelectedGameObject(controlButton);
        controlPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySound(backSound); 
    }
    public void survey()
    {
         Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdtUC22pmHLhEJ0rYwYYYOCQp1WK86lNOPKDX6inuEdd7s24Q/viewform?usp=dialog");
    }
}
