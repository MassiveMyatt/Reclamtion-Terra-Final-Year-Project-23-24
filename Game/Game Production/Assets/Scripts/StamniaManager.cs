using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StamniaManager : MonoBehaviour
{

    public float maxStamina;
    public float currentStamina;
    public StamniaBarManager staminaManager;

    /// <summary>
    /// Method is called when the scene loads and sets the stamina bar to the max player stamina, set in the inspector.
    /// </summary>
    private void Awake()
    {
        currentStamina = maxStamina;
    }
    /// <summary>
    /// Method to remove stamina from the player.
    /// </summary>
    /// <param name="Stamina">Amount of stamina to be removed.</param>
    public void TakeStamina(float Stamina)
    {
        if (!(currentStamina <= 0))
        {
            currentStamina -= Stamina;
            staminaManager.UpdateStaminaBar(maxStamina, currentStamina);
        }

    }
    /// <summary>
    /// Method to addd stamina to the players stamina bar.
    /// </summary>
    /// <param name="AddedStamina">The amount of stamina to be added.</param>
    public void AddStamina(float AddedStamina)
    {
        if (currentStamina <= 100)
        {
            currentStamina += AddedStamina;
            staminaManager.UpdateStaminaBar(maxStamina, currentStamina);
        }

    }
    /// <summary>
    /// Returns the amount of stamina the player currently has
    /// </summary>
    /// <returns>current stamina value.</returns>
    public float GetStamina()
    {
        return currentStamina;
    }
}
