using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloidController : Enemy
{
    [SerializeField] private float deathTimer = 3f;
    [SerializeField] private GameObject floidLaser;
    [SerializeField] private float attackCooldown = 1.5f;
    private Vector3 firePoint ;
    private float attackTimer = 0f;
    private int movingDirection = 1;
    private bool playerDetected = false;
    private Transform playerTransform;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (movingDirection == 0) movingDirection = 1;
        playerTransform = target.transform;
    }

    private void  Update()
    {
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
        
        firePoint = transform.position + new Vector3(0, 0.8f, 0);
        
        Vector2 direction = (playerTransform.position - firePoint).normalized;
        GameObject laser = Instantiate(floidLaser, firePoint, Quaternion.identity);
        laser.SetActive(true);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        laserRb.velocity = direction * 10f;
        
        //animator.SetTrigger(attackAnimationName);
        Debug.Log("Floid attacked!");
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
            Debug.Log("Floid detected Player!");
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            GetComponent<SpriteRenderer>().flipX = movingDirection < 0;
            Debug.Log("Richtung geändert!");
        }
    }
}

