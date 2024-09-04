using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    private List<DamageManager> enemies = new List<DamageManager>();

    /// <summary>
    /// This method is called when the scene loads and ensure's the gameobject isnt destroyed between scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// This method adds an enemy to the list of enemies.
    /// </summary>
    /// <param name="enemy">The enemies DamageManager component</param>
    public void RegisterEnemy(DamageManager enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
    /// <summary>
    /// This method removes an enemy from the list of enemies.
    /// </summary>
    /// <param name="enemy">The enemies DamageManager componen</param>
    public void UnregisterEnemy(DamageManager enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    /// <summary>
    /// This method returns the current length of the enemy list.
    /// </summary>
    /// <returns>the number of enemies in the list.</returns>
    public float ListLength()
    {
        return enemies.Count();
    }
}
