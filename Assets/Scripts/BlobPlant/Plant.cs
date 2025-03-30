using System.Collections;
using UnityEngine;

public class Plant : Enemy
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private GameObject plantAttackPrefab;
    
    private float attackTimer = 0f;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Start()
    {
        attackTimer = 0f;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (canSee(target) && attackTimer <= 0f)
        {
            ShootProjectile();
            attackTimer = attackCooldown;
        }
    }

    private void ShootProjectile()
    {
        Debug.Log("Plant is shooting a projectile!");

        if (plantAttackPrefab != null)
        {
            GameObject projectile = Instantiate(plantAttackPrefab, transform.position, Quaternion.identity);
            PlantAttack attackScript = projectile.GetComponent<PlantAttack>();
            if (attackScript != null)
            {
                attackScript.SetTarget(target.transform.position);
            }
        }
    }
}