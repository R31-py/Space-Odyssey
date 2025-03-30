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
                ShootProjectile();
                attackTimer = attackCooldown;
            }
        }
    }

    private void ShootProjectile()
    {
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