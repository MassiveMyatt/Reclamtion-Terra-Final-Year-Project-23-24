using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;

    /// <summary>
    /// This method updates the health bar.
    /// </summary>
    /// <param name="maxHealth">The maximum health of the gameobject.</param>
    /// <param name="currentHealth">The minimum health of the gameobject.</param>
    public void updateHealthBar(float maxHealth, float currentHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
