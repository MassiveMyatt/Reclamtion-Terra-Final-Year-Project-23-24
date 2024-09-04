using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SaberControllerEnemy : MonoBehaviour
{
    public float timeBetweenAttacks;
    public float damageDealt;
    public ParticleSystem attack;
    public GameObject hitPos;
    public ParticleSystem blocked;
    public GameObject blockPos;

    public bool isBlocked = false;
    public NPCNavMesh enemyController;

    public AudioController audioController;
    /// <summary>
    /// Defines what should happen when the enemy's saber colliders with a given collider
    /// </summary>
    /// <param name="col">Collider the enemy is colliding with.</param>
    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            DamageManager damage = col.GetComponent<DamageManager>();
            damage.TakeDamagePlayer(damageDealt);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Invoke(nameof(AttackReset), timeBetweenAttacks);
            ParticleSystem ps = Instantiate(attack, hitPos.transform.position, hitPos.transform.rotation);
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        if (col.gameObject.CompareTag("PlayerBlocked"))
        {
            ParticleSystem ps = Instantiate(blocked, blockPos.transform.position, blockPos.transform.rotation);
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            audioController.PlaySound(0);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Invoke(nameof(AttackReset), timeBetweenAttacks);
            enemyController.AttackBlocked();
        }
    }

    /// <summary>
    /// This method resets attacks.
    /// </summary>
    void AttackReset()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}

