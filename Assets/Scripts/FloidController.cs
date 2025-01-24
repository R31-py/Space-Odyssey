using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloidController : MonoBehaviour, IEnemy
{
    [SerializeField] public float lifePoints = 4f;
    public float deathTimer = 3f;
    
    [SerializeField] private GameObject FloidLaser;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speed = 2f;
    
    [SerializeField] private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    [SerializeField]public Animator animator;
    public Rigidbody2D body;
    public bool canMove = true;
    private int movingDirection = 1;
    
    private Transform playerTransform;
    private bool playerDetected = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        Move(); 
        
        if (playerDetected)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                Attack(); 
                attackTimer = attackCooldown;
            }
        }
        
        if (lifePoints <= 0)
        {
            animator.SetTrigger("Death");
            canMove = false;
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Attack()
    {
        Debug.Log($"{gameObject.name} attacked!");
        Vector2 direction = (playerTransform.position - firePoint.position).normalized;
        GameObject laser = Instantiate(FloidLaser, firePoint.position, Quaternion.identity);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        laserRb.velocity = direction * 10f;
        animator.SetTrigger("attack");
    }

    public void Move()
    {
        if (canMove)
        {
            body.velocity = new Vector2(-movingDirection * speed, body.velocity.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        
    }

    public void Trigger()
    {
        Debug.Log($"{gameObject.name} player detected!");
        playerDetected = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger(); // Spieler wurde entdeckt
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            movingDirection *= -1;
            transform.localScale = new Vector3(movingDirection, 1, 1);
        }
    }
}
