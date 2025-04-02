using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FinalBoss : Enemy
{
    [Header("Final Boss Settings")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float flySpeed = 3f;
    [SerializeField] private float attackFlySpeed = 5f;  
    [SerializeField] private float attackDuration = 3f;     
    [SerializeField] private float breathTime = 3f;  
    [SerializeField] private float arrowInterval = 1f; // Time between arrow shots during attack      

    [Header("Animation States/Parameters")]
    [SerializeField] private string idleState = "finalboss-idle";
    [SerializeField] private string prepAttackState = "finalboss-prep-attack";
    [SerializeField] private string attackState = "finalboss-attack";

    private static readonly int DetectedParam = Animator.StringToHash("Detected");

    [Header("Movement Boundary")]
    [SerializeField] private BoxCollider2D movementBoundary;

    [Header("Boss Fight Music")]
    [SerializeField] private AudioClip bossFightMusic;
    
    [Header("Projectile Settings")]
    [SerializeField] private GameObject arrowPrefab;

    private Transform playerTransform;
    private bool isAttacking = false;
    private bool isBreathing = false;
    private Vector2 currentFlyDirection;
    private float directionChangeTimer = 0f;
    private float directionChangeInterval = 2f;
    private Bounds flyBounds;
    private AudioSource audioSource;

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

        if (movementBoundary != null)
        {
            flyBounds = movementBoundary.bounds;
        }
        
        currentFlyDirection = Random.insideUnitCircle.normalized;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        animator.Play(idleState);
    }

    protected override void Update()
    {
        // Clamp Y position to keep the boss in a specific vertical range.
        transform.position = new Vector3(transform.position.x, math.clamp(transform.position.y, 17, 31), transform.position.z);
        base.Update();
        if (isDead || playerTransform == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool playerDetected = distanceToPlayer <= detectionRange && !isAttacking && !isBreathing;
        
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
        animator.Play(attackState);
        
        float attackTimer = 0f;
        float arrowTimer = 0f;
        
        while (attackTimer < attackDuration)
        {
            // Update flying direction
            directionChangeTimer += Time.deltaTime;
            if (directionChangeTimer >= directionChangeInterval)
            {
                directionChangeTimer = 0f;
                currentFlyDirection = Random.insideUnitCircle.normalized;
            }
            
            // Move boss within flyBounds
            Vector2 nextPosition = (Vector2)transform.position + (currentFlyDirection * attackFlySpeed * Time.deltaTime);
            Vector3 testPos = new Vector3(nextPosition.x, nextPosition.y, flyBounds.center.z);
            if (flyBounds.Contains(testPos))
            {
                transform.position = nextPosition;
            }
            else
            {
                currentFlyDirection = -currentFlyDirection;
            }
            
            // Shoot arrows at fixed intervals
            arrowTimer += Time.deltaTime;
            if (arrowTimer >= arrowInterval)
            {
                arrowTimer = 0f;
                ShootArrow();
            }
            
            attackTimer += Time.deltaTime;
            yield return null;
        }
        
        isAttacking = false;
        isBreathing = true;
        animator.Play(idleState);
        body.velocity = Vector2.zero;
        yield return new WaitForSeconds(breathTime);
        isBreathing = false;
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
        Vector3 testPos = new Vector3(nextPosition.x, nextPosition.y, flyBounds.center.z);
        if (flyBounds.Contains(testPos))
        {
            transform.position = nextPosition;
        }
        else
        {
            currentFlyDirection = -currentFlyDirection;
        }
    }
    
    private void ShootArrow()
    {
        if (arrowPrefab == null || playerTransform == null) return;
        
        // Spawn the arrow at the boss's position.
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        
        // Calculate direction toward the player.
        Vector2 direction = (new Vector2(playerTransform.position.x, playerTransform.position.y) - 
                             new Vector2(transform.position.x, transform.position.y)).normalized;

        
        // Rotate the arrow to face the player.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // Set the arrow's movement direction.
        FinalBossArrow arrowScript = arrow.GetComponent<FinalBossArrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDirection(direction);
        }
    }
    
    public override void getHit(int damage)
    {
        if (isDead) return;

        lifepoints -= damage;
        Debug.Log("FinalBoss getHit called: damage = " + damage + ", remaining health = " + lifepoints);
        if (lifepoints <= 0)
        {
            isDead = true;
            body.velocity = Vector2.zero;
            // Optionally, play death animation here
            Destroy(gameObject, 2f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(DetectedParam, true);
            StartAttackCycle();
            
            if (bossFightMusic != null && !audioSource.isPlaying)
            {
                audioSource.clip = bossFightMusic;
                audioSource.Play();
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(DetectedParam, false);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
