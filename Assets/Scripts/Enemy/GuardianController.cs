using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : Enemy
{
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] public PlayerValues player;

    private bool isAttacking = false;
    private bool playerDetected = false;
    private int movingDirection = 1;
    private float currentHitCooldown = 0f;
    

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

        if (isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            StartAttacking();
        }
        
        if (currentHitCooldown < attackCooldown)
        {
            currentHitCooldown += Time.deltaTime;
        }
    }

    private void StartAttacking()
    {
        isAttacking = true;
        canMove = false; 
        animator.SetBool(moveAnimationName, false);
        animator.SetTrigger(attackAnimationName);
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
        playerDetected = true;
        canMove = true;
        Debug.Log("Guardian detected Player!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger();
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && currentHitCooldown >= attackCooldown)
        {
            Debug.Log(currentHitCooldown);
            player.health -= 1;
            currentHitCooldown = 0f;
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