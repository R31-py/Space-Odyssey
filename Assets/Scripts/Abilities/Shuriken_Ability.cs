using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken_Ability : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 10f;
    private float direction;
    private PlayerController player;
    private Animator animator;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>(); 
        
        direction = player.transform.localScale.x;
        
    }

    private void Update()
    {
        transform.position += Vector3.right * (direction * speed * Time.deltaTime);

        // Diese Animation rotiert das Objekt nach der Z-Achse
        animator.Play("shuriken_spin");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.CompareTag("Enemy") && collision is BoxCollider2D)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.lifepoints -= 4;
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Shuriken hit the wall!");
            Destroy(gameObject);
        }
    }
}