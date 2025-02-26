/*******************************************************************************************
    Visual Novel, GDDV107 - Alexander "AJ" Fraser, Lisa Zhu, Jyden Briones

    The purpose of this script is to manage the main menu, and the actions that happen when 
    you press the buttons.

    Methods:
    StartGame - Loads the main game scene.
    OpenHowToPlay - Opens the tutorial panel.
    CloseHowToPlay - Closes tutorial panel, returns back to main menu.
    OpenSettings - Opens the settings panel.
    CloseSettings - Closes settings panel, returns back to main menu.
    OpenCredits - Opens the credits panel.
    CloseCredits - Closes credits panel, returns back to main menu.
    QuitGame - Closes application
********************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject howToPlayPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    //Start Game
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Tutorial Instructions
    public void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    //Game Settings
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    //Credits
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    //Quit
    public void QuitGame()
    {
        Application.Quit();
    }
}
