using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform quinticSpawn;  // The spawn point where the enemy will appear before moving towards the target
    public Transform targetLocation;
    public float quinticSpawnInterval = 6f;

    private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnQuinticEnemies());
    }

    private void Update()
    {
        if (GameManager.Instance.playerHP <= 0) StopSpawning();
    }

    IEnumerator SpawnQuinticEnemies()
    {
        while (isSpawning)
        {
            SpawnEnemy(quinticSpawn, MovementType.DoubleCubic);  // Spawn an enemy at the quintic spawn point
            yield return new WaitForSeconds(quinticSpawnInterval);
        }
    }

    void SpawnEnemy(Transform spawnPoint, MovementType moveType)
{
    if (!isSpawning || enemyPrefab == null || spawnPoint == null || targetLocation == null)
        return;

    // Instantiate the enemy at the spawn point's position
    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

    // Access the EnemyBehavior component on the spawned enemy
    EnemyBehavior movement = enemy.GetComponent<EnemyBehavior>();

    if (movement != null)
    {
        // Initialize the enemy's movement
        movement.Initialize(targetLocation, moveType);
    }
    else
    {
        Debug.LogWarning("EnemyBehavior component missing on spawned enemy!");
    }
}

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
}
