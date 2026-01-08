using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the dialogue UI window. Pauses the game when dialogue is shown.
/// </summary>
public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The dialogue panel that contains the text")]
    public GameObject dialoguePanel;
    
    [Tooltip("The Text component to display the dialogue")]
    public Text dialogueText;

    private bool isDialogueActive = false;
    private bool keyWasDown = false;

    void Start()
    {
        // Ensure dialogue is hidden at start
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    void Update()
    {
        CheckInput();
    }

    void OnGUI()
    {
        // OnGUI runs even when Time.timeScale = 0, so we check input here too
        if (isDialogueActive)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        // Manual key down detection that works even when paused
        bool keyIsDown = Input.GetKey(KeyCode.E);
        
        if (isDialogueActive && keyIsDown && !keyWasDown)
        {
            CloseDialogue();
        }
        
        keyWasDown = keyIsDown;
    }

    /// <summary>
    /// Shows the dialogue window with the specified text and pauses the game.
    /// </summary>
    /// <param name="text">The text to display in the dialogue</param>
    public void ShowDialogue(string text)
    {
        if (dialoguePanel == null || dialogueText == null)
            return;

        dialogueText.text = text;
        dialoguePanel.SetActive(true);
        isDialogueActive = true;
        
        // Mark key as already pressed to prevent immediate close
        keyWasDown = true;
        
        // Pause the game
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Closes the dialogue window and resumes the game.
    /// </summary>
    public void CloseDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        
        isDialogueActive = false;
        
        // Resume the game
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Returns whether the dialogue is currently active.
    /// </summary>
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
