using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{

    private string sceneName;

    /// <summary>
    /// This method is called when a scene with this class is first loaded. It is searching for any GameObject with
    /// the Menu Music tage in order to keep it playing through the menus.
    /// </summary>
    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Menu Music");

        if(musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// Update is called every frame and is constantly checking if the Intro scene has loaded, so that the main menu
    /// music can be destroyed and stopped.
    /// </summary>
    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;

        if (sceneName == "Intro")
        {
            Destroy(this.gameObject);
        }
    }

}
