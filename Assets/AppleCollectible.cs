using UnityEngine;

/// <summary>
/// Collectible apple that floats and can be collected by the player.
/// When collected, it increases the score and destroys itself.
/// </summary>
public class AppleCollectible : MonoBehaviour
{
    [Header("Levitation Settings")]
    [Tooltip("Speed of the levitation animation")]
    public float levitationSpeed = 1f;
    
    [Tooltip("Height of the levitation movement")]
    public float levitationHeight = 0.3f;

    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
        
        // Random time offset so apples don't all move in sync
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // Create smooth floating animation using sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * levitationSpeed + timeOffset) * levitationHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player touched the apple
        if (other.CompareTag("Player"))
        {
            CollectApple();
        }
    }

    private void CollectApple()
    {
        // Find and notify the score manager
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScore(1);
        }

        // Destroy this apple
        Destroy(gameObject);
    }
}
