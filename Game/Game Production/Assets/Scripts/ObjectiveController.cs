using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ObjectiveController : MonoBehaviour
{
    public TMP_Text enemyCountText;
    public float enemyCount;
    private string enemyCountString;

    public TMP_Text factoryText;

    public PlayerController playerController;

    private bool isDestroyedLocal = false;
    private bool isDead = false;

    public Canvas finsishedScreen;

    /// <summary>
    /// This method is called every frame and is constantly checking to see if conditions are met to change
    /// the objectives to green, to indicate they are completed. It also updates how many enemies are remaining.
    /// </summary>
    void Update()
    {
        enemyCount = HealthManager.Instance.ListLength();
        enemyCountString = enemyCount.ToString();
        enemyCountText.text = enemyCountString + " Remaining";

        if (enemyCount == 0)
        {
            enemyCountText.color = Color.green;
            isDead = true;
        }

        if (playerController.isDestroyed)
        {
            factoryText.color = Color.green;
            isDestroyedLocal = true;
        }

        if (isDead && isDestroyedLocal)
        {
            Debug.Log("You Win");
            finsishedScreen.enabled = true;
            Time.timeScale = 0.5f;
        }
    }
}
