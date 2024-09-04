using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StamniaBarManager : MonoBehaviour
{
    public Image stamniaBar;
    /// <summary>
    /// This method updates the players stamina bar.
    /// </summary>
    /// <param name="maxStam">The maximum player stamina</param>
    /// <param name="currentStam">The current stamina of the player</param>
    public void UpdateStaminaBar(float maxStam, float currentStam)
    {
        stamniaBar.fillAmount = currentStam / maxStam;
    }
}
