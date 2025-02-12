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
    
    
    public void Attack()
    {
        Debug.Log("Attacking");
    }

    public void Move(int direction)
    {
         // If target not close go to him
        if (direction != 0)
        {
            // -1 or 1
            direction = Mathf.Abs(direction) / direction;
            body.velocity = new Vector2(direction * moveSpeed, body.velocity.y);  
            animator.SetBool(moveAnimationName, true);
        }
        else
        {
            animator.SetBool(moveAnimationName, false);
        }
    }

    public void Trigger()
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
        if(other.tag == target.tag && targetInSight)
            Trigger();   
    }

    private void Update()
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
