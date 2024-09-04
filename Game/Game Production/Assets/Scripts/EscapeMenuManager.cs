using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuManager : MonoBehaviour
{

    public Canvas menu;

    public Canvas help;

    /// <summary>
    /// This method reusumes the game but disabling any enabled UI.
    /// </summary>
    public void ResumeGame()
    {
        menu.enabled = false;
        help.enabled = false;
        Time.timeScale = 1;

    }

    /// <summary>
    /// This method loads the settings page.
    /// </summary>
    public void Settings()
    {
        menu.enabled = false;
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
    }

    /// <summary>
    /// This methid loads the main menu.
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);    
    }

    /// <summary>
    /// This method loads the help menu.
    /// </summary>
    public void Help()
    {
        help.enabled = true;
        menu.enabled = false;
    }

    /// <summary>
    /// This method restarts the game.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

}
