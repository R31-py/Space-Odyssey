using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloidController : Enemy
{
    [SerializeField] private float deathTimer = 3f;
    [SerializeField] private GameObject floidLaser;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 1.5f;
    private float attackTimer = 0f;
    private int movingDirection = 1;
    private Transform playerTransform;
    private bool playerDetected = false;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (movingDirection == 0) movingDirection = 1;
    }

    private void Update()
    {
        if (canMove)
        {
            body.velocity = new Vector2(-movingDirection * moveSpeed, body.velocity.y);
            if (body.velocity.x != 0)
            {
                animator.SetTrigger(moveAnimationName);
            }
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
        
        Vector2 direction = (playerTransform.position - firePoint.position).normalized;
        GameObject laser = Instantiate(floidLaser, firePoint.position, Quaternion.identity);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        laserRb.velocity = direction * 10f;
        
        animator.SetTrigger(attackAnimationName);
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
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            transform.localScale = new Vector3(movingDirection, 1, 1);
            Debug.Log("Richtung geändert!");
        }
    }
}

