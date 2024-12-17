using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotutController : MonoBehaviour
{
    
[SerializeField] public float lifePoints = 14f;
    [SerializeField] public PlayerValues player;
    public Rigidbody2D body;
    private float movingDirection = 1f;
    private float hitCooldown = 1f;
    private float directionChange = 0f;
    private float speed = 3f;
    [SerializeField]public Animator animator;
    public bool playerDetected = false;
    public bool canMove = false;
    public float waitTime = .3f;
    public float deathTimer = 3f;
    private float pushDistance = 0f;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movingDirection = Math.Abs(player.transform.position.x - transform.position.x) / (player.transform.position.x - transform.position.x);
        if (canMove)
        {
            body.velocity = new Vector2(movingDirection * speed, body.velocity.y);
        }

        if (lifePoints <= 0)
        {
            animator.SetTrigger("Dead");
        }
        hitCooldown += Time.deltaTime;
    }

    public void die()
    {
        Destroy(gameObject);
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

        if (waitTime > 0f)
        {
            waitTime -= Time.deltaTime;
        }
        else if(waitTime <= 0 && playerDetected)
        {
            canMove = true;
            animator.SetBool("canMove", true);
            waitTime = 1f;
        }
    }

    public void damage()
    {
        if (canMove)
        {
            canMove = false;
            animator.SetBool("isWake", false);
            waitTime = 1f;
            animator.SetBool("canMove", false);
        }
        animator.SetTrigger("Damage");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !playerDetected)
        {
            animator.SetBool("isWake", true);
            animator.SetBool("canMove", true);
            canMove = true;
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("isWake", false);
            animator.SetBool("canMove", false);
            playerDetected = false;
            canMove = false;
        }
    }
}
