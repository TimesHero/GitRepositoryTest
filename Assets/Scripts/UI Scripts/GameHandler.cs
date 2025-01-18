using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using TMPro;
using Unity.VisualScripting;
public class GameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firstButtonToSelect;
    private EventSystem input;
    public GameObject pausePanel;
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
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        input.SetSelectedGameObject(firstButtonToSelect);
    }
}