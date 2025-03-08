using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Controller : Enemy
{
    [SerializeField] private float deathTimer = 3f;
    [SerializeField] private GameObject SHs_Dagger;
    [SerializeField] private float attackCooldown = 5f;
    private float attackTimer = 1f;
    private Transform playerTransform;
    private bool playerDetected = false;
    private bool isAttacking = false; 

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (!playerDetected) 
        {
            isAttacking = false; 
            animator.ResetTrigger(attackAnimationName); // Stop attack animation if player leaves
            animator.Play("idle");
            return;
        }

        // Attack only if not already attacking
        if (!isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    public override void Attack()
    {
        if (playerTransform == null) return;
        // Richtung zum Spieler berechnen
        /*Vector2 attackDirection = (playerTransform.position - transform.position).normalized;

        // Projektil erzeugen
        GameObject dagger = Instantiate(SHs_Dagger, transform.position, Quaternion.identity);
        Rigidbody2D daggerRb = dagger.GetComponent<Rigidbody2D>();
        daggerRb.velocity = attackDirection * 10f;*/ // Geschwindigkeit anpassen

        Debug.Log($"{gameObject.name} attacked!");
        isAttacking = true; // Prevent multiple attacks at once
        animator.SetTrigger(attackAnimationName);
    }

    // Animation event for dagger spawn
    public void SpawnDagger()
    {
        if (playerDetected) // Only spawn dagger if player is still in range
        {
            Instantiate(SHs_Dagger, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log($"{gameObject.name} tried to attack, but the player is gone.");
        }

        isAttacking = false; // Allow next attack
    }
    
    public override void Trigger()
    {
        Debug.Log($"{gameObject.name} player detected!");
        playerDetected = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log($"{gameObject.name} detected the player.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
            Debug.Log($"{gameObject.name} lost sight of the player.");
        }
    }
}
