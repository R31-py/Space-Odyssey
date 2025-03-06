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
            Destroy(gameObject, deathTimer);
        }
    }

    public override void Attack()
    {
        if (playerTransform == null) return;

        Debug.Log($"{gameObject.name} attacked!");
        animator.SetTrigger(attackAnimationName);
        
    }

    public void SpawnDagger()
    {
        Debug.Log($"{gameObject.name} spawned a dagger!");

        // Richtung zum Spieler berechnen
        /*Vector2 attackDirection = (playerTransform.position - transform.position).normalized;

        // Projektil erzeugen
        GameObject dagger = Instantiate(SHs_Dagger, transform.position, Quaternion.identity);
        Rigidbody2D daggerRb = dagger.GetComponent<Rigidbody2D>();
        daggerRb.velocity = attackDirection * 10f;*/ // Geschwindigkeit anpassen
        Instantiate(SHs_Dagger, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }

    public override void Move(int direction)
    {
        throw new System.NotImplementedException();
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
            Trigger();
        }
    }
}
