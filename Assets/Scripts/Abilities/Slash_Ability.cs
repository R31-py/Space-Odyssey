using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_Ability : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifetime = 5f;
    private float direction;
    private PlayerController player;
    private Rigidbody2D rb;

    private void Start()
    {
        Destroy(gameObject, lifetime); 
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        
        direction = player.transform.localScale.x;
        
        GetComponent<SpriteRenderer>().flipX = (direction < 0);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * (direction * speed * Time.deltaTime)); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.lifepoints -= 2;
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); 
        }
    }
}