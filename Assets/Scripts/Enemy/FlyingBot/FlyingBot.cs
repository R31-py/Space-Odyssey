using UnityEngine;

public class FlyingBot : Enemy
{
    [Header("Flying Settings")]
    public float idleSpeed = 3f;
    public float hoverRadius = 3f;
    public float directionChangeInterval = 2f;
    public float maxDirectionAngle = 45f;
    
    [Header("Combat Settings")]
    [SerializeField] private bool isShooting = false;
    public int playerAttackDamage = 1;
    
    // Property to determine which way the bot is facing
    public float FacingDirection => Mathf.Sign(transform.localScale.x);
    
    // Private variables for movement
    private Vector2 randomDirection;
    private float lastDirectionChangeTime;
    private Vector2 startPosition;
    private bool initialized = false;

    protected override void Start()
    {
        base.Start();
        
        // Make sure we have a rigidbody
        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
            if (body == null)
            {
                body = gameObject.AddComponent<Rigidbody2D>();
            }
        }
        
        // Configure rigidbody for flying
        body.gravityScale = 0;
        body.drag = 0.5f;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Set start position
        startPosition = transform.position;
        PickNewRandomDirection();
        animator = null;
        initialized = true;
    }

    protected override void Update()
    {
        if (isDead || !initialized) return;

        // Check if player is in sight
        if (target != null && canSee(target))
        {
            targetInSight = true;
            SetCombatState(true);
        }
        else
        {
            targetInSight = false;
            SetCombatState(false);
        }
    }

    private void FixedUpdate()
    {
        if (isDead || !initialized) return;
        
        // Perform hovering movement
        RandomHover();
        
        // Face player when shooting
        if (isShooting && target != null)
        {
            FaceTarget();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Animator playerAnimator = collision.gameObject.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);
                if (currentState.IsName("attack") || currentState.IsName("attack2"))
                {
                    getHit(playerAttackDamage);
                }
            }
        }
    }

    public void SetCombatState(bool shouldShoot)
    {
        isShooting = shouldShoot;
    }

    private void FaceTarget()
    {
        if (target == null) return;
        
        // Calculate direction and apply to local scale
        float direction = target.transform.position.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.1f) // Small threshold to prevent flickering
        {
            float xScale = Mathf.Sign(direction);
            transform.localScale = new Vector3(xScale, 1, 1);
        }
    }

    private void RandomHover()
    {
        // Change direction periodically
        if (Time.time - lastDirectionChangeTime > directionChangeInterval)
        {
            PickNewRandomDirection();
            lastDirectionChangeTime = Time.time;
        }

        // Apply correction to stay within hover radius
        float distanceFromStart = Vector2.Distance(transform.position, startPosition);
        if (distanceFromStart > hoverRadius)
        {
            // Calculate direction to return to center
            Vector2 returnDirection = (startPosition - (Vector2)transform.position).normalized;
            
            // Smoothly blend current direction with return direction
            randomDirection = Vector2.Lerp(randomDirection, returnDirection, 0.1f).normalized;
        }

        // Apply movement
        body.velocity = randomDirection * idleSpeed;
    }

    private void PickNewRandomDirection()
    {
        // Calculate direction toward center
        Vector2 toCenter = (startPosition - (Vector2)transform.position).normalized;
        
        // Apply random angle variation
        float randomAngle = Random.Range(-maxDirectionAngle, maxDirectionAngle);
        randomDirection = Quaternion.Euler(0, 0, randomAngle) * toCenter;
        randomDirection = randomDirection.normalized;
    }

    // Override base class methods that we don't need
    public override void Move(float direction)
    {
        // Flying bots use RandomHover() instead of the standard Move method
    }
    
    public override void getHit(int amount)
    {
        if(isDead) return;
    
        lifepoints -= amount;
        if (lifepoints <= 0)
        {
            isDead = true;
            
            // Optional: Add death effect here
            // Instantiate(deathEffect, transform.position, Quaternion.identity);
            
            Destroy(gameObject, 0.2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Show hover radius in editor
        Gizmos.color = Color.cyan;
        if (Application.isPlaying && initialized)
        {
            Gizmos.DrawWireSphere(startPosition, hoverRadius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, hoverRadius);
        }
    }
}