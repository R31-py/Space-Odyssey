using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float maxJumpHeight = 0;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerValues playerValues;
    private Rigidbody2D body;
    float horizontalInput;
    public float gravity = 6f;
    private BoxCollider2D boxCollider2D;
    private float wallJumpCD;
    private float direction;
    private float dashCD;
    private float dashTimer;
    private float jumpHeight = 0;
    private bool secondJump = false;
    
    // Get components
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update every frame
    private void Update()
    {
        bool isGroundedValue = isGrounded();
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Get input (Left, Right)
        horizontalInput = Input.GetAxis("Horizontal");
        direction = horizontalInput / Math.Abs(horizontalInput);
        // Sprite facing direction
        if (playerValues.tutorialStage >= 0)
        {
            if(horizontalInput != 0)
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }

        
        if(wallJumpCD > 0.2f)
        {
            // Move the player
            body.velocity = new Vector2(horizontalInput * movementSpeed, body.velocity.y);

            if(onWall() && !isGroundedValue)
            {
                body.gravityScale = 2f;
                body.velocity = Vector2.zero;
            }else
            {
                body.gravityScale = 3;
            }

            if (playerValues.tutorialStage >= 1)
            {
                if (Input.GetKeyDown(playerValues.JUMP) && !secondJump && !isGroundedValue)
                {
                    jumpHeight = 10;
                    secondJump = true;
                    animator.SetTrigger("Jump");
                }
            
                // Detect Spacebar to Jump
                if(Input.GetKey(playerValues.JUMP))
                {
                    jump();
                }

                if (Input.GetKeyUp(playerValues.JUMP))
                {
                    jumpHeight = maxJumpHeight + 1;
                }
            }
            
        }else
        {
            wallJumpCD += Time.deltaTime;
        }


        if (playerValues.tutorialStage >= 1)
        {
            if(Input.GetKey(KeyCode.E) && body.velocity.x != 0 && dashCD > 0.7f){
                dashTimer = 0.15f;
            }else{
                dashCD += Time.deltaTime;
            }
        }
        if(dashTimer > 0){
            dash();
            dashTimer -= Time.deltaTime;
        }else{
            dashTimer = 0;
        }

        if (isGroundedValue)
        {
            jumpHeight = 0;
        }

        if (Input.GetKey(playerValues.FIGHT))
        {
            slash();
        }

        // Update animation booleans
        animator.SetBool("isMoving", horizontalInput != 0);
        animator.SetBool("isGrounded", isGroundedValue);
    }

    // Jump function
    private void jump()
    {
     //   SoundManager.Instance.PlaySound2D("player_Jump");
        bool isGroundedValue = isGrounded();
        if (isGroundedValue)
        {
            jumpHeight = 0;
            secondJump = false;
        }

        if(jumpHeight < maxJumpHeight)
        {
            if(jumpHeight == 0)
                animator.SetTrigger("Jump");  
            jumpHeight += 1;
            if (jumpHeight < 10)
            {
                body.velocity = new Vector2(body.velocity.x, 5);
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x, 10);
            }
            
        }
        else if (onWall() && !isGroundedValue)
        {
            
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCD = 0;
        }
        
    }

    private void slash()
    {
        //SoundManager.Instance.PlaySound2D("player_Slash");
        animator.SetTrigger("attack");
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.right * direction, 0.1f, groundLayer);
        Enemy enemy = raycastHit.collider.gameObject.GetComponent<Enemy>();
        if (enemy) 
            enemy.lifepoints -= 1;
        
    }

    private void dash()
    {
        body.velocity = new Vector2(movementSpeed * 4 * transform.localScale.x, body.velocity.y);
        dashCD = 0;
    }
    

    // Detect collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private bool isGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
