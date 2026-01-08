using UnityEngine;

/// <summary>
/// Component for interactable objects that show a dialogue when the player presses E.
/// Attach this to objects like Apple that the player can interact with.
/// </summary>
public class Interactable : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [Tooltip("The text to display in the dialogue window when interacting with this object")]
    [TextArea(3, 10)]
    public string dialogueText = "Привет. Я - могучее яблоко. Ты выполнил квест";

    [Header("Interaction Settings")]
    [Tooltip("The range at which the player can interact with this object")]
    public float interactionRange = 2f;

    [Header("References")]
    [Tooltip("Reference to the player transform. If not set, will try to find by name.")]
    public Transform playerTransform;
    
    [Tooltip("Reference to the DialogueUI. If not set, will search for it.")]
    public DialogueUI dialogueUI;

    private bool isPlayerInRange = false;

    void Start()
    {
        // Find the player in the scene if not assigned
        if (playerTransform == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        // Find the DialogueUI in the scene if not assigned
        if (dialogueUI == null)
        {
            dialogueUI = FindAnyObjectByType<DialogueUI>();
        }
    }

    void Update()
    {
        if (playerTransform == null || dialogueUI == null)
            return;

        // Check if player is in range
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        isPlayerInRange = distance <= interactionRange;

        // Handle interaction input - only open dialogue, closing is handled by DialogueUI
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !dialogueUI.IsDialogueActive())
        {
            dialogueUI.ShowDialogue(dialogueText);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw interaction range in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
