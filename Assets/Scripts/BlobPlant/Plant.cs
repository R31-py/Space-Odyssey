using System.Collections;
using UnityEngine;

public class Plant : Enemy
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private GameObject plantAttackPrefab;
    
    private float attackTimer = 0f;
    private Transform playerTransform;
    private bool playerDetected = false;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Start()
    {
        attackTimer = attackCooldown; // Ensures the plant doesn't shoot immediately
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        playerDetected = distanceToPlayer <= detectionRange;

        if (playerDetected)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackTimer = attackCooldown; // Reset cooldown (ShootProjectile is now called only via animation event)
            }
        }
    }

    // Ensure this method is correctly linked in the animation event
    private void ShootProjectile()
    {
        Debug.Log("Plant is shooting a projectile!");

        if (plantAttackPrefab != null)
        {
            GameObject projectile = Instantiate(plantAttackPrefab, transform.position, Quaternion.identity);
            PlantAttack attackScript = projectile.GetComponent<PlantAttack>();
            if (attackScript != null)
            {
                attackScript.SetTarget(playerTransform.position);
            }
        }
    }
}