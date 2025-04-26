using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamelController : Enemy
{
    [SerializeField] private float deathTimer = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashDuration = 0.2f;
    private PlayerValues player;

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

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerValues>();
            }
        }

        playerTransform = player?.transform;
    }

    private void Update()
    {
        if (isCharging) return; 
        
        if (currentHitCooldown < attackCooldown)
        {
            currentHitCooldown += Time.deltaTime;
        }

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
            if (player != null)
            {
                player.money += dropmoney; 
            }
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
            if (player == null)
            {
                player = collision.GetComponent<PlayerValues>();
            }
            Debug.Log("Chamel detected Player!");

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (collision.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true;  
                movingDirection = -1;          
            }
            else
            {
                spriteRenderer.flipX = false;   
                movingDirection = 1;           
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && currentHitCooldown >= attackCooldown)
        {
            if (player == null)
            {
                player = other.gameObject.GetComponent<PlayerValues>();
            }

            player.health -= 1;
            currentHitCooldown = 0f;
            Debug.Log("Chamel damaged the player!");
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            GetComponent<SpriteRenderer>().flipX = movingDirection < 0;
            Debug.Log("Direction changed!");
        }
    }
}
