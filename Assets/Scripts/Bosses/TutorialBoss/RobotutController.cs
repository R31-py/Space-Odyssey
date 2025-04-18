using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RobotutController : Enemy
{
    [Header("Robotut Settings")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float wakeDelay = 0.3f;
    [SerializeField] private float stunDuration = 0.5f;
    [SerializeField] private float turnDelay = 0.5f; // New turning delay variable
    [SerializeField] private float attackDamageDelay = 0.4f; // Delay before damage is applied in attack animation
    [SerializeField] private GameObject[] objectsToDestroyOnDeath;
    private float lastAttackTime;
    private bool isWakingUp;
    private bool isAttacking = false;
    private bool wasMoving = false;
    private bool isTurning = false;
    private PlayerValues playerValues;
    private float lastDirectionChangeTime;
    private int currentFacingDirection = 1; // 1 = right, -1 = left

    // Animation state names based on your Animator
    private const string STATE_SLEEP = "Sleep";
    private const string STATE_WAKEUP = "WakeUp";
    private const string STATE_IDLE = "Idle";
    private const string STATE_WALK = "Walk";
    private const string STATE_ATTACK = "Attack";
    private const string STATE_DAMAGE = "Damage";
    private const string STATE_DEATH = "Death";
    private const string STATE_SHUTDOWN = "ShutDown";

    protected override void Start()
    {
        base.Start();
        InitializeRobotut();
        currentFacingDirection = Mathf.RoundToInt(transform.localScale.x);
    }

    private void InitializeRobotut()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if(target != null) playerValues = target.GetComponent<PlayerValues>();
    
        // Set these to match your actual animator state names
        deathAnimationName = "Dead"; // Parameter name for triggering death
        attackAnimationName = "Attack"; // Parameter name for triggering attack
        moveAnimationName = "Walking"; // Parameter name for walk state
    }

    protected override void Update()
    {
        if (isDead) return;
    
        base.Update();
        UpdateDirection();

        if (!targetInSight) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        bool isInAttackRange = distanceToPlayer <= attackRange;
        bool shouldMove = !isInAttackRange && canMove && !isAttacking && !isTurning;

        // Only change animation state when the movement state CHANGES
        if (shouldMove && !wasMoving)
        {
            animator.Play(STATE_WALK);
            wasMoving = true;
        }
        else if (!shouldMove && wasMoving)
        {
            animator.Play(STATE_IDLE);
            wasMoving = false;
        }

        // Handle movement based on shouldMove flag
        if (shouldMove)
        {
            float direction = Mathf.Sign(target.transform.position.x - transform.position.x);
            Move(direction);
        }
        else
        {
            // Stop movement by setting velocity to zero
            body.velocity = new Vector2(0, body.velocity.y);
            
            if (isInAttackRange && CanAttack() && !isTurning)
            {
                Attack();
            }
        }
    }

    private void UpdateDirection()
    {
        if (!targetInSight || target == null || isDead || isTurning) return;

        float targetDirection = Mathf.Sign(target.transform.position.x - transform.position.x);
        int newDirection = Mathf.RoundToInt(targetDirection);
        
        // Only change direction if it's different and enough time has passed
        if (newDirection != currentFacingDirection && !isTurning && Time.time - lastDirectionChangeTime >= turnDelay)
        {
            StartCoroutine(TurnTowardsPlayer(newDirection));
        }
    }

    private IEnumerator TurnTowardsPlayer(int newDirection)
    {
        isTurning = true;
        wasMoving = false;
        
        // Play idle animation during turning
        animator.Play(STATE_IDLE);
        
        // Wait for turn delay
        yield return new WaitForSeconds(turnDelay);
        
        // Update direction
        transform.localScale = new Vector3(-(2*newDirection), 2, 2);
        currentFacingDirection = newDirection;
        lastDirectionChangeTime = Time.time;
        
        isTurning = false;
    }

    public override void Attack()
    {
        if (CanAttack() && target != null && !isAttacking && !isTurning)
        {
            isAttacking = true;
            canMove = false; // Stop movement during attack
            wasMoving = false; // Reset movement state
            
            // Play Attack animation directly
            animator.Play(STATE_ATTACK);
            
            // Delay damage application until the right moment in the animation
            StartCoroutine(DealDamageDuringAttack());
            
            lastAttackTime = Time.time;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator DealDamageDuringAttack()
    {
        // Wait until we reach the damage point in the animation
        yield return new WaitForSeconds(attackDamageDelay);
        
        // Only deal damage if we're still in attack animation and facing the player
        if (IsInAnimationState(STATE_ATTACK) && IsFacingPlayer())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
            if (distanceToPlayer <= attackRange && playerValues != null)
            {
                playerValues.health -= 1;
            }
        }
    }

    private bool IsFacingPlayer()
    {
        if (target == null) return false;
        
        float directionToPlayer = Mathf.Sign(target.transform.position.x - transform.position.x);
        float facingDirection = -Mathf.Sign(transform.localScale.x);
        
        // Return true if the signs match (we're facing the player)
        return Mathf.Approximately(directionToPlayer, facingDirection);
    }

    private IEnumerator AttackSequence()
    {
        // Wait for attack animation to finish
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        canMove = true; // Re-enable movement after cooldown
        
        // Return to Idle after attack
        animator.Play(STATE_IDLE);
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    private IEnumerator WakeUpSequence()
    {
        isWakingUp = true;
        animator.Play(STATE_WAKEUP);
        yield return new WaitForSeconds(wakeDelay);
        targetInSight = true;
        canMove = true;
        isWakingUp = false;
        wasMoving = false; // Reset movement state
        animator.Play(STATE_IDLE);
    }

    public override void getHit(int damage)
    {
        if (isDead) return;

        lifepoints -= damage;
        
        if (lifepoints <= 0)
        {
            HandleDeath();
            MusicManager.Instance.PlayMusic("background");
        }
        else
        {
            // Reset movement state when taking damage
            wasMoving = false;
            isTurning = false;
            
            // Play damage animation
            animator.Play(STATE_DAMAGE);
            StartCoroutine(HandleStun());
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        wasMoving = false; // Reset movement state
        isTurning = false;
        
        // Play death animation
        animator.Play(STATE_DEATH);
        
        GetComponent<Collider2D>().enabled = false;
        canMove = false;
        body.velocity = Vector2.zero;

        // Destroy all specified objects
        foreach (GameObject obj in objectsToDestroyOnDeath)
        {
            if (obj != null)
                Destroy(obj); // Destroy each object immediately
        }

        Destroy(gameObject, 2f);
        player.GetComponent<PlayerSaveManager>().SavePlayerData(SceneManager.GetActiveScene().name);
        player.GetComponent<PlayerSaveManager>().SetBossDefeated("Robotut");
    }

    private IEnumerator HandleStun()
    {
        canMove = false;
        body.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunDuration);
        
        if (!isDead) 
        {
            canMove = true;
            animator.Play(STATE_IDLE);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !targetInSight && !isWakingUp)
        {
            MusicManager.Instance.PlayMusic("boss_fight");
            // Make sure we're checking the actual current state
            if (IsInAnimationState(STATE_SLEEP))
            {
                StartCoroutine(WakeUpSequence());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && targetInSight && !isDead)
        {
            StartCoroutine(ShutdownSequence());
        }
    }

    private IEnumerator ShutdownSequence()
    {
        // Reset movement state when shutting down
        wasMoving = false;
        isTurning = false;
        
        // Play shutdown animation
        animator.Play(STATE_SHUTDOWN);
        targetInSight = false;
        canMove = false;
        body.velocity = Vector2.zero;
        
        // Wait for shutdown animation to complete
        yield return new WaitForSeconds(0.5f);
        
        // Return to sleep state
        animator.Play(STATE_SLEEP);
    }

    // Check if currently in a specific animation state
    private bool IsInAnimationState(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    // Override Move to properly handle animations
    public override void Move(float direction)
    {
        if (canMove && !isAttacking && !isTurning)
        {
            // Call base Move only if we're allowed to move
            base.Move(direction);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}