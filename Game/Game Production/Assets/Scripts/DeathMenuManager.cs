using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuManager : MonoBehaviour
{
    /// <summary>
    /// This method loads the home screen.
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    /// <summary>
    /// This method restarts the game level.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
