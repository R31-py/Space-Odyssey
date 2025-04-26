using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peyeramid : Enemy
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float height = 3f;
    [SerializeField] private float hitCooldown = 1f;
    private float currentHitCooldown = 0f;
    private PlayerValues player;
    private Rigidbody2D body;
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerValues>();
            }
        }
    }

    private void Update()
    {
        
        if (canMove)
        {
            Move(0); // Direction is unused, but needed for compatibility
        }
        
        if (currentHitCooldown < hitCooldown)
        {
            currentHitCooldown += Time.deltaTime;
        }    

        if (lifepoints <= 0)
        {
            canMove = false;
            Destroy(gameObject);
        }
    }

    public override void Move(float direction)
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z); // Keep original X/Z, move only Y
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            if (player == null)
            {
                player = other.gameObject.GetComponent<PlayerValues>();
            }

            if (currentHitCooldown >= hitCooldown)
            {
                player.health -= 1;
                currentHitCooldown = 0f;
            }
        }
    }
    
}
