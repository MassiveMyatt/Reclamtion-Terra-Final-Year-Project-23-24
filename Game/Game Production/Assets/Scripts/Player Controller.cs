using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float speedPostWall;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float gravityScale;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float rotationRate;
    [SerializeField] private float dodgeDistance;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float timeBetweenDodge;
    [SerializeField] private float timeBetweenBlock;
    [SerializeField] private float blockAmount;
    [SerializeField] private float addedBackStamina;
    [SerializeField] private float interactRange;

    private bool isGrounded;
    private bool isMoving;
    private bool isSprint;
    private bool isAttacking;
    private bool isBlocking;
    private bool hasAttacked = false;
    private bool hasDodge = false;
    private bool hasBlocked = false;
    private bool canBlock;
    private bool hasPressed;

    private Vector2 move;
    private Vector3 movement;

    public Rigidbody rb;
    Animator animator;
    public GameObject saber;
    public GameObject blockBox;

    public StamniaManager stamina;
    private float currentStam;

    public Canvas menu;

    public DamageManager damage;

    private bool playerInInteractRange;
    public LayerMask whatIsFactory;
    public GameObject interactButton;

    private bool inRange = false;
    public ParticleSystem explosion;
    public GameObject exploPos;
    public bool isDestroyed = false;
    private AudioController audioController;

    /// <summary>
    /// This method is called when the scene loads and gets the specified components, sets them to a variable.s
    /// </summary>
    private void Awake()
    {
        TryGetComponent(out rb);
        animator = GetComponent<Animator>();
        audioController = GetComponent<AudioController>();

    }

    /// <summary>
    /// Method reads the 2D Vector of the player's position, to allow for the player to move.
    /// </summary>
    /// <param name="context">refers to an input action, such as pressing W,defined in the Input Manager.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        saber.transform.localScale = Vector3.zero;
        move = context.ReadValue<Vector2>();

        if (isSprint)
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0.5f);
        }
    }
    /// <summary>
    /// Method checks if player is grounded and if they are, AddForce causes the player to jump.
    /// </summary>
    /// <param name="context">refers to an input action, such as pressing SPACE,defined in the Input Manager.</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }
    /// <summary>
    /// Method defines what to do when the user wants to sprint. It sets the new animation for sprinting as well
    /// as the new sprint speed.
    /// </summary>
    /// <param name="context">refers to an input action such as the key press of SHIFT.</param>
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprint = context.ReadValueAsButton();
        animator.SetFloat("Speed", 1f);
        saber.transform.localScale = Vector3.zero;
        if (isSprint)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = speedPostWall;
            animator.SetFloat("Speed", 0.5f);
        }

    }
    /// <summary>
    /// Method defines what should happen when the user wants to attack. It triggers the attack animation and calls a 
    /// coroutine to perform that animation. Also enables the saber.
    /// </summary>
    /// <param name="context">refers to an input action, defined in input manager.</param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton();
        saber.transform.localScale = Vector3.one;
        if (!hasAttacked)
        {
            hasAttacked = true;
            Invoke(nameof(AttackReset), timeBetweenAttacks);
            StartCoroutine(Attack());
            audioController.PlaySound(0);
        }

    }
    /// <summary>
    /// Coroutine that is called whilst the !hasAttacked. Makes the attack animation be able to trigger and play
    /// at the same time as the running/walking animation.
    /// </summary>
    /// <returns>Suspends the coroutine for a given amount of time</returns>
    public IEnumerator Attack()
    {
        ColliderEnable();
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
        animator.SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.77f);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
        ColliderDisable();
    }
    /// <summary>
    /// Method to reset attacks.
    /// </summary>
    private void AttackReset()
    {
        hasAttacked = false;
    }
    /// <summary>
    /// Method to enable the saber collider.
    /// </summary>
    private void ColliderEnable()
    {
        saber.GetComponent<CapsuleCollider>().enabled = true;
    }
    /// <summary>
    /// Method to disable the saber collider.
    /// </summary>
    private void ColliderDisable()
    {
        saber.GetComponent<CapsuleCollider>().enabled = false;

    }
    /// <summary>
    /// Method that defines what should happen when the user wants to dodge. Adds a relative force to the player's
    /// rigid body compoent
    /// </summary>
    /// <param name="context">refers to an input action defined in input manager.</param>
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!hasDodge)
        {
            hasDodge = true;
            rb.AddRelativeForce(Vector3.forward * dodgeDistance, ForceMode.Impulse);
            Invoke(nameof(DodgeReset), timeBetweenDodge);
        }

    }
    /// <summary>
    /// Method to reset dodging.
    /// </summary>
    private void DodgeReset()
    {
        hasDodge = false;
    }
    /// <summary>
    /// This method defines what should happen when the user wants to block. It triggers the blocking animation if the 
    /// player has stamina.
    /// </summary>
    /// <param name="context">refers to an input action defined in input manager.</param>
    public void OnBlock(InputAction.CallbackContext context)
    {
        if (canBlock)
        {
            stamina.TakeStamina(blockAmount);
            isBlocking = context.ReadValueAsButton();
            animator.SetBool("isBlocking", isBlocking);
            saber.transform.localScale = Vector3.one;
            animator.SetLayerWeight(animator.GetLayerIndex("Block"), 1);
            audioController.PlaySound(2);
        }

        if (!hasBlocked)
        {
            hasBlocked = true;
            Invoke(nameof(BlockReset), timeBetweenBlock);
            animator.SetLayerWeight(animator.GetLayerIndex("Block"), 0);


        }

    }
    /// <summary>
    /// Method to reset blocking.
    /// </summary>
    private void BlockReset()
    {
        hasBlocked = false;

    }

    /// <summary>
    /// Method to disable block when player has no stamina.
    /// </summary>
    private void DisableBlock()
    {
        canBlock = false;
        animator.SetLayerWeight(animator.GetLayerIndex("Block"), 0);
    }
    /// <summary>
    /// Method to enable stamina.
    /// </summary>
    private void EnableBlock()
    {
        canBlock = true;
    }
    /// <summary>
    /// Method defines what shuld happen when the user wants to pause.
    /// </summary>
    /// <param name="context">refers to an input action defined in the input manager</param>
    public void OnPause(InputAction.CallbackContext context)
    {
        menu.enabled = true;
        Time.timeScale = 0;

    }
    /// <summary>
    /// Method defines what should happen when the user interacts with a gameobject, in this case the factory
    /// that should be destroyed. 
    /// </summary>
    /// <param name="context">refers to an input action given in input manager.</param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (inRange && !isDestroyed)
        {
            ParticleSystem ps = Instantiate(explosion, exploPos.transform.position, exploPos.transform.rotation);
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            isDestroyed = true;
        }
    }

    /// <summary>
    /// Update is called every frame, creating a new vector based on the players posoition given in OnMove
    /// Also checking if user is in range of an interactable object, displaying a prompt if so.
    /// </summary>
    private void Update()
    {
        movement = new Vector3(move.x, 0f, move.y);
        CheckIfMove();
        if (!isMoving)
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        }

        playerInInteractRange = Physics.CheckSphere(transform.position, interactRange, whatIsFactory);
        
        if (playerInInteractRange && !isDestroyed)
        {
            interactButton.SetActive(true);
            inRange = true;
        }
        else
        {
            interactButton.SetActive(false);
            inRange = false;
        }
    }
    /// <summary>
    /// Method is called every fixed framerate frame. It updates the rigid body position based on the movement vector and
    /// the speed of the player. It is also applying a constant gravity effect to ensure the player is always on the ground unless,
    /// the jump action has been called which adds a greater force.
    /// As well, it ensure the player is always looking in the direction they're moving.
    /// </summary>
    private void FixedUpdate()
    {
        rb.position += movement * speed;

        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

        if (movement.sqrMagnitude > 0)
        {
            Quaternion rotation = Quaternion .LookRotation(movement.normalized, Vector3.up);
            rb.rotation = Quaternion.Lerp(rb.rotation, rotation, Time.fixedDeltaTime * rotationRate);
        }
        if (isBlocking)
        {
            blockBox.GetComponent<BoxCollider>().enabled = true;
            saber.transform.localScale = Vector3.one;
            stamina.TakeStamina(blockAmount);
        }
        else
        {
            blockBox.GetComponent<BoxCollider>().enabled = false;
            stamina.AddStamina(blockAmount);
            audioController.StopSound(2);

        }

        currentStam = stamina.GetStamina();

        if (currentStam == 0)
        {
            DisableBlock();
            isBlocking = false;
        }
        else
        {
            EnableBlock();
        }

    }

    /// <summary>
    /// This method defines what should happen when the player initally collides with objects.
    /// </summary>
    /// <param name="collision">This is a collision</param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            speed = 0;
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Tree"))
        {
            speed = 0;
        }

    }

    /// <summary>
    /// This method defines what the player should do upon exiting a collision with an object.
    /// </summary>
    /// <param name="collision">This is a collision</param>
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            speed = speedPostWall;
        }

        if ((collision.gameObject.CompareTag("Floor")) || (collision.gameObject.CompareTag("Platform")) )
        {
            isGrounded = false;
        }
        
        if (collision.gameObject.CompareTag("Tree"))
        {
            speed = speedPostWall;
        }
    }

    /// <summary>
    /// This method defines what should happen when the player remains colliding with an object.
    /// </summary>
    /// <param name="collision">This is a collision</param>
    public void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Platform")) || (collision.gameObject.CompareTag("Floor")))
        {
            isGrounded = true;
        }

    }
    /// <summary>
    /// Method to define what should happen when the player is constantly colliding with a given collider.
    /// </summary>
    /// <param name="other">name of the collider</param>
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            damage.TakeDamagePlayer(10);
            Debug.Log("ow water");
        }
    }
    /// <summary>
    /// Checks if the player is able to move.
    /// </summary>
    public void CheckIfMove()
    {
        isMoving = move != Vector2.zero;
    }
}
