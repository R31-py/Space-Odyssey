using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string abilityName; // Consider making it private and controlled via a property if not changing dynamically
    [SerializeField] private Sprite abilityIcon;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private BoxCollider2D boxCollider2D;

    void Start()
    {
        // Ensure the required components are present
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on the GameObject.");
            }
        }

        if (boxCollider2D == null)
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
            {
                Debug.LogError("BoxCollider2D component not found on the GameObject.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Your logic for ability input handling can be added here.
    }

    public void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger(abilityName);
        }
        else
        {
            Debug.LogWarning("Animator not set up for the ability.");
            return;
        }

        Collider2D enemy = HasHit();
        if (enemy != null)
        {
            if (enemy.GetComponent<EnemyController>() != null)
            {
                EnemyController e = enemy.GetComponent<EnemyController>();
                // Apply damage to the enemy
                e.lifePoints -= damage;
                e.animator.SetTrigger("onHitTrig");
                e.pushBack(.3f * transform.localScale.x);
            }
            if(enemy.GetComponent<RobotutController>() != null)
            {
                Debug.Log("Po mret");
                RobotutController rb = enemy.GetComponent<RobotutController>();
                rb.lifePoints -= 1;
                rb.damage();
            } 
        }
    }

    private Collider2D HasHit()
    {
        if (boxCollider2D == null)
        {
            Debug.LogWarning("BoxCollider2D not set up for hit detection.");
            return null;
        }

        // Cast a box in the direction the character is facing to detect enemies
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), range, enemyLayer);
        if (raycastHit.collider != null)
        {
            Collider2D enemyController = raycastHit.collider;
            if (enemyController != null)
            {
                return enemyController;
            }
        }

        return null;
    }
}
