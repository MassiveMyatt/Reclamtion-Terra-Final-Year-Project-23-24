using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// This method starts the game.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(4);
        Time.timeScale = 1;
    }


    /// <summary>
    /// This method loads the settings page.
    /// </summary>
    public void Settings()
    {
        SceneManager.LoadSceneAsync(2);
    }
    /// <summary>
    /// This method loads the main menu.
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    /// <summary>
    /// This method loads the help scren.
    /// </summary>
    public void Help()
    {
        SceneManager.LoadSceneAsync(5);
    }
    /// <summary>
    /// This method exits the user from the game.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }

}
