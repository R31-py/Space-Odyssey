using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamelController : Enemy
{
    [SerializeField] private float deathTimer = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashDuration = 0.2f;

    private float attackTimer = 0f;
    private int movingDirection = 1;
    private bool playerDetected = false;
    private bool isCharging = false;
    
    private Transform playerTransform;
    private Rigidbody2D body;
    private Animator animator;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (movingDirection == 0) movingDirection = 1;
        playerTransform = target.transform;
    }

    private void Update()
    {
        if (dashDuration > 0)
        {
            dashDuration -= Time.deltaTime;
            moveSpeed = dashSpeed;
            animator.SetTrigger(attackAnimationName);
        }else
        {
            moveSpeed = 3f;
        }
        
        if (isCharging) return; 

        if (canMove)
        {
            body.velocity = new Vector2(-movingDirection * moveSpeed, body.velocity.y);
            animator.SetTrigger(moveAnimationName);
        }

        if (playerDetected)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }

        if (lifepoints <= 0)
        {
            animator.SetTrigger(deathAnimationName);
            canMove = false;
            Destroy(gameObject);
        }
    }

    public override void Attack()
    {
        if (playerTransform == null) return;
        // Recalculate direction using the latest player position
        Vector2 chargeDirection = (playerTransform.position - transform.position).normalized;
        
        animator.SetTrigger(attackAnimationName);
        Debug.Log("Chamel attacked with event!");
    }

    public override void Trigger()
    {
        playerDetected = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger();
            Debug.Log("Chamel detected Player!");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isCharging && (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Player")))
        {
            body.velocity = Vector2.zero; // Stop charge on collision
            isCharging = false;
            canMove = true;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            GetComponent<SpriteRenderer>().flipX = movingDirection < 0;
            Debug.Log("Direction changed!");
        }
    }
}
