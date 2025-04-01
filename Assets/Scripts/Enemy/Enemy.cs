using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cinemachine; // Add this for Cinemachine functionality
using Object = System.Object;

public class Enemy : MonoBehaviour, IEnemy
{
    public GameObject target;
    
    // Inactive until player is reprogrammed
    // public Values targetValues;
    
    public float moveSpeed;
    public float attackRange;
    public float triggerRange;
    public int lifepoints;
    public int dropmoney;
    protected bool isDead = false;
    
    public bool canMove = true;
    public bool targetInSight;

    public Rigidbody2D body;
    public Animator animator;
    private PlayerValues player;
    
    public String[] enemyLayer;
    public String deathAnimationName;
    public String attackAnimationName;
    public String moveAnimationName;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private float[] enemyBoundry = { 100,100 };
    public float startX;
    
    // Add Cinemachine Impulse Source component reference
    [SerializeField] private CinemachineImpulseSource impulseSource;
    
    // Camera shake parameters
    [SerializeField] private float hitShakeIntensity = 0.5f;
    [SerializeField] private float deathShakeIntensity = 1.0f;

    protected virtual void Start()
    {
        startX = transform.position.x;
        player = target.GetComponent<PlayerValues>();
        
        // Get or add the impulse source component
        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
            if (impulseSource == null)
            {
                impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
                // Set default impulse source settings
                impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 0.2f;
                impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.15f;
                impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.05f;
                impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.05f;
            }
        }
    }
    
    public virtual void Attack()
    {
        Debug.Log("Attacking");
    }

    public virtual void Move(float direction)
    {   
        // Corrected boundary check
        if (transform.position.x > startX - enemyBoundry[0] && transform.position.x < startX + enemyBoundry[1])
        {
            // If target is not close, move towards it
            if (direction != 0)
            {
                direction = Mathf.Sign(direction); // Avoid division by zero
                body.velocity = new Vector2(direction * moveSpeed, body.velocity.y);
                animator.SetBool(moveAnimationName, true);
            }
            else
            {
                animator.SetBool(moveAnimationName, false);
            }
        }
        else
        {
            // Reverse direction when out of bounds
            body.velocity = new Vector2(-Mathf.Sign(body.velocity.x) * moveSpeed, body.velocity.y);
        }
    }

    public virtual void Trigger()
    {
        Debug.Log("Trigger");
    }
    
    public Vector2 GetDirection(Transform target)
    {
        return target.position - transform.position;
    }
    public void OnPlayerEnterDetection()
    {
        // Handle player entering detection (e.g., wake up)
        if (!targetInSight)
        {
            targetInSight = true;
            // Additional logic (e.g., wake animation)
        }
    }

    public void OnPlayerExitDetection()
    {
        // Handle player exiting detection (e.g., shutdown)
        if (targetInSight)
        {
            targetInSight = false;
            // Additional logic (e.g., shutdown animation)
        }
    }
    public bool canSee(GameObject target)
    {
        int layerMask = ~LayerMask.GetMask(enemyLayer);
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, GetDirection(target.transform), triggerRange, layerMask);
        if (raycastHit.collider != null)
        {
            if (raycastHit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
        // Add to your canSee method
        Debug.DrawRay(transform.position, GetDirection(target.transform).normalized * triggerRange, Color.red, 0.1f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other == target)
            Trigger();   
    }

    public virtual void getHit(int amount)
    {
        // Prevent invalid hits
        if(isDead || amount <= 0) return;

        // Calculate remaining lives BEFORE deduction
        int remainingLives = lifepoints - amount;
        
        // Apply damage
        lifepoints = remainingLives;

        if (lifepoints <= 0)
        {
            isDead = true;
            ParticleSystem particlesInstance = Instantiate(deathParticles, transform.position, Quaternion.identity);
            particlesInstance.Play();
            animator.SetTrigger(deathAnimationName);
            player.money += dropmoney;
            Destroy(gameObject, 3f);
        }
    }
    

    protected virtual void Update()
    {
        if (isDead) return;

        if (canSee(target) && target != null && target.CompareTag("Player"))
        {
            targetInSight = true;
        }
        
        if (targetInSight && canMove)
        {
            Move((int)GetDirection(target.transform).x); 
        } 
    }
}