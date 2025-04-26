using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : Enemy
{
    [SerializeField] private float attackCooldown = 1.5f;
    private PlayerValues player;

    private bool isAttacking = false;
    private bool playerDetected = false;
    private int movingDirection = 1;
    private float currentHitCooldown = 0f;
    
    private void Awake()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerValues>();
            }
        }
    }
    

    private void Update()
    {
        if (lifepoints <= 0)
        {
            animator.SetTrigger(deathAnimationName);
            canMove = false;
            Destroy(gameObject);
        }

        if (playerDetected)
        {
            canMove = true;
        }
    
        if (canMove)
        {
            Move(-movingDirection);
            animator.SetBool(moveAnimationName, true);
        }

        // Update cooldown over time
        if (currentHitCooldown < attackCooldown)
        {
            currentHitCooldown += Time.deltaTime;
        }

        // Check if the enemy is in range and ready to attack
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        if (distanceToPlayer <= attackRange && currentHitCooldown >= attackCooldown && !isAttacking)
        {
            StartAttacking();
        }
    }

    private void StartAttacking()
    {
        isAttacking = true;
        canMove = false;
        animator.SetBool(moveAnimationName, false);
        animator.SetTrigger(attackAnimationName);

        // Optional: You can call ResetAttackState if you need the cooldown to restart
        StartCoroutine(ResetAttackState());
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        canMove = true;
    }

    public override void Trigger()
    {
        if (!playerDetected) // Only trigger once
        {
            playerDetected = true;
            canMove = true;
            Debug.Log("Guardian detected Player!");
        }
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
            currentHitCooldown = 0f; // Reset cooldown after attack
            Debug.Log("Guardian damaged the player!");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            GetComponent<SpriteRenderer>().flipX = movingDirection < 0;
        }
    }
}