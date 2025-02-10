using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TurretBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float turretRange = 4f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 4f;
    [SerializeField] private float fireInterval = 0.7f;

    private float fireTimer = 0f;
    private float bulletKillDistance = 0.8f; 

    // Upgrade Methods
    public void UpgradeSpeed(float speedIncrease)
    {
        bulletSpeed += speedIncrease;
    }

    public void UpgradeRange(float rangeIncrease)
    {
        turretRange += rangeIncrease;
    }

    public void UpgradeKillDistance(float distanceIncrease)
    {
        bulletKillDistance += distanceIncrease;
    }

    void Update()
    {
        if (target == null || !IsTargetInRange())
        {
            FindTarget(); // Find a new target only if current one is invalid
        }

        if (target != null)
        {
            // Rotate turret towards target
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            fireTimer += Time.deltaTime;
            CheckRange();
        }
    }

    public void CheckRange()
    {
        if (target == null) return;

        float dx = transform.position.x - target.position.x;
        float dy = transform.position.y - target.position.y;
        float distanceSquared = dx * dx + dy * dy;

        // Compare squared distance to the square of the turret range
        if (distanceSquared <= turretRange * turretRange && fireTimer >= fireInterval)
        {
            FireBullet();
            fireTimer = 0f;
        }
    }

    public void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (target.position - transform.position).normalized;

        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.Initialize(direction, bulletSpeed, target, bulletKillDistance); // Pass kill distance
        }
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistanceSquared = turretRange * turretRange;

        foreach (GameObject enemy in enemies)
        {
            // Skip enemies that are too far away or already dead
            if (enemy == null) continue;

            float dx = transform.position.x - enemy.transform.position.x;
            float dy = transform.position.y - enemy.transform.position.y;
            float distanceSquared = dx * dx + dy * dy;

            // Update the target if a closer enemy is found
            if (distanceSquared <= closestDistanceSquared)
            {
                closestEnemy = enemy.transform;
                closestDistanceSquared = distanceSquared;
            }
        }

        target = closestEnemy;
    }

    private bool IsTargetInRange()
    {
        if (target == null) return false;

        float dx = transform.position.x - target.position.x;
        float dy = transform.position.y - target.position.y;
        float distanceSquared = dx * dx + dy * dy;

        return distanceSquared <= turretRange * turretRange;
    }
}
