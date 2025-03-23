using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float maxJumpHeight = 0;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerValues playerValues;
    public Rigidbody2D body;
    private float horizontalInput;
    private float gravity = 6f;
    private BoxCollider2D boxCollider2D;
    private float wallJumpCD;
    private float direction;
    private float dashCD;
    private float dashTimer;
    private float jumpHeight = 0;
    private bool secondJump = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Debug.Log("Is Grounded: " + isGrounded());
        bool isGroundedValue = isGrounded();
        transform.rotation = Quaternion.Euler(0, 0, 0);

        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
            direction = Mathf.Sign(horizontalInput);

        if (playerValues.tutorialStage >= 0 && horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }

        if (wallJumpCD > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * movementSpeed, body.velocity.y);

            if (onWall() && !isGroundedValue)
            {
                if (body.gravityScale != 2f) body.gravityScale = 2f;
                body.velocity = Vector2.zero;
            }
            else if (body.gravityScale != 3)
            {
                body.gravityScale = 3;
            }

            if (playerValues.tutorialStage >= 1)
            {
                if (Input.GetKeyDown(playerValues.JUMP) && !secondJump && !isGroundedValue)
                {
                    jumpHeight = 10;
                    secondJump = true;
                    animator.SetTrigger("jump");
                }

                if (Input.GetKey(playerValues.JUMP))
                {
                    jump();
                }

                if (Input.GetKeyUp(playerValues.JUMP) && !isGroundedValue)
                {
                    jumpHeight = maxJumpHeight + 1;
                }
            }
        }
        else
        {
            wallJumpCD += Time.deltaTime;
        }

        if (playerValues.tutorialStage >= 1)
        {
            if (Input.GetKey(KeyCode.E) && body.velocity.x != 0 && dashCD > 0.7f)
            {
                dashTimer = 0.15f;
            }
            else
            {
                dashCD += Time.deltaTime;
            }
        }
        if (dashTimer > 0)
        {
            dash();
            dashTimer -= Time.deltaTime;
        }
        else
        {
            dashTimer = 0;
        }

        if (isGroundedValue)
        {
            jumpHeight = 0;
        }

        if (Input.GetKeyDown(playerValues.FIGHT) )
        {
            slash();
            animator.SetBool("attack", true);
        }
        
       
        Vector3 velocity = body.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -movementSpeed, movementSpeed);
        velocity.z = Mathf.Clamp(velocity.z, -movementSpeed, movementSpeed);
        body.velocity = velocity;

        animator.SetBool("move", horizontalInput != 0);
        animator.SetBool("falling", !isGroundedValue);
    }

    private void jump()
    {
        bool isGroundedValue = isGrounded();
        if (isGroundedValue)
        {
            jumpHeight = 0;
            secondJump = false;
        }

        if (jumpHeight < maxJumpHeight)
        {
            if (jumpHeight == 0)
                animator.SetTrigger("jump");
            jumpHeight += 1;
            body.velocity = new Vector2(body.velocity.x, jumpHeight < 10 ? 5 : 10);
        }
        else if (onWall() && !isGroundedValue)
        {
            float flipDirection = -Mathf.Sign(transform.localScale.x);
            transform.localScale = new Vector3(flipDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            body.velocity = horizontalInput == 0
                ? new Vector2(flipDirection * 10, 0)
                : new Vector2(flipDirection * 3, 6);

            wallJumpCD = 0;
        }
    }

    private void slash()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.right * direction, 0.1f, groundLayer);
        if (raycastHit.collider != null)
        {
            Enemy enemy = raycastHit.collider.GetComponent<Enemy>();
            if (enemy) enemy.lifepoints -= 1;
        }
    }

    private void dash()
    {
        body.velocity = new Vector2(movementSpeed * 4 * transform.localScale.x, body.velocity.y);
        dashCD = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        //RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        //return raycastHit.collider != null;
        return false;
    }
}