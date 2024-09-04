using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public float remainingTime;
    public Canvas deathScreen;

    /// <summary>
    /// This method is called evry frame is is constantly updating the countdown timer.
    /// </summary>
    void Update()
    {
        if (remainingTime <= 0)
        {
            deathScreen.enabled = true;
            Time.timeScale = 0;
        }
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
