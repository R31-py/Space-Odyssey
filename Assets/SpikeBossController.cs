using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using Update = Unity.VisualScripting.Update;

public class SpikeBossController : Enemy
{
    public bool isAiming = false;
    public bool Deactivated = false;
    // Start is called before the first frame update

    void Update()
    {
        
        animator.SetBool("Deactivated", Deactivated);
        
        if (isAiming)
        {
            aim(target.transform);
        }
        
        animator.SetBool("isAiming", isAiming);

    }

    void Attack(Vector3 target)
    {
        animator.SetTrigger("Attack");
        body.velocity = new Vector2((transform.position.x - target.x) * moveSpeed, (transform.position.y - target.y) * moveSpeed);
        
        // Check if it hits ground to stop
        // -------------------------------
        // Concuss the boss
        Deactivated = true;
    }
    
    public void aim(Transform target)
    {
        Vector3 targetPos = target.position;
        Vector3 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
