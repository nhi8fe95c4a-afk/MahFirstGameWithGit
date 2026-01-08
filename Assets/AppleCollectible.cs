using UnityEngine;
using System.Collections;

/// <summary>
/// Collectible apple that floats and can be collected by the player.
/// When collected, it increases the score and fades out before being destroyed.
/// </summary>
public class AppleCollectible : MonoBehaviour
{
    [Header("Levitation Settings")]
    [Tooltip("Speed of the levitation animation")]
    public float levitationSpeed = 1f;
    
    [Tooltip("Height of the levitation movement")]
    public float levitationHeight = 0.3f;

    [Header("Collection Settings")]
    [Tooltip("Time it takes for the apple to fade out when collected")]
    public float fadeOutTime = 0.5f;

    private Vector3 startPosition;
    private float timeOffset;
    private bool isCollected = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D appleCollider;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
        
        // Random time offset so apples don't all move in sync
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
        
        // Get the sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Get the collider component (cache for performance)
        appleCollider = GetComponent<Collider2D>();
        
        // Debug: Verify the apple is initialized properly
        Debug.Log($"Apple {gameObject.name} initialized at position {startPosition}");
        
        // Verify we have required components
        if (spriteRenderer == null)
        {
            Debug.LogError($"Apple {gameObject.name} is missing SpriteRenderer!");
        }
        
        if (appleCollider == null)
        {
            Debug.LogError($"Apple {gameObject.name} is missing Collider2D!");
        }
        else if (!appleCollider.isTrigger)
        {
            Debug.LogWarning($"Apple {gameObject.name} collider is not set as trigger!");
        }
    }

    void Update()
    {
        // Only levitate if not collected
        if (!isCollected)
        {
            // Create smooth floating animation using sine wave
            float newY = startPosition.y + Mathf.Sin(Time.time * levitationSpeed + timeOffset) * levitationHeight;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug: Log what collided with us
        Debug.Log($"Apple {gameObject.name} triggered by: {other.gameObject.name}, tag: {other.tag}");
        
        // Check if the player touched the apple and we haven't been collected yet
        if (!isCollected && other.CompareTag("Player"))
        {
            Debug.Log($"Apple {gameObject.name} collected by Player!");
            CollectApple();
        }
    }

    private void CollectApple()
    {
        // Mark as collected to prevent multiple collections
        isCollected = true;
        
        // Disable the collider so we can't be collected again
        appleCollider.enabled = false;
        
        // Find and notify the score manager
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScore(1);
        }

        // Start fading out
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeOutTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        
        // Destroy this apple after fade out is complete
        Destroy(gameObject);
    }
}
