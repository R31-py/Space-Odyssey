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

    private float nextFireTime;
    private FlyingBot parentEnemy;
    private Transform target;

    void Start()
    {
        parentEnemy = GetComponentInParent<FlyingBot>();
        
        if (parentEnemy != null)
        {
            target = parentEnemy.target?.transform;
        }
    }

    void Update()
    {
        if (parentEnemy == null || target == null || !parentEnemy.targetInSight) return;

        FaceTarget();
        HandleShooting();
    }

    void FaceTarget()
    {
        Vector2 direction = target.position - transform.position;
        direction.x *= parentEnemy.FacingDirection; // Account for flip
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }

    void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 shootDirection = firePoint.right * parentEnemy.FacingDirection;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // Set up the bullet with the required properties
        if (bullet.TryGetComponent(out Bullet enemyBullet))
        {
            enemyBullet.Initialize(shootDirection, bulletSpeed);
        }
    }
}