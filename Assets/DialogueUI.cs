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
    private int dialogueOpenedFrame = -1;

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
        // Use GetKeyDown which only triggers once per press
        // Skip the frame when dialogue was opened to prevent immediate close
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E) && Time.frameCount > dialogueOpenedFrame)
        {
            CloseDialogue();
        }
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
        dialogueOpenedFrame = Time.frameCount;
        
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
