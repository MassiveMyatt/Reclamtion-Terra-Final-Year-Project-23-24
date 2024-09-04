using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberController : MonoBehaviour
{
    public float timeBetweenAttacks;
    public float damageDealt;
    public ParticleSystem attack;
    public GameObject hitPos;

    private float enemyHealth;
    public DamageManager playerDamageManager;
    public float healthGained;

    public AudioController audioController;
    /// <summary>
    /// Defines what should happen when the players' saber colliders with a given collider
    /// </summary>
    /// <param name="col">Collider the player is colliding with.</param>
    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.CompareTag("Enemy"))
        {
            DamageManager damage = col.GetComponent<DamageManager>();
            enemyHealth = damage.getHealth();
            ParticleSystem ps = Instantiate(attack, hitPos.transform.position, hitPos.transform.rotation);
            audioController.PlaySound(1);
            damage.TakeDamage(damageDealt);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            Debug.Log(enemyHealth);
            if (enemyHealth <= 25)
            {
                playerDamageManager.GainHealth(healthGained);
            }
        }

    }

}
