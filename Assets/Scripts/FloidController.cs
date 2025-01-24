using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloidController : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField]public Animator animator;
    public Rigidbody2D body;
    private int movingDirection = 1;
    private float attackTimer = 0f;
    private Transform playerTransform;
    private bool playerDetected = false;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        
        // Spieler-Referenz finden, vorausgesetzt, der Spieler hat das Tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        Move(); // Bewegung ausführen, wenn der Spieler entdeckt wurde
    }

    public void Attack()
    {
        Debug.Log($"{gameObject.name} greift an!");
        // Hier kannst du Schaden an den Spieler zufügen
        // Zum Beispiel:
        // if (playerTransform != null) playerTransform.GetComponent<PlayerValues>()?.TakeDamage(damage);
    }

    public void Move()
    {
        body.velocity = new Vector2(-movingDirection * speed, body.velocity.y);
    }

    public void Trigger()
    {
        Debug.Log($"{gameObject.name} hat den Spieler entdeckt!");
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
