using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamelController : Enemy
{
    [SerializeField] private float deathTimer = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] public PlayerValues player;

    private float attackTimer = 0f;
    private int movingDirection = 1;
    private bool playerDetected = false;
    private bool isCharging = false;
    private float currentHitCooldown = 0f;

    
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
        
        if (isCharging) return; 

        if (canMove)
        {
            Move(-movingDirection);
        }

        if (playerDetected)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                Attack();
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
        
        if (dashDuration > 0)
        {
            dashDuration -= Time.deltaTime;
            moveSpeed = dashSpeed;
            animator.SetTrigger(attackAnimationName);
        }else
        {
            moveSpeed = 3f;
        }
        
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && attackTimer <= 0)
        {
            Debug.Log(currentHitCooldown);
            player.health -= 1;
            attackTimer = attackCooldown;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            GetComponent<SpriteRenderer>().flipX = movingDirection < 0;
            Debug.Log("Direction changed!");
        }
    }
}
