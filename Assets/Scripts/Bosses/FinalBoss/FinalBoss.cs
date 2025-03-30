using System.Collections;
using UnityEngine;

public class FinalBoss : Enemy
{
    [Header("Final Boss Settings")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float flySpeed = 3f;
    [SerializeField] private float shootInterval = 0.5f;
    [SerializeField] private int arrowsPerAttack = 5;
    [SerializeField] private float breathTime = 3f;
    [SerializeField] private GameObject arrowPrefab;

    [Header("Animation States/Parameters")]
    [SerializeField] private string idleState = "finalboss-idle";
    [SerializeField] private string prepAttackState = "finalboss-prep-attack";
    [SerializeField] private string attackState = "finalboss-attack";

    // Animator parameters
    private static readonly int DetectedParam = Animator.StringToHash("Detected");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    // Internals
    private Transform playerTransform;
    private bool isAttacking = false;
    private bool isBreathing = false;
    private Vector2 currentFlyDirection;   // For random movement around the room
    private float directionChangeTimer = 0f;
    private float directionChangeInterval = 2f;

    protected override void Start()
    {
        base.Start();
        
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        // Disable gravity for a flying boss
        if (body != null)
        {
            body.gravityScale = 0f;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Start in idle state
        animator.Play(idleState);
    }

    protected override void Update()
    {
        base.Update();
        if (isDead) return;
        if (playerTransform == null) return;  // No player? No action.

        // Check if player is within detection range
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool playerDetected = distanceToPlayer <= detectionRange && !isAttacking && !isBreathing;

        // Update animator param to go from idle → prep-attack or vice versa
        animator.SetBool(DetectedParam, playerDetected);

        // If we're not attacking or breathing, do random flying around
        if (!isAttacking && !isBreathing)
        {
            FlyAround();
        }
    }
    
    public void TriggerAttack()
    {
        // We set the Attack trigger, which transitions from prep-attack → attack
        animator.SetTrigger(AttackTrigger);
    }
    
    public void StartAttackCycle()
    {
        if (!isAttacking && !isDead)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    /// <summary>
    /// Coroutine that shoots multiple arrows toward the player,
    /// then takes a 3-second 'breath' in idle state.
    /// </summary>
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        // Optionally, stop flying movement during the actual Attack
        body.velocity = Vector2.zero;

        for (int i = 0; i < arrowsPerAttack; i++)
        {
            ShootArrowAtPlayer();
            yield return new WaitForSeconds(shootInterval);
        }

        // After finishing the arrows, go idle for breathTime
        isAttacking = false;
        isBreathing = true;
        animator.SetBool(DetectedParam, false);  // Returns to idle

        yield return new WaitForSeconds(breathTime);

        isBreathing = false;
    }
    private void ShootArrowAtPlayer()
    {
        if (arrowPrefab == null || playerTransform == null) return;

        Vector3 spawnPos = transform.position;
        GameObject arrowObj = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

        // Rotate arrow to face the player
        Vector2 direction = (playerTransform.position - spawnPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Give arrow a velocity
        Rigidbody2D arrowRb = arrowObj.GetComponent<Rigidbody2D>();
        if (arrowRb != null)
        {
            float arrowSpeed = 8f;
            arrowRb.velocity = direction * arrowSpeed;
        }
    }
    
    private void FlyAround()
    {
        // Change direction every few seconds
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= directionChangeInterval)
        {
            directionChangeTimer = 0f;
            // Pick a random direction to fly
            currentFlyDirection = Random.insideUnitCircle.normalized;
        }

        // Move boss
        body.velocity = currentFlyDirection * flySpeed;
    }
    
    public override void getHit(int damage)
    {
        if (isDead) return;

        lifepoints -= damage;
        if (lifepoints <= 0)
        {
            isDead = true;
            body.velocity = Vector2.zero;
            //animator.Play(\"finalboss-death\"); when boss dies
            Destroy(gameObject, 2f);
        }
    }
}
