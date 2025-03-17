using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peyeramid : Enemy
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float height = 3f;
    [SerializeField] public PlayerValues player;
    private Vector3 startPos;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    private void Update()
    {
        if (canMove)
        {
            Move(0); // Direction is unused, but needed for compatibility
        }

        if (lifepoints <= 0)
        {
            canMove = false;
            Destroy(gameObject);
        }
    }

    public override void Attack()
    {
        // Implement attack logic if necessary
        Debug.Log($"{gameObject.name} attacks!");
    }

    public override void Move(float direction)
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z); // Keep original X/Z, move only Y
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.health -= 1;
        }
    }
    
    
}
