using System.Collections;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float hitCooldown = 1f;
    
    private float currentHitCooldown = 0f;
    private bool playerDetected = false;
    private bool isAttacking = false;
    private bool isFlying = false;

    protected override void Start()
    {
        base.Start();
    
        // Initialize components
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
    
        // Setup animation names
        deathAnimationName = "Death";
        attackAnimationName = "Bite";
        moveAnimationName = "Fly";
    
        // Diagnostic check for Rigidbody2D
        if (body != null) {
            Debug.Log($"[Bat] Rigidbody2D initialized: Type={body.bodyType}, Gravity={body.gravityScale}");
        
            // Ensure appropriate settings for flying
            body.gravityScale = 0f;
            body.freezeRotation = true;
        } else {
            Debug.LogError("[Bat] No Rigidbody2D found!");
        }
    
        // Diagnostic check for Animator
        if (animator != null) {
            Debug.Log("[Bat] Animator initialized");
        } else {
            Debug.LogError("[Bat] No Animator found!");
        }
    }
    
    private void MoveTowardsPlayer()
    {
        if (target == null) return;
    
        Vector2 direction = (target.transform.position - transform.position).normalized;
    
        // Apply velocity directly
        body.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    
        // Debug the movement
        Debug.Log($"[Bat] Moving: Direction={direction}, Velocity={body.velocity}, Position={transform.position}");
    
        // Flip sprite based on movement direction
        transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
    }

    protected override void Update()
    {
        if (isDead) {
            Debug.Log("[Bat] Dead, not updating");
            return;
        }

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target == null) return; // Safety check
        }

        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        
        // Detection logic
        if (distanceToPlayer <= detectionRange && !playerDetected)
        {
            playerDetected = true;
            animator.SetBool("Detected", true);
            StartCoroutine(StartFlying());
        }

        // Movement logic when flying
        if (isFlying && !isAttacking)
        {
            MoveTowardsPlayer();
        }
        
        // Attack logic
        if (distanceToPlayer <= attackRange && !isAttacking && isFlying)
        {
            StartCoroutine(AttackPlayer());
        }
        
        // Cooldown logic
        if (currentHitCooldown < hitCooldown)
        {
            currentHitCooldown += Time.deltaTime;
        }
        
        if (isFlying) {
            Debug.Log($"[Bat] Flying state active, attacking={isAttacking}, velocity={body.velocity}");
        }
    }

    private IEnumerator StartFlying()
    {
        // First set Detected to true to trigger the first transition (bat → bat-idle-to-fly)
        animator.SetBool("Detected", true);
    
        // Wait for the transition to happen
        yield return new WaitForSeconds(0.1f);
    
        // Now set Fly to true to trigger the second transition (bat-idle-to-fly → bat fly)
        animator.SetBool("Fly", true);
    
        // Then reset Detected to prevent going back
        animator.SetBool("Detected", false);
    
        yield return new WaitForSeconds(0.1f);
    
        // Now we should be in flying state
        isFlying = true;
    
        Debug.Log("Bat should now be in flying state!");
    }
    

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        
        // Stop movement during attack
        body.velocity = Vector2.zero;

        // Use the animator trigger
        animator.SetTrigger(attackAnimationName);

        yield return new WaitForSeconds(0.5f);

        // Check if still in range after attack animation
        if (target != null && Vector2.Distance(transform.position, target.transform.position) <= attackRange)
        {
            // Get player component and damage it safely
            PlayerValues playerValues = target.GetComponent<PlayerValues>();
            if (playerValues != null)
            {
                playerValues.health -= 1;
            }
        }

        yield return new WaitForSeconds(0.5f); // Small cooldown after attack
        isAttacking = false;
    }

    // Override base class getHit method
    public override void getHit(int amount)
    {
        if (isDead) return;

        lifepoints -= amount;
        if (lifepoints <= 0)
        {
            // Use death logic and parameters matching state machine
            isDead = true;
            isFlying = false;
            body.velocity = Vector2.zero;
            
            animator.SetTrigger(deathAnimationName);
            GetComponent<Collider2D>().enabled = false;
            
            Destroy(gameObject, 1f);
        }
    }
    
    // Override these methods to completely disable the base behavior
    public override void Attack() { /* Empty to disable base behavior */ }
    public override void Move(float direction) { /* Empty to disable base behavior */ }
    public override void Trigger() { /* Empty to disable base behavior */ }
}