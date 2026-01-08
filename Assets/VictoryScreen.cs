using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Victory screen that appears when all apples are collected.
/// Displays a black background with a red victory message.
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

    void Start()
    {
        // Hide the victory screen initially
        HideVictoryScreen();
    }

    /// <summary>
    /// Shows the victory screen with the victory message.
    /// </summary>
    public void ShowVictoryScreen()
    {
        if (blackBackground != null)
        {
            blackBackground.gameObject.SetActive(true);
        }
        
        if (victoryText != null)
        {
            victoryText.text = victoryMessage;
            victoryText.gameObject.SetActive(true);
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
    }
}
