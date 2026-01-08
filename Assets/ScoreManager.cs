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

    private int currentScore = 0;

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
            // Format score with leading zero (00, 01, 02, etc.)
            scoreText.text = currentScore.ToString("D2");
        }
    }
}
