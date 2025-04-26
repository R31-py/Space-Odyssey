using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloidLaser : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    public Vector2 direction;
    

    private void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);//Space.Self=>relative to the laser; makes it move along whatever X
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
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject); 
        }
    }
}
