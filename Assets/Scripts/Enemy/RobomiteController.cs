using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobomiteController : Enemy
{
    [SerializeField] private float waitTime = 0.3f;
    [SerializeField] private float hitCooldown = 1f;
    [SerializeField] public PlayerValues player;
    private float currentHitCooldown = 0f;
    private int movingDirection = 1;
    private bool playerDetected = false;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerDetected)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                canMove = true;
            }
        }
        
        if (canMove)
        {
            body.velocity = new Vector2(-movingDirection * moveSpeed, body.velocity.y);
            animator.SetTrigger(moveAnimationName);
        }
        
        if (currentHitCooldown < hitCooldown)
        {
            currentHitCooldown += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!playerDetected)
            {
                animator.SetTrigger("Detected");
                playerDetected = true;
            }
            
            if (collision.transform.position.x > transform.position.x)
            {
                movingDirection = -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                movingDirection = 1;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && currentHitCooldown >= hitCooldown)
        {
            Debug.Log(currentHitCooldown);
            player.health -= 1;
            currentHitCooldown = 0f;
        }
        
        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            transform.localScale = new Vector3(movingDirection, 1, 1);
        }
    }

}
