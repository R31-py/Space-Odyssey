using System.Collections;
using UnityEngine;

public class SpikeBossController : Enemy
{
    [Header("Spike Boss Settings")]
    public float jumpForce = 15f; 
    public float detectionRange = 8f;  

    [Header("Ceiling Positions")]
    [Tooltip("Left boundary of the ceiling (e.g., x=90, y=9)")]
    public Transform ceilingLeft;
    [Tooltip("Right boundary of the ceiling (e.g., x=111, y=9)")]
    public Transform ceilingRight;

    private enum BossState { Idle, Aiming, Jumping, OnCeiling }
    private BossState currentState = BossState.Idle;

    private Transform player;     
    private Rigidbody2D rb;
    private Animator animator;

    private bool hasCollided = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        currentState = BossState.Idle;
    }

    private void Update()
    {
        if (isDead) return;
        if (player != null && currentState == BossState.Idle)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= detectionRange)
            {
                currentState = BossState.Aiming;
                StartCoroutine(BossBehaviorRoutine());
            }
        }
        
        if (currentState == BossState.Aiming && player != null)
        {
            AimAt(player);
        }
    }
    
    private IEnumerator BossBehaviorRoutine()
    {
        while (!isDead && player != null)
        {
            currentState = BossState.Aiming;
            animator.SetTrigger("Aim");
            yield return new WaitForSeconds(1f);

            currentState = BossState.Jumping;
            animator.SetTrigger("Attack");
            JumpToTarget(player.position);
            
            hasCollided = false;
            float timer = 0f;
            while (!hasCollided && timer < 2f)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            currentState = BossState.OnCeiling;
            Vector2 ceilingTarget = new Vector2(
                Random.Range(ceilingLeft.position.x, ceilingRight.position.x),
                ceilingLeft.position.y 
            );
            animator.SetTrigger("Attack");
            JumpToTarget(ceilingTarget);
            
            timer = 0f;
            while (Vector2.Distance(transform.position, ceilingTarget) > 1f && timer < 2f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            currentState = BossState.Idle;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private void JumpToTarget(Vector2 targetPos)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        rb.velocity = direction * jumpForce;
    }

    private void AimAt(Transform target)
    {
        Vector2 diff = (Vector2)target.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;
            rb.velocity = Vector2.zero;

            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerValues pv = collision.gameObject.GetComponent<PlayerValues>();
                if (pv != null)
                {
                    pv.health -= 1;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    public override void getHit(int damage)
    {
        if (isDead) return;
        lifepoints -= damage;
        if (lifepoints <= 0)
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            Destroy(gameObject, 2f);
        }
    }
}
