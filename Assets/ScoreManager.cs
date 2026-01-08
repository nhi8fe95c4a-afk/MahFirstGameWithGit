using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the player's score and updates the UI score counter.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The Text component that displays the score")]
    public Text scoreText;

    [Header("Victory Settings")]
    [Tooltip("Total number of apples in the game")]
    public int totalApples = 20;
    
    [Tooltip("Reference to the victory screen")]
    public VictoryScreen victoryScreen;

    private int currentScore = 0;
    private bool victoryShown = false;

    void Start()
    {
        // Initialize score display
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Adds points to the current score.
    /// </summary>
    /// <param name="points">Number of points to add</param>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
        
        // Check if all apples are collected
        if (currentScore >= totalApples && !victoryShown)
        {
            ShowVictory();
        }
    }

    /// <summary>
    /// Gets the current score.
    /// </summary>
    public int GetScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Resets the score to zero.
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            // Format score with leading zeros (00-99, then 100+)
            // With 20 apples in the game, D2 format is sufficient
            scoreText.text = currentScore.ToString("D2");
        }
    }

    /// <summary>
    /// Shows the victory screen when all apples are collected.
    /// </summary>
    private void ShowVictory()
    {
        victoryShown = true;
        
        if (victoryScreen != null)
        {
            victoryScreen.ShowVictoryScreen();
        }
        else
        {
            Debug.LogWarning("VictoryScreen reference is not set in ScoreManager!");
        }
    }
}
