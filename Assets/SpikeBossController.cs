using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpikeBossController : Enemy
{
    public bool isAiming = false;
    public bool Deactivated = false;
    public float jumpForce = 8f; // Power of the jump
    public float shootSpeed = 10f; // Speed when dashing down
    public float groundCheckDistance = 0.5f; // Distance to check for ground
    public LayerMask groundLayer; // The ground layer

    private bool isJumping = false;
    private bool hasShot = false;
    private bool hitPlayer = false;

    private void Update()
    {
        if (Deactivated) return; // If stunned, do nothing

        if (canSee(target) && !isJumping) // If sees the target and isn't jumping
        {
            StartCoroutine(JumpAttack());
        }

        if (isJumping && transform.position.y >= 12.5f) // When reaching peak height
        {
            isAiming = true;
            aim(target.transform);

            if (!hasShot)
            {
                StartCoroutine(DashDownward());
            }
        }

        // Check if landed
        if (isJumping && IsGrounded() || hitPlayer)
        {
            StartCoroutine(RecoverAfterLanding());
            hitPlayer = false;
        }
        
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, 89, 106), 
            transform.position.y, 
            transform.position.z
        );

        if (lifepoints <= 0)
        {
            MusicManager.Instance.PlayMusic("background");
        }
    }

    private IEnumerator JumpAttack()
    {
        isJumping = true;
        animator.SetTrigger("Jump");

        // Jump upwards
        body.velocity = new Vector2(Random.Range(-2, 2f), jumpForce);
        
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator DashDownward()
    {
        hasShot = true;
        yield return new WaitForSeconds(1f); // Small delay before dashing

        if (isAiming)
        {
            animator.SetTrigger("Dash");

            // Dash downward at aimed angle
            Vector2 dashDirection = transform.right * -1; // Move downward
            body.velocity = dashDirection * shootSpeed;
        }
    }

    private IEnumerator RecoverAfterLanding()
    {
        isJumping = false;
        isAiming = false;
        hasShot = false;

        yield return new WaitForSeconds(2f); // Pause before jumping again
    }

    public void aim(Transform target)
    {
        Vector3 targetPos = target.position;
        Vector3 thisPos = transform.position;
        targetPos.x -= thisPos.x;
        targetPos.y -= thisPos.y;
        float angle = -Mathf.Atan2(targetPos.y, -targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == target)
        {
            target.GetComponent<PlayerValues>().health -= 1;
            hitPlayer = true;
        }
    }
    public override void Trigger()
    {
        MusicManager.Instance.PlayMusic("boss_fight");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger();
            Debug.Log($"{gameObject.name} detected the player.");
        }
    }
}
