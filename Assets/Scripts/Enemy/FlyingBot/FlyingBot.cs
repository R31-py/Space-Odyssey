using UnityEngine;

public class FlyingBot : Enemy
{
    [Header("Flying Settings")]
    public float idleSpeed = 3f;
    public float hoverRadius = 3f;
    public float directionChangeInterval = 2f;
    public float maxDirectionAngle = 45f;
    
    [Header("Combat Settings")]
    [SerializeField] public bool isShooting = false;
    public int playerAttackDamage = 1;
    [SerializeField] private string[] playerAttackAnimationStates = {"attack", "attack2"}; // Animation states considered as attacks
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    [Header("Death Effects")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionDuration = 0.5f;
    
    // Property to determine which way the bot is facing
    public float FacingDirection => Mathf.Sign(transform.localScale.x);
    
    // Private variables for movement
    private Vector2 randomDirection;
    private float lastDirectionChangeTime;
    private Vector2 startPosition;
    private bool initialized = false;
    private CircleCollider2D triggerCollider; // Reference to trigger collider
    private Collider2D physicalCollider; // Reference to physical collider

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
                DebugLog("FlyingBot: Rigidbody2D was missing and had to be added automatically");
            }
        }
        
        // Configure rigidbody for flying
        body.gravityScale = 0;
        body.drag = 0.5f;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Set start position
        startPosition = transform.position;
        PickNewRandomDirection();
        
        // Check animator
        if (GetComponent<Animator>() != null && animator == null)
        {
            animator = GetComponent<Animator>();
            DebugLog("Found animator component but it wasn't assigned");
        }
        
        // Find player if target is null
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            DebugLog("Looking for player: " + (target != null ? "Found" : "Not found"));
        }
        
        // Get existing physical collider
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            if (!col.isTrigger)
            {
                physicalCollider = col;
                DebugLog("Found physical collider: " + col.GetType().Name);
                break;
            }
        }
        
        // Add physical collider if none found
        if (physicalCollider == null)
        {
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = false;
            boxCollider.size = new Vector2(1f, 1f); // Set appropriate size
            physicalCollider = boxCollider;
            DebugLog("Added physical body collider");
        }
        
        // Add trigger collider if not exists
        triggerCollider = GetComponent<CircleCollider2D>();
        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<CircleCollider2D>();
            triggerCollider.isTrigger = true;
            triggerCollider.radius = triggerRange;
            DebugLog("Added trigger collider for player detection");
        }
        else if (!triggerCollider.isTrigger)
        {
            // If there's already a CircleCollider2D but it's not a trigger, add a second one
            CircleCollider2D newTrigger = gameObject.AddComponent<CircleCollider2D>();
            newTrigger.isTrigger = true;
            newTrigger.radius = triggerRange;
            triggerCollider = newTrigger;
            DebugLog("Added separate trigger collider for player detection");
        }
        
        initialized = true;
        DebugLog("Initialization complete. Start position: " + startPosition);
    }

    protected override void Update()
    {
        // Call base class Update which handles canSee() check
        base.Update();
        
        if (isDead)
        {
            DebugLog("Bot is dead, skipping Update");
            return;
        }
        
        if (!initialized)
        {
            DebugLog("Bot is not initialized, skipping Update");
            return;
        }

        // Check if target is null
        if (target == null)
        {
            DebugLog("Target is null. Make sure 'Player' tag is set and target is properly assigned in Enemy class");
            // Try to find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player;
                DebugLog("Found player and assigned as target");
            }
        }

        // Check if player is in sight
        if (target != null)
        {
            bool canSeeTarget = CanSee(target);
            DebugLog("Can see target: " + canSeeTarget);
            
            if (canSeeTarget)
            {
                targetInSight = true;
                SetCombatState(true);
            }
        }
    }

    // Override CanSee method to improve detection
    protected new bool CanSee(GameObject target)
    {
        if (target == null) return false;
        
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        
        // Make sure distance is not greater than triggerRange
        distance = Mathf.Min(distance, triggerRange);
        
        // Increase box size for better detection
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            new Vector2(1f, 1f), // Larger box
            0f,
            direction,
            distance, // Use actual distance
            ~LayerMask.GetMask(enemyLayer)
        );
        
        // Debug visualization
        Debug.DrawRay(transform.position, direction * distance, hit.collider != null ? Color.green : Color.red, 0.1f);
        
        if (hit.collider != null)
        {
            DebugLog("BoxCast hit: " + hit.collider.gameObject.name + " Tag: " + hit.collider.gameObject.tag);
            return hit.collider.CompareTag("Player");
        }
        
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only handle detection logic in trigger, not damage
        DebugLog("Trigger entered: " + collision.gameObject.name + " Tag: " + collision.gameObject.tag);
        
        if (collision.CompareTag("Player"))
        {
            targetInSight = true;
            SetCombatState(true);
            DebugLog("Player detected by trigger!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetInSight = false;
            SetCombatState(false);
            DebugLog("Player left trigger area");
        }
    }

    private void FixedUpdate()
    {
        if (isDead || !initialized)
        {
            if (isDead) DebugLog("Bot is dead, skipping FixedUpdate");
            if (!initialized) DebugLog("Bot is not initialized, skipping FixedUpdate");
            return;
        }
        
        // Check if body is frozen or has constraints that prevent movement
        if (body.constraints != RigidbodyConstraints2D.None && body.constraints != RigidbodyConstraints2D.FreezeRotation)
        {
            DebugLog("WARNING: Rigidbody has constraints that may prevent movement: " + body.constraints);
        }
        
        // Perform hovering movement
        RandomHover();
        
        // Face player when shooting
        if (isShooting && target != null)
        {
            FaceTarget();
            DebugLog("Facing target. isShooting: " + isShooting);
        }
        
        // Debug current velocity
        DebugLog("Current velocity: " + body.velocity + " Speed: " + body.velocity.magnitude);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DebugLog("Collision with: " + collision.gameObject.name + " Tag: " + collision.gameObject.tag);
        
        // Ignore projectiles completely
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("PlayerProjectile"))
        {
            DebugLog("Ignoring projectile collision");
            return;
        }
        
        // Only process player collisions
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision(collision.gameObject);
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        Animator playerAnimator = player.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);
            
            // Check if player is in any attack animation state
            bool isPlayerAttacking = false;
            foreach (string attackState in playerAttackAnimationStates)
            {
                if (currentState.IsName(attackState))
                {
                    isPlayerAttacking = true;
                    break;
                }
            }
            
            DebugLog("Player animation state: " + currentState.fullPathHash + " IsAttack: " + isPlayerAttacking);
            
            if (isPlayerAttacking)
            {
                // Player is directly colliding and is in attack animation - apply damage
                ApplyDamage(playerAttackDamage);
                DebugLog("Player hit bot with melee attack. Damage: " + playerAttackDamage);
            }
            else
            {
                DebugLog("Player collision with bot, but not in attack state. No damage taken.");
            }
        }
        else
        {
            DebugLog("Player has no Animator component");
            // Explicit handling of null animator case - can be customized based on design requirements
            // For example, default to no damage if there's no animator to check attack state
        }
    }

    private void ApplyDamage(int amount)
    {
        if(isDead) return;
    
        int oldLifepoints = lifepoints;
        lifepoints -= amount;
        DebugLog("Bot hit! Damage: " + amount + " Old HP: " + oldLifepoints + " New HP: " + lifepoints);
        
        if (lifepoints <= 0)
        {
            isDead = true;
            DebugLog("Bot died!");
            
            // Spawn explosion effect
            if(explosionPrefab != null)
            {
                GameObject explosion = Instantiate(
                    explosionPrefab, 
                    transform.position, 
                    Quaternion.identity
                );
                Destroy(explosion, explosionDuration);
            }
            else
            {
                Debug.LogWarning("No explosion prefab assigned to FlyingBot");
            }
            
            Destroy(gameObject, 0.1f); // Small delay before destroying bot
        }
    }

    // Override to block all external damage sources
    public override void getHit(int amount)
    {
        DebugLog("Blocked external damage call - only melee collisions can damage this bot");
    }

    public void SetCombatState(bool shouldShoot)
    {
        bool stateChanged = isShooting != shouldShoot;
        isShooting = shouldShoot;
        if (stateChanged)
        {
            DebugLog("Combat state changed to: " + (isShooting ? "Shooting" : "Not Shooting"));
        }
    }

    public void FaceTarget()
    {
        if (target == null) return;
        
        // Calculate direction and apply to local scale
        float direction = target.transform.position.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.01f) // Small threshold to prevent flickering
        {
            float xScale = Mathf.Sign(direction);
            Vector3 oldScale = transform.localScale;
            transform.localScale = new Vector3(xScale, 1, 1);
            
            if (oldScale.x != xScale)
            {
                DebugLog("Flipped direction to face: " + (xScale > 0 ? "right" : "left"));
            }
        }
    }

    private void RandomHover()
    {
        // Change direction periodically
        if (Time.time - lastDirectionChangeTime > directionChangeInterval)
        {
            PickNewRandomDirection();
            lastDirectionChangeTime = Time.time;
            DebugLog("Changed hover direction to: " + randomDirection);
        }

        // Apply correction to stay within hover radius
        float distanceFromStart = Vector2.Distance(transform.position, startPosition);
        DebugLog("Distance from start: " + distanceFromStart + " / " + hoverRadius);
        
        if (distanceFromStart > hoverRadius)
        {
            // Calculate direction to return to center
            Vector2 returnDirection = (startPosition - (Vector2)transform.position).normalized;
            
            // Old direction before adjustment
            Vector2 oldDirection = randomDirection;
            
            // Smoothly blend current direction with return direction
            randomDirection = Vector2.Lerp(randomDirection, returnDirection, 0.1f).normalized;
            
            DebugLog("Adjusting direction to stay in radius. Old: " + oldDirection + " New: " + randomDirection);
        }

        // Apply movement
        Vector2 oldVelocity = body.velocity;
        body.velocity = randomDirection * idleSpeed;
        
        // Check if velocity changed
        if ((oldVelocity - body.velocity).sqrMagnitude > 0.1f)
        {
            DebugLog("Applied velocity: " + body.velocity + " Speed: " + body.velocity.magnitude);
        }
        
        // If velocity is zero but we're trying to move, something is wrong
        if (body.velocity.sqrMagnitude < 0.1f && randomDirection.sqrMagnitude > 0.1f)
        {
            DebugLog("WARNING: Zero velocity despite movement command. Check rigidbody settings or collisions.");
        }
    }

    private void PickNewRandomDirection()
    {
        Vector2 toCenter = (startPosition - (Vector2)transform.position);
    
        // Handle case when already at center
        if (toCenter.magnitude < 0.1f)
        {
            // Generate completely random direction when at center
            randomDirection = Random.insideUnitCircle.normalized;
        }
        else
        {
            // Original logic with angle variation
            float randomAngle = Random.Range(-maxDirectionAngle, maxDirectionAngle);
            randomDirection = Quaternion.Euler(0, 0, randomAngle) * toCenter.normalized;
        }

        DebugLog("New random direction: " + randomDirection);
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
        
        // Draw direction arrow
        if (Application.isPlaying && initialized)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + randomDirection * 2);
        }
        
        // Draw detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
    
    private void DebugLog(string message)
    {
        if (showDebugLogs)
        {
            Debug.Log("[FlyingBot " + gameObject.name + "]: " + message);
        }
    }
}