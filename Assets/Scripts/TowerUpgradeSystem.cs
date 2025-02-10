using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TowerUpgradeSystem : MonoBehaviour
{
    [Header("Upgrade Buttons")]
    [SerializeField] private Button speedButton;
    [SerializeField] private Button rangeButton;
    [SerializeField] private Button killDistanceButton;

    [Header("Upgrade Costs")]
    [SerializeField] private int speedUpgradeCost = 10;
    [SerializeField] private int rangeUpgradeCost = 15;
    [SerializeField] private int killDistanceUpgradeCost = 20;

    [Header("Upgrade Labels")]
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI killDistanceText;

    [Header("Turret Stats")]
    [SerializeField] private TurretBehavior turret; // Reference to the turret

    private int speedLevel = 1;
    private int rangeLevel = 1;
    private int killDistanceLevel = 1;
    private const int maxLevel = 5;

    private void Start()
    {
        // Add listeners for the buttons
        speedButton.onClick.AddListener(UpgradeSpeed);
        rangeButton.onClick.AddListener(UpgradeRange);
        killDistanceButton.onClick.AddListener(UpgradeKillDistance);

        // Initialize UI text
        UpdateUpgradeTexts();
    }

    private void Update()
    {
        // Dynamically update button interactability based on available gold and levels
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        bool canUpgradeSpeed = GameManager.Instance.gold >= speedUpgradeCost && speedLevel < maxLevel;
        bool canUpgradeRange = GameManager.Instance.gold >= rangeUpgradeCost && rangeLevel < maxLevel;
        bool canUpgradeKillDistance = GameManager.Instance.gold >= killDistanceUpgradeCost && killDistanceLevel < maxLevel;

        SetButtonState(speedButton, canUpgradeSpeed, speedLevel);
        SetButtonState(rangeButton, canUpgradeRange, rangeLevel);
        SetButtonState(killDistanceButton, canUpgradeKillDistance, killDistanceLevel);
    }

    private void SetButtonState(Button button, bool canUpgrade, int level)
    {
        button.interactable = canUpgrade;
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        if (level >= maxLevel)
        {
            buttonText.text = "Fully Upgraded"; // Change text when max level is reached
            button.interactable = false;
        }

        ColorBlock colors = button.colors;
        colors.normalColor = canUpgrade ? Color.white : Color.gray;
        colors.disabledColor = Color.gray;
        button.colors = colors;
    }

    private void UpgradeSpeed()
    {
        if (GameManager.Instance.gold >= speedUpgradeCost && speedLevel < maxLevel)
        {
            GameManager.Instance.gold -= speedUpgradeCost;
            turret.UpgradeSpeed(1);
            speedLevel++;
            UpdateUpgradeTexts();
            GameManager.Instance.UpdateUI();
        }
    }

    private void UpgradeRange()
    {
        if (GameManager.Instance.gold >= rangeUpgradeCost && rangeLevel < maxLevel)
        {
            GameManager.Instance.gold -= rangeUpgradeCost;
            turret.UpgradeRange(0.3f);
            rangeLevel++;
            UpdateUpgradeTexts();
            GameManager.Instance.UpdateUI();
        }
    }

    private void UpgradeKillDistance()
    {
        if (GameManager.Instance.gold >= killDistanceUpgradeCost && killDistanceLevel < maxLevel)
        {
            GameManager.Instance.gold -= killDistanceUpgradeCost;
            turret.UpgradeKillDistance(0.2f);
            killDistanceLevel++;
            UpdateUpgradeTexts();
            GameManager.Instance.UpdateUI();
        }
    }

    private void UpdateUpgradeTexts()
    {
        speedText.text = "Attack Speed Lvl. " + speedLevel;
        rangeText.text = "Tower Range Lvl. " + rangeLevel;
        killDistanceText.text = "Bullet Kill Distance Lvl. " + killDistanceLevel;
    }
}
