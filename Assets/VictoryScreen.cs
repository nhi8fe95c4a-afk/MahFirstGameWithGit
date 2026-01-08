using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Victory screen that appears when all apples are collected.
/// Displays a black background with a red victory message.
/// Fades in from alpha and pauses the game.
/// </summary>
public class VictoryScreen : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The panel that contains the black background")]
    public Image blackBackground;
    
    [Tooltip("The text component that displays the victory message")]
    public Text victoryText;

    [Header("Victory Settings")]
    [Tooltip("The victory message to display")]
    public string victoryMessage = "Вы собрали все яблоки. Теперь вы довольны?";
    
    [Tooltip("Duration of the fade-in animation in seconds")]
    public float fadeInDuration = 1.0f;

    void Start()
    {
        // Hide the victory screen initially
        HideVictoryScreen();
        
        // Ensure the canvas is set to render on top of everything
        EnsureCanvasSetup();
    }

    /// <summary>
    /// Ensures the Canvas is properly configured to render on top of the game.
    /// </summary>
    private void EnsureCanvasSetup()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            // Set to Screen Space - Overlay to render directly in front of the camera
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // Set high sort order to ensure it renders on top
            canvas.sortingOrder = 100;
        }
    }

    /// <summary>
    /// Shows the victory screen with the victory message.
    /// Fades in from alpha and pauses the game.
    /// </summary>
    public void ShowVictoryScreen()
    {
        // Pause the game
        Time.timeScale = 0f;
        
        if (blackBackground != null)
        {
            blackBackground.gameObject.SetActive(true);
        }
        
        if (victoryText != null)
        {
            victoryText.text = victoryMessage;
            victoryText.gameObject.SetActive(true);
        }
        
        // Start the fade-in animation
        StartCoroutine(FadeInVictoryScreen());
    }

    /// <summary>
    /// Coroutine that fades in the victory screen from alpha 0 to 1.
    /// </summary>
    private IEnumerator FadeInVictoryScreen()
    {
        float elapsedTime = 0f;
        
        // Start with alpha at 0
        if (blackBackground != null)
        {
            Color bgColor = blackBackground.color;
            bgColor.a = 0f;
            blackBackground.color = bgColor;
        }
        
        if (victoryText != null)
        {
            Color textColor = victoryText.color;
            textColor.a = 0f;
            victoryText.color = textColor;
        }
        
        // Fade in over time (using unscaled time since the game is paused)
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            
            // Update alpha for background
            if (blackBackground != null)
            {
                Color bgColor = blackBackground.color;
                bgColor.a = alpha;
                blackBackground.color = bgColor;
            }
            
            // Update alpha for text
            if (victoryText != null)
            {
                Color textColor = victoryText.color;
                textColor.a = alpha;
                victoryText.color = textColor;
            }
            
            yield return null;
        }
        
        // Ensure final alpha is exactly 1
        if (blackBackground != null)
        {
            Color bgColor = blackBackground.color;
            bgColor.a = 1f;
            blackBackground.color = bgColor;
        }
        
        if (victoryText != null)
        {
            Color textColor = victoryText.color;
            textColor.a = 1f;
            victoryText.color = textColor;
        }
    }

    /// <summary>
    /// Hides the victory screen.
    /// </summary>
    public void HideVictoryScreen()
    {
        if (blackBackground != null)
        {
            blackBackground.gameObject.SetActive(false);
        }
        
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }
        
        // Unpause the game
        Time.timeScale = 1f;
    }
}
