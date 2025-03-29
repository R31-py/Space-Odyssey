using UnityEngine;

public class Bot : MonoBehaviour
{
    [Header("Detection & Shooting")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float shootRange = 3f;
    [SerializeField] private float shootCooldown = 2f;
    
    [Header("Bot Stats")]
    [SerializeField] private int health = 3;

    [Header("References")]
    [SerializeField] private PlayerValues player; //player script with a health property
    private Transform playerTransform;

    private Animator animator;
    private float currentShootTimer;
    private bool isDead = false;
    private bool isDetected = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isDead) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (!isDetected && distanceToPlayer <= detectionRange)
        {
            isDetected = true;
            animator.SetBool("Detected", true);
        }
        
        if (isDetected && distanceToPlayer <= shootRange)
        {
            currentShootTimer += Time.deltaTime;
            if (currentShootTimer >= shootCooldown)
            {
                animator.SetTrigger("Shoot");

                currentShootTimer = 0f;
            }
        }
    }
    
    public void ShootPlayer()
    {
        if (player != null)
        {
            player.health -= 1;
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            animator.SetTrigger("Death");
            
            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 1f);
        }
    }
}
