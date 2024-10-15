using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float lifePoints = 4f;
    [SerializeField] public PlayerValues player;
    public Rigidbody2D body;
    private int movingDirection = 1;
    private float hitCooldown = 1f;
    private float directionChange = 0f;
    private float speed = 1f;
    [SerializeField]private Animator animator;
    public bool playerDetected = false;
    public bool canMove = false;
    public float waitTime = .3f;
    public float deathTimer = 3f;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
            body.velocity = new Vector2(-movingDirection * speed, body.velocity.y);
            animator.SetTrigger("Move");
        }

        if (lifePoints <= 0)
        {
            animator.SetTrigger("Death");
            canMove = false;
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
                Destroy(gameObject);
        }
        
        if (hitCooldown < 1f)
        {
            hitCooldown += Time.deltaTime;
        }
    }
 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speed = 3f;
            if (!playerDetected)
            {
                animator.SetTrigger("Detected");
                playerDetected = true;
            }
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                movingDirection = -1;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                movingDirection = 1;
            }
        }
        else
        {
            speed = 1f;
        }
        
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && hitCooldown >= 1f)
        {
            Debug.Log(hitCooldown);
            animator.SetTrigger("Attack");
            player.health -= 1;
            hitCooldown = 0f;
        }

        if (other.gameObject.tag == "Wall")
        {
            movingDirection *= -1;
            transform.localScale = new Vector3(movingDirection, 1, 1);
        }
    }

}
