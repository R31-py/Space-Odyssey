using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloidLaser : MonoBehaviour
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
        if (collision.CompareTag("Player"))
        {
            PlayerValues playerValues = collision.GetComponent<PlayerValues>();
            if (playerValues != null)
            {
                playerValues.health -= 1;
            }
            Destroy(gameObject); 
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); 
        }
    }
}
