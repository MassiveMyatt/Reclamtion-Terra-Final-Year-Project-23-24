using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class NPCNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    public Animator animator;
    private Rigidbody rb;

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
    [SerializeField] private float timeBetweenBlockedAttacks;
    [SerializeField] private float Damage;

    public GameObject saber;

    private bool isBlocked;

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
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            Chase();
        }

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
        //Gizmos.DrawSphere(transform.position, sightRange);
    }

    /// <summary>
    /// This methid defines the patrol state. The NPCs destination is set based upon a list of points, and we iterate through that
    /// list as the NPC reaches each point.
    /// </summary>
    private void Patrol()
    {
        saber.transform.localScale = Vector3.zero;
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
    /// This method sets the chase state for the NPC, simply always following the player.
    /// </summary>
    private void Chase()
    {
        saber.transform.localScale = Vector3.zero;
        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        navMeshAgent.destination = movePositionTransform.position;
        navMeshAgent.speed = sprintSpeed;
    }

    /// <summary>
    /// This method sets the attack state, ensuring when the enemy is attacking they are facing the player and are not moving.
    /// Also establishes how much damage the enemy does to the player as well as how long each attack takes.
    /// </summary>
    private void Attack()
    {
        saber.transform.localScale = Vector3.one;
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        transform.LookAt(player);
        navMeshAgent.SetDestination(transform.position);

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
        AdjustLayerWeight(1, 1, 3);
        animator.SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.77f);
        AdjustLayerWeight(1, 0, 3);
    }

    /// <summary>
    /// This method just resets the attack so the NPC can attack mulitple times.
    /// </summary>
    private void AttackReset()
    {
        hasAttacked = false;
    }
    /// <summary>
    /// This method defines what should happen when the enemy's attack is blocked by the player.
    /// </summary>
    public void AttackBlocked()
    {
        StartCoroutine(IEBlock());
        hasAttacked = false;
        navMeshAgent.SetDestination(transform.position);
    }
    /// <summary>
    /// This is an iterative method that is only called when the enemy's attack has been blocked. It's purpose is to animate the enemy's
    /// stagger animation.
    /// </summary>
    /// <returns>suspends couroutine for a given amount of seconds.</returns>
    public IEnumerator IEBlock()
    {
        AdjustLayerWeight(1, 1, 3);
        animator.SetTrigger("isStaggered");
        yield return new WaitForSeconds(0.3f);
        AdjustLayerWeight(1, 0, 3);
    }
    /// <summary>
    /// This method allows for adjusting any given layer within an animator. This implementation is needed to ensure the stagger
    /// animation can play correctly.
    /// </summary>
    /// <param name="layerIndex">takes the index of the animator layer</param>
    /// <param name="targetWeight">takes the intended weight of the layer</param>
    /// <param name="duration">how long the adjustment should last for</param>
    /// <returns>Suspends the coroutine and returns null</returns>
    private IEnumerator AdjustLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        float time = 0;
        float initialWeight = animator.GetLayerWeight(layerIndex);
        while (time < duration)
        {
            time += Time.deltaTime;
            float weight = Mathf.Lerp(initialWeight, targetWeight, time / duration);
            animator.SetLayerWeight(layerIndex, weight);
            yield return null;
        }
        animator.SetLayerWeight(layerIndex, targetWeight);
    }
}
