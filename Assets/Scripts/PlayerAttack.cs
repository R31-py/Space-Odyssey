using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackReach;
    [SerializeField] private LayerMask enemyLayer;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float CDTimer = Mathf.Infinity;
    private BoxCollider2D boxCollider2D;

   private void Awake()
   {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        attackReach = 2;
   }

   private void Update()
   {
    if(Input.GetKey(KeyCode.Z) && CDTimer > attackCooldown)
    {
            Attack();
    }
        CDTimer += Time.deltaTime;
   }

   private void Attack()
   {
        animator.SetTrigger("attack"); 
        CDTimer = 0;
        EnemyController enemy = hasHit();
        if(enemy != null){
            enemy.lifePoints -= 1;
            enemy.body.velocity = new Vector2(transform.localScale.x * 10, enemy.body.velocity.y);

        }
   }

   private EnemyController hasHit()
{
    RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), attackReach, enemyLayer);
    if (raycastHit.collider != null)
    {
        EnemyController enemyController = raycastHit.collider.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            return enemyController; 
        }
    }
    
    return null;
}

}
