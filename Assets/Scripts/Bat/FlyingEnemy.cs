using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int health = 1;
    [SerializeField] private float hitCooldown = 1f;
    [SerializeField] private PlayerValues player;

    private Rigidbody2D body;
    private Animator animator;
    private Transform playerTransform;
    private float currentHitCooldown = 0f;
    private bool playerDetected = false;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isFlying = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= detectionRange && !playerDetected)
        {
            playerDetected = true;
            animator.SetBool("Detected", true);
            StartCoroutine(StartFlying());
        }

        if (isFlying && !isAttacking)
        {
            MoveTowardsPlayer();
        }
        
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
        
        if (currentHitCooldown < hitCooldown)
        {
            currentHitCooldown += Time.deltaTime;
        }
    }

    private IEnumerator StartFlying()
    {
        yield return new WaitForSeconds(0.5f);
        
        isFlying = true;
        animator.SetBool("Fly", true);
        //
        animator.SetBool("Detected", false);
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        body.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        animator.SetTrigger("Bite");

        body.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            player.health -= 1;
        }

        isAttacking = false;
    }

    public void TakeDamage()
    {
        if (isDead) return;

        isDead = true;
        isFlying = false;
        body.velocity = Vector2.zero;
        
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        
        Destroy(gameObject, 1f);
    }
}
