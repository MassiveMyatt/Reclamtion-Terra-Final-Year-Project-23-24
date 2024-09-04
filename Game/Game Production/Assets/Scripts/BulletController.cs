using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletDamage;

    public ParticleSystem hit;
    public ParticleSystem blocked;
    private Transform hitPos;

    /// <summary>
    /// This method defines what the bullet, shot from the enemies gun, should do when hitting the player
    /// whilst he is either blocking or not blocking.
    /// </summary>
    /// <param name="col">Name of the collider the bullet is colliding with.</param>
    private void OnTriggerEnter(Collider col)
    {
        hitPos = col.transform.Find("HitPosGunPoint");
        if (col.gameObject.CompareTag("Player"))
        {
            DamageManager damage = col.GetComponent<DamageManager>();
            ParticleSystem ps = Instantiate(hit, hitPos.transform.position, hitPos.transform.rotation);
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);

            damage.TakeDamagePlayer(bulletDamage);
            Invoke(nameof(DestroyBullet), 1.2f);
        }
        if (col.gameObject.CompareTag("PlayerBlocked"))
        {
            DestroyBullet();
        }
    }

    /// <summary>
    /// Method which destroys the bullet GameObject instance.
    /// </summary>
    private void DestroyBullet()
    {
        Destroy(gameObject);

    }
}
