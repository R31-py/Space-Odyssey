using UnityEngine;

public class MuzzleController : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    
    [Header("Settings")]
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
    public float aimSmoothness = 5f;
    
    private float nextFireTime;
    private FlyingBot parentEnemy;
    private Transform target;
    
    void Start()
    {
        parentEnemy = GetComponentInParent<FlyingBot>();
    }
    
    void Update()
    {
        // Check conditions
        if (parentEnemy == null || !parentEnemy.isShooting || !parentEnemy.targetInSight) return;
        
        // Get target reference
        if (parentEnemy.target != null)
        {
            target = parentEnemy.target.transform;
        }
        
        if (target == null) return;
        
        // Flip muzzle when player is to the left of the bot
        FlipMuzzle();
        
        // Aim and shoot
        UpdateAim();
        HandleShooting();
    }
    
    private void FlipMuzzle()
    {
        // Determine if player is to the left of the bot
        bool isPlayerOnLeft = target.position.x < parentEnemy.transform.position.x;
    
        // Keep the same scale magnitude but change the sign
        Vector3 currentScale = transform.localScale;
        currentScale.x = Mathf.Abs(currentScale.x) * (isPlayerOnLeft ? -1 : 1);
        transform.localScale = currentScale;
    }
    
    private void UpdateAim()
    {
        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            aimSmoothness * Time.deltaTime
        );
    }
    
    private void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.right * bulletSpeed;
        }
    }
}