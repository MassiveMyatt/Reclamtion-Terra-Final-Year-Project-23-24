using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPauseMenuManager : MonoBehaviour
{
    /// <summary>
    /// This method resumes the game
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(3);
    }
}
