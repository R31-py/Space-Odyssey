using UnityEngine;

public class MuzzleController : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Settings")]
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
    public float rotationSpeed = 5f;
    public float maxAimAngle = 60f; // Now properly used for clamping

    [Header("Aiming")]
    [SerializeField] private float aimSmoothness = 5f;
    [SerializeField] private float shootingAngleThreshold = 10f;

    private float nextFireTime;
    private FlyingBot parentEnemy;
    private Transform target;

    void Start()
    {
        parentEnemy = GetComponentInParent<FlyingBot>();
        if (parentEnemy != null && parentEnemy.target != null)
        {
            target = parentEnemy.target.transform;
        }
    }

    void Update()
    {
        if (ShouldShoot())
        {
            UpdateTargetReference();
            UpdateAim();
            HandleShooting();
        }
        else
        {
            ResetAim();
        }
    }

    private bool ShouldShoot()
    {
        return parentEnemy != null && 
               parentEnemy.targetInSight && 
               parentEnemy.isShooting && 
               target != null;
    }

    private void UpdateTargetReference()
    {
        if (target == null && parentEnemy.target != null)
        {
            target = parentEnemy.target.transform;
        }
    }

    private void UpdateAim()
    {
        Vector2 directionToTarget = GetAimDirection();
        RotateTowards(directionToTarget);
    }

    private Vector2 GetAimDirection()
    {
        // Fixed: Removed incorrect direction flipping
        return (Vector2)target.position - (Vector2)transform.position;
    }

    private void RotateTowards(Vector2 direction)
    {
        float desiredAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Calculate default angle based on parent's facing
        float defaultAngle = parentEnemy.FacingDirection > 0 ? 0f : 180f;
        
        // Clamp angle within allowed range
        float angleDifference = Mathf.DeltaAngle(defaultAngle, desiredAngle);
        float clampedAngle = defaultAngle + Mathf.Clamp(angleDifference, -maxAimAngle, maxAimAngle);
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, clampedAngle);
        
        // Use spherical interpolation for smoother rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            aimSmoothness * Time.deltaTime
        );
    }

    private void ResetAim()
    {
        if (parentEnemy == null) return;

        // Calculate default direction based on current facing
        float targetAngle = parentEnemy.FacingDirection > 0 ? 0f : 180f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            aimSmoothness * Time.deltaTime
        );
    }

    private void HandleShooting()
    {
        if (Time.time >= nextFireTime && IsAimedAtTarget())
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private bool IsAimedAtTarget()
    {
        if (target == null) return false;

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float angleDifference = Vector2.Angle(transform.right, directionToTarget);

        return angleDifference < shootingAngleThreshold;
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Vector2 shootDirection = transform.right;

        if (bullet.TryGetComponent(out Bullet enemyBullet))
        {
            enemyBullet.Initialize(shootDirection, bulletSpeed);
        }
        else
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = shootDirection * bulletSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePoint.position, firePoint.position + (Vector3)transform.right * 2);
        }
    }
}