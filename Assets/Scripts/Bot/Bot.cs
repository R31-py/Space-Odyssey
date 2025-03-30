using UnityEngine;

public class Bot : MonoBehaviour
{
    [Header("Detection & Shooting")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float shootRange = 3f;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float moveSpeed = 2f; // Added speed for movement

    [Header("Bot Stats")]
    [SerializeField] private int health = 3;

    [Header("References")]
    [SerializeField] private PlayerValues player; // Player script with a health property
    private Transform playerTransform;
    private Rigidbody2D body; // Rigidbody for movement
    private Animator animator;

    private float currentShootTimer;
    private bool isDead = false;
    private bool isDetected = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        body = GetComponent<Rigidbody2D>(); // Get Rigidbody2D

        if (body == null)
        {
            Debug.LogError("[Bot] No Rigidbody2D found! Movement will not work.");
        }
    }

    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // If player is in detection range, detect the player
        if (!isDetected && distanceToPlayer <= detectionRange)
        {
            isDetected = true;
            animator.SetBool("Detected", true);
        }

        // If detected but NOT in shoot range, move toward the player
        if (isDetected && distanceToPlayer > shootRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Stop moving when within shooting range
            body.velocity = Vector2.zero;
        }

        // If within shooting range, attack with cooldown
        if (isDetected && distanceToPlayer <= shootRange)
        {
            currentShootTimer += Time.deltaTime;
            if (currentShootTimer >= shootCooldown)
            {
                animator.SetTrigger("Shoot");
                currentShootTimer = 0f;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null || body == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        body.velocity = new Vector2(direction.x * moveSpeed, body.velocity.y);

        // Flip bot to face the player
        transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);

        Debug.Log($"[Bot] Moving: Direction={direction}, Velocity={body.velocity}");
    }

    public void ShootPlayer()
    {
        if (player != null)
        {
            player.health -= 1;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            animator.SetTrigger("Death");

            body.velocity = Vector2.zero; // Stop movement on death
            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 1f);
        }
    }
}
