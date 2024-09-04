using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScript : MonoBehaviour
{
    /// <summary>
    /// This method loads the main game scene.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.LoadSceneAsync(1);
    }

}
