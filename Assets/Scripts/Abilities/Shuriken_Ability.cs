using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken_Ability : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 10f;
    private Animator animator;
    private Transform transform;
    private void Start()
    {
        Destroy(gameObject, lifetime);
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        
        Vector3 currentPosition = transform.position;

        currentPosition.x += speed * Time.deltaTime;
        
        transform.position = currentPosition;
        //Diese Animation rotiert das GameObject nach der z-Achse
        animator.Play("shuriken_spin");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
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
            Destroy(gameObject); 
        }
    }
}
