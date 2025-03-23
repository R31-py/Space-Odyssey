using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class  Enemy : MonoBehaviour, IEnemy
{
    public GameObject target;
    
    // Inactive until player is reprogrammed
    // public Values targetValues;
    
    public float moveSpeed;
    public float attackRange;
    public float triggerRange;
    public int lifepoints;
    
    public bool canMove = true;
    public bool targetInSight;

    public Rigidbody2D body;
    public Animator animator;
    
    public String[] enemyLayer;
    public String deathAnimationName;
    public String attackAnimationName;
    public String moveAnimationName;
    public float[] enemyBoundry = { 100,100 };
    public float startX;

    public void Start()
    {
        startX=transform.position.x;
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
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other == target)
            Trigger();   
    }

    public void getHit(int amount)
    {
        lifepoints -= amount;
        if (lifepoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    private  void Update()
    {
        if (canSee(target) && !target.tag.Equals("Player"))
        {
            targetInSight = true;
        }
        
        if (targetInSight && canMove)
        {
            Move((int)GetDirection(target.transform).x); 
        } 
    }
}
