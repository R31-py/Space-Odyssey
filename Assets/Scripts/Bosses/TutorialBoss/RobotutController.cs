using UnityEngine;
using System.Collections;

public class RobotutController : Enemy
{
    [Header("Robotut Settings")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float wakeDelay = 0.3f;
    [SerializeField] private float stunDuration = 0.5f;
    private float lastAttackTime;
    private bool isWakingUp;
    private PlayerValues playerValues;
    private float movingDirection = 1f;


    
    protected override void Start()
    {
        base.Start();
        InitializeRobotut();
    }

    private void InitializeRobotut()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        playerValues = target.GetComponent<PlayerValues>();
    
        deathAnimationName = "Dead";
        attackAnimationName = "Attack";
        moveAnimationName = "canMove";
    }

    protected override void Update()
    {
        if (isDead) return;
    
        base.Update();
        UpdateDirection();
    
        // Add distance check
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        bool isInAttackRange = distanceToPlayer <= attackRange;

        // Handle attack when in range
        if (CanAttack() && targetInSight && isInAttackRange) // Modified condition
        {
            Attack();
        }

        movingDirection = Mathf.Sign(target.transform.position.x - transform.position.x);
    }
    private void UpdateDirection()
    {
        if (targetInSight)
        {
            transform.localScale = new Vector3(- movingDirection, 1, 1);
        }
    }

    public override void Attack()
    {
        if (CanAttack())
        {
            base.Attack();
            animator.SetTrigger("Attack");
            playerValues.health -= 1;
            lastAttackTime = Time.time;
        }
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    public override void Trigger()
    {
        if (!targetInSight && !isDead)
        {
            StartCoroutine(WakeUpSequence());
        }
    }

    private IEnumerator WakeUpSequence()
    {
        isWakingUp = true;
        animator.SetBool("isWake", true);
        yield return new WaitForSeconds(wakeDelay);
        targetInSight = true;
        canMove = true;
        animator.SetBool("canMove", true);
        isWakingUp = false;
    }

    public override void getHit(int damage)
    {
        if (isDead) return;
    
        base.getHit(damage);
    
        if (lifepoints <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage");
            StartCoroutine(HandleStun());
        }
    }

// Modified die method
    

    private IEnumerator HandleStun()
    {
        canMove = false;
        animator.SetBool("canMove", false);
        yield return new WaitForSeconds(stunDuration);
        if (!isDead) 
        {
            canMove = true;
            animator.SetBool("canMove", true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
        }
        
        // Original wall collision handling
        if (collision.gameObject.CompareTag("Wall"))
        {
            movingDirection *= -1;
            transform.localScale = new Vector3(movingDirection, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !targetInSight)
        {
            animator.SetBool("isWake", true);
            canMove = true;
            targetInSight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("ShutDown");
            animator.SetBool("isWake", false);
            animator.SetBool("canMove", false);
            canMove = false;
            targetInSight = false;
        }
    }

   public override void Move(float direction)
{
    if (canMove)
    {
        base.Move(direction);
        body.velocity = new Vector2(movingDirection * moveSpeed, body.velocity.y);
    }
}

private new void Die() // Hides base Enemy.Die() if needed
{
    if (isDead) return;
    
    isDead = true;
    animator.SetTrigger("Death");
    Destroy(gameObject, 1f);
    
    // Disable all components
    GetComponent<Collider2D>().enabled = false;
    this.enabled = false;
}
void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, triggerRange);
}
}