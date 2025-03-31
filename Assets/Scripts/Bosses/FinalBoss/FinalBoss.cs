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
    private Vector2 currentFlyDirection;
    private float directionChangeTimer = 0f;
    private float directionChangeInterval = 2f;
    private Bounds flyBounds; // Movement boundary

    protected override void Start()
    {
        base.Start();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        
        if (body != null)
        {
            body.gravityScale = 0f;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        CapsuleCollider2D boundary = GetComponent<CapsuleCollider2D>();
        if (boundary != null)
        {
            flyBounds = boundary.bounds;
            boundary.isTrigger = true;
        }

        // Set a random starting direction
        currentFlyDirection = Random.insideUnitCircle.normalized;

        // Start in idle state
        animator.Play(idleState);
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || playerTransform == null) return;

        // Check if player is within detection range
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool playerDetected = distanceToPlayer <= detectionRange && !isAttacking && !isBreathing;

        // Update animator param
        animator.SetBool(DetectedParam, playerDetected);

        if (playerDetected)
        {
            StartAttackCycle();
        }
        else
        {
            FlyAround();
        }
    }
    
    public void StartAttackCycle()
    {
        if (!isAttacking && !isDead)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        body.velocity = Vector2.zero;

        for (int i = 0; i < arrowsPerAttack; i++)
        {
            ShootArrowAtPlayer();
            yield return new WaitForSeconds(shootInterval);
        }

        isAttacking = false;
        isBreathing = true;
        animator.SetBool(DetectedParam, false);

        yield return new WaitForSeconds(breathTime);

        isBreathing = false;
    }

    private void ShootArrowAtPlayer()
    {
        if (arrowPrefab == null || playerTransform == null) return;

        Vector3 spawnPos = transform.position;
        GameObject arrowObj = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

        Vector2 direction = (playerTransform.position - spawnPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Rigidbody2D arrowRb = arrowObj.GetComponent<Rigidbody2D>();
        if (arrowRb != null)
        {
            float arrowSpeed = 8f;
            arrowRb.velocity = direction * arrowSpeed;
        }
    }
    
    private void FlyAround()
    {
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= directionChangeInterval)
        {
            directionChangeTimer = 0f;
            currentFlyDirection = Random.insideUnitCircle.normalized;
        }

        Vector2 nextPosition = (Vector2)transform.position + (currentFlyDirection * flySpeed * Time.deltaTime);

        if (flyBounds.Contains(nextPosition))
        {
            transform.position = nextPosition;
        }
        else
        {
            currentFlyDirection = -currentFlyDirection;
        }
    }
    
    public override void getHit(int damage)
    {
        if (isDead) return;

        lifepoints -= damage;
        if (lifepoints <= 0)
        {
            isDead = true;
            body.velocity = Vector2.zero;
            Destroy(gameObject, 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(DetectedParam, true);
            StartAttackCycle();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(DetectedParam, false);
        }
    }
}
