using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_Ability : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime)); 
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
            Destroy(gameObject); 
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); 
        }
    }
}
