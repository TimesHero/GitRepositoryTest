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
    public GameObject backButton;
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
    public void Controlpanel()
    {
        controlPanel.SetActive(true);
        input.SetSelectedGameObject(backButton);
    }
    public void back()
    {
        input.SetSelectedGameObject(controlButton);
        controlPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySound(backSound); 
    }
}
