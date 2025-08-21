using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Central controller for the sorting game. Handles spawning items, tracking the game timer,
/// updating the score and combo, and evaluating player throws.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Round Settings")]
    [Tooltip("Duration of the round in seconds.")]
    public float roundLength = 60f;

    [Tooltip("Spawn rate at the start of the round (items per second).")]
    public float startSpawnRate = 0.8f;

    [Tooltip("Spawn rate at the end of the round (items per second).")]
    public float endSpawnRate = 2.2f;

    [Tooltip("Speed of items at the start of the round (units per second).")]
    public float beltSpeedStart = 2.5f;

    [Tooltip("Speed of items at the end of the round (units per second).")]
    public float beltSpeedEnd = 4.0f;

    [Header("Prefabs & References")]
    [Tooltip("List of item prefabs to spawn. Each prefab should have a TrashItem script with its itemType set.")]
    public List<GameObject> itemPrefabs;

    [Tooltip("Transform that defines where new items are spawned along the belt.")]
    public Transform spawnPoint;

    [Tooltip("UI text used to display the player's current score.")]
    public Text scoreText;

    [Tooltip("UI text used to display the remaining time in seconds.")]
    public Text timerText;

    [Tooltip("UI text used to display the current combo multiplier.")]
    public Text comboText;

    // Internal game state
    private float remainingTime;
    private float spawnAccumulator;
    private int score;
    private int comboCount;
    private float comboMultiplier = 1f;
    private bool gameRunning;

    private void Start()
    {
        ResetGame();
    }

    /// <summary>
    /// Resets internal state and begins the round.
    /// </summary>
    public void ResetGame()
    {
        remainingTime = roundLength;
        spawnAccumulator = 0f;
        score = 0;
        comboCount = 0;
        comboMultiplier = 1f;
        gameRunning = true;
        UpdateUI();
    }

    private void Update()
    {
        if (!gameRunning)
            return;

        // Update timer
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            gameRunning = false;
            UpdateUI();
            // TODO: Trigger end-of-round UI here (optional)
            return;
        }

        // Update spawn logic
        float progress = 1f - (remainingTime / roundLength); // 0 at start, 1 at end
        float currentSpawnRate = Mathf.Lerp(startSpawnRate, endSpawnRate, progress);
        float currentSpeed = Mathf.Lerp(beltSpeedStart, beltSpeedEnd, progress);

        spawnAccumulator += Time.deltaTime * currentSpawnRate;
        if (spawnAccumulator >= 1f)
        {
            spawnAccumulator -= 1f;
            SpawnItem(currentSpeed);
        }

        // Update UI timer each frame
        UpdateTimerUI();
    }

    /// <summary>
    /// Spawns a new trash item at the spawn point with the given belt speed.
    /// </summary>
    private void SpawnItem(float beltSpeed)
    {
        if (itemPrefabs == null || itemPrefabs.Count == 0)
            return;

        int index = Random.Range(0, itemPrefabs.Count);
        GameObject prefab = itemPrefabs[index];
        if (prefab == null)
            return;

        GameObject instance = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        TrashItem trash = instance.GetComponent<TrashItem>();
        if (trash != null)
        {
            trash.SetSpeed(beltSpeed);
        }
    }

    /// <summary>
    /// Evaluates a throw made by the player. Increases or decreases the score and combo based on whether
    /// the item was flicked into the correct bin for its type.
    /// </summary>
    public void EvaluateThrow(TrashItem item, ThrowDirection direction)
    {
        if (!gameRunning || item == null)
            return;

        bool correct = false;
        switch (direction)
        {
            case ThrowDirection.Left:
                correct = item.itemType == ItemType.Organic;
                break;
            case ThrowDirection.Up:
                correct = item.itemType == ItemType.Paper;
                break;
            case ThrowDirection.Right:
                correct = item.itemType == ItemType.Plastic;
                break;
        }

        if (correct)
        {
            comboCount++;
            // Combo multiplier scales up to 2.0 (e.g. 1 + 10 combo * 0.1 = 2)
            comboMultiplier = Mathf.Min(2f, 1f + comboCount * 0.1f);
            int points = Mathf.RoundToInt(10f * comboMultiplier);
            score += points;
        }
        else
        {
            // Incorrect sort: immediate penalty and combo reset
            score -= 10;
            comboCount = 0;
            comboMultiplier = 1f;
        }

        // Destroy the sorted item
        Destroy(item.gameObject);

        UpdateUI();
    }

    /// <summary>
    /// Called when an item leaves the belt without being sorted. Applies a smaller penalty but does not reset the combo.
    /// </summary>
    public void MissedItem(TrashItem item)
    {
        if (!gameRunning)
        {
            Destroy(item.gameObject);
            return;
        }
        score -= 5;
        Destroy(item.gameObject);
        UpdateUI();
    }

    /// <summary>
    /// Called when the player performs a swipe without any item present. Applies a small penalty.
    /// </summary>
    public void RegisterMissWithoutItem()
    {
        if (!gameRunning)
            return;
        score -= 5;
        UpdateUI();
    }

    /// <summary>
    /// Updates all UI text elements based on the current score and combo.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        if (comboText != null)
        {
            comboText.text = $"×{comboMultiplier:F1}";
        }
        UpdateTimerUI();
    }

    /// <summary>
    /// Updates the timer text based on the remaining time.
    /// </summary>
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        }
    }
}