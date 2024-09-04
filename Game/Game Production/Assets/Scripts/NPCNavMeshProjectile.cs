using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavMeshProjectile : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    public LayerMask whatIsPlayer;
    public LayerMask isGround;

    public Transform[] points;
    private int current;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;

    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    private Transform player;

    private bool hasAttacked = false;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float Damage;

    private float bulletTime;
    [SerializeField] private float timer;
    [SerializeField] private float bulletSpeed;
    public GameObject bullet;
    public Transform gun;
    public GameObject gunModel;

    public GameObject exploPos;
    public ParticleSystem explosion;

    public AudioController audioController;

    /// <summary>
    /// This method is called at the start of the game, before any update methods the first time. Getting the NavMeshAgent and
    /// the player's transform.
    /// </summary>
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Cyrus").transform;
        current = 0;
    }

    /// <summary>
    /// Update is called every frame. It acts as a finite state machine, checking if the player is in Sight or Attack range
    /// and then changing its state based upon that.
    /// </summary>
    private void FixedUpdate()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }

        //if (playerInSightRange && !playerInAttackRange)
        //{
        //    Chase();
        //}

        if (playerInAttackRange && playerInSightRange)
        {
            Attack();
        }
    }

    /// <summary>
    /// This method allows seeing the size of the ranges.
    /// </summary>
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, attackRange);
    }

    /// <summary>
    /// This methid defines the patrol state. The NPCs destination is set based upon a list of points, and we iterate through that
    /// list as the NPC reaches each point.
    /// </summary>
    private void Patrol()
    {
        gunModel.transform.localScale = Vector3.zero;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        navMeshAgent.speed = walkSpeed;

        if (transform.position.z != points[current].position.z)
        {
            navMeshAgent.SetDestination(points[current].position);
        }
        else
        {
            current = (current + 1) % points.Length;
            if (current >= points.Length)
            {
                current = 0;
            }
        }


    }

    /// <summary>
    /// This method sets the attack state, ensuring when the NPC is attacking they are facing the player and are not moving.
    /// Also establishes how much damage the NPC does to the player as well as how long each attack takes.
    /// </summary>
    private void Attack()
    {
        Vector3 gunScale = new(0.1333334f, 0.1333334f, 0.1333334f);
        gunModel.transform.localScale = gunScale;
        animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        transform.LookAt(player);
        //navMeshAgent.destination = movePositionTransform.position;
        navMeshAgent.speed = 0;
        bulletTime -= Time.deltaTime;
        if (bulletTime > 0)
        {
            return;
        }

        bulletTime = timer;

        if (!hasAttacked)
        {
            StartCoroutine(attack());
            hasAttacked = true;
            Invoke(nameof(AttackReset), timeBetweenAttacks);
        }
    }

    /// <summary>
    /// This method is a iterative method. It is only called when the enemy has attacked.
    /// Its function is to animate the enemy attack.
    /// </summary>
    /// <returns>Suspends the coroutine according to the given amount of seconds</returns>
    public IEnumerator attack()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
        animator.SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.6f);
        ParticleSystem ps = Instantiate(explosion, exploPos.transform.position, exploPos.transform.rotation);
        audioController.PlaySound(0);
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        Rigidbody rb = Instantiate(bullet, gun.transform.position, gun.transform.rotation).GetComponent<Rigidbody>();
        Vector3 direction = (player.transform.position - gun.transform.position).normalized;
        rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.77f);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
    }

    /// <summary>
    /// This method just resets the attack so the NPC can attack mulitple times.
    /// </summary>
    private void AttackReset()
    {
        hasAttacked = false;
    }
}
