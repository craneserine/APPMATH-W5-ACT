using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Player stats
    public int playerHP = 5;
    public int gold = 0;

    // UI elements
    public TextMeshProUGUI hpText;  // TextMeshPro component for displaying player's HP
    public TextMeshProUGUI goldText; // TextMeshPro component for displaying gold
    public GameObject failUI;  // Reference to the fail UI GameObject
    public GameObject gameOverUI; // Game Over UI
    public GameObject shopUI; // Shop UI
    public GameObject goldPrefab; // Prefab for gold collection animation

    private bool isPaused = false;
    private bool isAnimating = false; // Prevents toggling during animation
    private bool isGameOver = false; // Flag to check if game is over
    public EnemySpawner enemySpawner; // Reference to the enemy spawner

    void Awake()
    {
        // Singleton pattern to access GameManager from anywhere
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        // Ensure the text fields are assigned in the Inspector
        if (hpText == null || goldText == null)
        {
            Debug.LogError("Text fields not assigned in the Inspector.");
        }

        shopUI.transform.localScale = Vector3.zero; // Start with UI hidden
        shopUI.SetActive(false);
    }

    void Start()
    {
        // Ensure the enemy spawner is initialized
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        UpdateUI();
    }

    // Update the HP and Gold text UI
    public void UpdateUI()
    {
        hpText.text = "HP: " + playerHP.ToString();
        goldText.text = "Gold: " + gold.ToString();
    }

    // Handle player damage
    public void TakeDamage()
    {
        if (isGameOver) return; // Prevent taking damage if the game is over

        playerHP--;
        UpdateUI();
        if (playerHP <= 0)
        {
            GameOver();
        }
    }

    // Trigger the game over sequence
    private void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true); // Show game over UI
        Time.timeScale = 0f; // Pause the game
    }

    // Trigger the fail UI when the enemy reaches the end point
    public void ShowFailUI()
    {
        failUI.SetActive(true); // Show the fail UI
        Time.timeScale = 0f; // Pause the game
    }

    // Hide the fail UI
    public void HideFailUI()
    {
        failUI.SetActive(false); // Hide the fail UI
        Time.timeScale = 1f; // Resume the game
    }

    // Pause the game and show the shop UI
    public void PauseGame()
    {
        if (isAnimating || isGameOver) return; // Prevent toggling if the game is over or animating

        isPaused = !isPaused;

        if (isPaused)
        {
            StartCoroutine(AnimateUI(shopUI, Vector3.zero, Vector3.one, 0.3f, true)); // Scale up, then pause
        }
        else
        {
            StartCoroutine(AnimateUI(shopUI, Vector3.one, Vector3.zero, 0.3f, false)); // Scale down, then unpause
        }
    }

    // Animate the UI scale (for the shop UI)
    private IEnumerator AnimateUI(GameObject ui, Vector3 startScale, Vector3 endScale, float duration, bool pauseAfter)
    {
        isAnimating = true;
        ui.SetActive(true);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime; // Use unscaled time to keep animation smooth
            float t = TweenUtils.EaseOut(timeElapsed / duration);
            ui.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        ui.transform.localScale = endScale;
        isAnimating = false;

        if (pauseAfter)
        {
            Time.timeScale = 0f; // Pause the game after the animation completes
        }
        else
        {
            Time.timeScale = 1f; // Resume after closing animation
            ui.SetActive(false);
        }
    }

    // Add gold and update UI
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    // Spawn a gold UI animation
    public void SpawnGold(Vector3 position)
    {
        GameObject goldObject = Instantiate(goldPrefab, position, Quaternion.identity);
        // Add gold collection animation here (lerp towards gold counter)
    }
}
