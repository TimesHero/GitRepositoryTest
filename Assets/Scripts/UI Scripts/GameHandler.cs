using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using TMPro;
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
    void Start()
    {
        input = FindAnyObjectByType<EventSystem>();
        Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        
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
        SceneManager.LoadScene("TestScene");
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
        endPanel.SetActive(true);
        input.SetSelectedGameObject(firstButtonToSelectGameOver);
        Time.timeScale = 0;
    }
}