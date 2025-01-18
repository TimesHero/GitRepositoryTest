using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using TMPro;
using Unity.VisualScripting;
public class MenuHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firstButtonToSelect;
    private EventSystem input;
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
        SceneManager.LoadScene("TestScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
