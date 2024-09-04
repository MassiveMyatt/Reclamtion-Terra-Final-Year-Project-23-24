using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public HealthBarManager healthBar;
    public Canvas menu;

    private void Awake()
    {
        health = maxHealth;
    }

    /// <summary>
    /// This method is called whenever the player has damaged the enemy and takes away a set amount of health,
    /// and also updates the healthbar of the enemy.
    /// </summary>
    /// <param name="damage">Damage value establishes how much damage the enemy should take</param>
    public void TakeDamage(float damage)
    {
        health  -= damage;
        healthBar.updateHealthBar(maxHealth, health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This method is called whenever the enemy has damaged the player and takes away a set amount of health,
    /// and also updates the healthbar of the player.
    /// </summary>
    /// <param name="damage">Damage value establishes how much damage the player is taking.</param>
    public void TakeDamagePlayer(float damage)
    {
        health -= damage;
        healthBar.updateHealthBar(maxHealth, health);
        if (health <= 0)
        {
            menu.enabled = true;
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }


    public void GainHealth(float healthGain)
    {
        health += healthGain;
        healthBar.updateHealthBar(maxHealth, health);
    }

    public float getHealth()
    {
        return health;
    }

    private void OnEnable()
    {
        if (gameObject.tag == "Enemy")
        {
            HealthManager.Instance.RegisterEnemy(this);
        }
        
    }

    private void OnDisable()
    {
        HealthManager.Instance.UnregisterEnemy(this);
    }

}
