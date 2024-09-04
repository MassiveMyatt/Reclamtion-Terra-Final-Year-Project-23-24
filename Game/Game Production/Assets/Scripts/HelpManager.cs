using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpManager : MonoBehaviour
{
    /// <summary>
    /// This method loads the main menu.
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
