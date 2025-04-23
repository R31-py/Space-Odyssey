using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 10;
    [SerializeField] public float maxJumpHeight = 0;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private InventoryController inventoryController;
    
    [SerializeField] private GameObject slash_pfb;
    [SerializeField] private GameObject shuriken_pfb;
    [SerializeField] private GameObject shield_pfb;
    private GameObject activeShield;

    
    public Rigidbody2D body;
    private float horizontalInput;
    private BoxCollider2D boxCollider2D;
    private float wallJumpCD;
    private float direction;
    private float dashCD;
    private float dashTimer;
    public float jumpHeight = 0;
    private bool secondJump = false;
    private bool isAttacking = false;
    
    // Invincibility-Cooldown
    private float invCooldownTimer = 300f; 


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    
    private void Update()
    {
        if (!PlayerValues.isDead)
        { 
            bool isGroundedValue = isGrounded();
            transform.rotation = Quaternion.Euler(0, 0, 0);

            horizontalInput = Input.GetAxis("Horizontal");
            
            //INVINCIBILITY CHANGES
            // Cooldown hochzählen
            invCooldownTimer += Time.deltaTime;

            // Eingabe für Unverwundbarkeit
            if (Input.GetKeyDown(KeyCode.L) && invCooldownTimer >= 300f)
            {
            playerValues.ActivateInvincibility(5f);
            invCooldownTimer = 0f;
            }
            // DERI KTU
        if (horizontalInput != 0)
            direction = Mathf.Sign(horizontalInput);

        if (playerValues.tutorialStage >= 0 && horizontalInput != 0 && !isAttacking)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }

        if (wallJumpCD > 0.2f && !isAttacking)
        {
            body.velocity = new Vector2(horizontalInput * movementSpeed, body.velocity.y);

            if (onWall() && !isGroundedValue)
            {
                body.gravityScale = 2f;
                body.velocity = Vector2.zero;
            }
            else
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

                if (Input.GetKeyDown(playerValues.JUMP))
                {
                    SoundManager.Instance.PlaySound2D("player_jump");
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

        if (playerValues.tutorialStage >= 1 && !isAttacking)
        {
            if (Input.GetKey(KeyCode.E) && body.velocity.x != 0 && dashCD > 2f)
            {
                dashTimer = .2f;
                animator.SetTrigger("dashing");
            }
            else
            {
                dashCD += Time.deltaTime;
            }
        }

        if (dashTimer > 0 && !isAttacking)
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

        if (Input.GetKeyDown(playerValues.FIGHT) && !isAttacking)
        {
            StartCoroutine(Attack());
            SoundManager.Instance.PlaySound2D("player_attack");
        }
        
        Vector3 velocity = body.velocity;
        body.velocity = velocity;

        animator.SetBool("move", horizontalInput != 0 && !isAttacking);
        animator.SetBool("falling", !isGroundedValue && !isAttacking);
        body.angularVelocity = 0;

        if (Input.GetKeyDown(KeyCode.Z) && playerValues.Inventory[0] != null)
        {
            switch (playerValues.Inventory[0].abilityID) 
            {
                case 1:
                    SlashAbility();
                    break;
                case 2:
                    ShurikenAbility();
                    break;
                case 3:
                    ShieldAbility();
                    break;
            }
            
            inventoryController.Remove(0);
        }
        
        if (Input.GetKeyDown(KeyCode.X) && playerValues.Inventory[1] != null)
        {
            switch (playerValues.Inventory[1].abilityID) 
            {
                case 1:
                    SlashAbility();
                    break;
                case 2:
                    ShurikenAbility();
                    break;
                case 3:
                    ShieldAbility();
                    break;
            }
            
            inventoryController.Remove(1);
        }
        
        if (Input.GetKeyDown(KeyCode.C) && playerValues.Inventory[2] != null)
        {
            switch (playerValues.Inventory[2].abilityID) 
            {
                case 1:
                    SlashAbility();
                    break;
                case 2:
                    ShurikenAbility();
                    break;
                case 3:
                    ShieldAbility();
                    break;
            }
            
            inventoryController.Remove(2);
        }
        
        }
        
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        // Choose a random attack animation
        string attackAnim = UnityEngine.Random.Range(0, 2) == 0 ? "attack" : "attack2";
        animator.CrossFade(attackAnim, 0.1f);

        // Stop movement during attack
        body.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f); // Adjust based on animation length

        isAttacking = false;
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
            StartCoroutine(slideOnWall());

            wallJumpCD = 0;
        }
    }

    private void slash()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider2D.bounds.center,         
            boxCollider2D.bounds.size * 3f,
            0,                                   
            Vector2.right * direction,
            0.5f,
            enemyLayer
        );
        if (raycastHit.collider != null)
        {
            Enemy enemy = raycastHit.collider.GetComponent<Enemy>();
            if (enemy) enemy.getHit(1);
        }
    }

    private void dash()
    {
        body.velocity = new Vector2(movementSpeed * 2 * transform.localScale.x, body.velocity.y);
        dashCD = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size * 0.9f, 0, Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public void SlashAbility()
    {
        animator.SetTrigger("attack");
        SoundManager.Instance.PlaySound2D("slash_ability");
        Instantiate(slash_pfb, transform.position + new Vector3(0, -0.7f, 0), Quaternion.identity);
    }

    public void ShurikenAbility()
    {
        SoundManager.Instance.PlaySound2D("shuriken_ability");
        Instantiate(shuriken_pfb, transform.position + new Vector3(0, -0.7f, 0), Quaternion.identity);
    }

    void ShieldAbility()
    {
        SoundManager.Instance.PlaySound2D("shield-up_ability");
        activeShield = Instantiate(shield_pfb, transform.position + new Vector3(0, -0.4f, 0), Quaternion.identity, transform);
        activeShield.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private IEnumerator slideOnWall()
    {
        animator.Play("wallSlide");
        
        yield return new WaitForSeconds(1f);
    }
 
}
