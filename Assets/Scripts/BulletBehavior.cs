using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 10f;  // Speed at which the bullet moves
    public float damage = 1f;  // The damage the bullet does to enemies
    public bool isHoming = false;  // Is the bullet homing towards a target?
    public Transform target;  // The target for homing bullets (enemy)

    
    void Start()
    {
        if (isHoming && target == null)
        {
            target = GameObject.FindWithTag("Player").transform; // Find the player if not assigned
        }
    }

    void Update()
    {
        MoveBullet();  // Call the method to move the bullet
    }

    // Handles bullet movement, including homing functionality
    void MoveBullet()
    {
        if (isHoming && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;  // Calculate direction to target
            transform.Translate(direction * speed * Time.deltaTime);  // Move the bullet towards the target
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);  // Regular movement in a straight line
        }
    }
    public void Initialize(Vector2 direction, float speed, Transform target, float killDistance)
    {
        this.speed = speed;
        this.target = target;
        isHoming = target != null;
        
        // Move in the given direction if not homing
        if (!isHoming)
        {
            GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehavior>().TakeDamage(damage);  // Damage the enemy
            Destroy(gameObject);  // Destroy the bullet after hitting the enemy
            SpawnGold();  // Spawn gold after killing the enemy
        }
    }

    // Spawns gold when the bullet hits an enemy
    void SpawnGold()
    {
        Gold goldInstance = Instantiate(goldPrefab, transform.position, Quaternion.identity); 
        goldInstance.Initialize(uiTarget, amount);
        goldInstance.StartMoving();
    }
}
