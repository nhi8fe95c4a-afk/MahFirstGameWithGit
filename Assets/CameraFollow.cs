using UnityEngine;

/// <summary>
/// Camera follow script that tracks the player with a flexible dead zone.
/// The camera stays still when the player is within the dead zone bounds,
/// and only starts following when the player moves beyond those bounds.
/// 
/// Default Settings:
/// - Dead Zone: 3x2 units (width x height)
/// - Smooth Speed: 0.125 (responsive but smooth)
/// - Offset: (0, 0, -10) for proper 2D depth
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target to follow (usually the Player)")]
    public Transform target;

    [Header("Dead Zone Settings")]
    [Tooltip("Horizontal dead zone width. Camera won't move if player is within this zone.")]
    public float deadZoneWidth = 3f;
    
    [Tooltip("Vertical dead zone height. Camera won't move if player is within this zone.")]
    public float deadZoneHeight = 2f;

    [Header("Follow Settings")]
    [Tooltip("How smoothly the camera follows the target (higher = smoother but slower)")]
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f;
    
    [Tooltip("Offset from the target position")]
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Limits (Optional)")]
    [Tooltip("Enable camera position limits")]
    public bool useLimits = false;
    
    [Tooltip("Minimum X position for camera")]
    public float minX = -10f;
    
    [Tooltip("Maximum X position for camera")]
    public float maxX = 10f;
    
    [Tooltip("Minimum Y position for camera")]
    public float minY = 0f;
    
    [Tooltip("Maximum Y position for camera")]
    public float maxY = 10f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate target position with offset
        Vector3 targetPosition = target.position + offset;
        
        // Get current camera position
        Vector3 currentPosition = transform.position;
        
        // Calculate the difference between target and camera
        Vector3 delta = targetPosition - currentPosition;
        
        // Apply dead zone - only move if outside dead zone bounds
        float moveX = 0f;
        float moveY = 0f;
        
        // Horizontal dead zone check
        if (Mathf.Abs(delta.x) > deadZoneWidth / 2f)
        {
            // Move camera, but keep the edge of dead zone at player position
            if (delta.x > 0)
                moveX = delta.x - deadZoneWidth / 2f;
            else
                moveX = delta.x + deadZoneWidth / 2f;
        }
        
        // Vertical dead zone check
        if (Mathf.Abs(delta.y) > deadZoneHeight / 2f)
        {
            // Move camera, but keep the edge of dead zone at player position
            if (delta.y > 0)
                moveY = delta.y - deadZoneHeight / 2f;
            else
                moveY = delta.y + deadZoneHeight / 2f;
        }
        
        // Create desired position
        Vector3 desiredPosition = currentPosition + new Vector3(moveX, moveY, 0);
        
        // Apply limits if enabled
        if (useLimits)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }
        
        // Smoothly move towards desired position
        Vector3 smoothedPosition = Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, smoothSpeed);
        
        // Keep Z position (important for camera depth)
        smoothedPosition.z = offset.z;
        
        transform.position = smoothedPosition;
    }

    void OnDrawGizmosSelected()
    {
        // Draw dead zone visualization
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 deadZoneCenter = transform.position;
            deadZoneCenter.z = target.position.z;
            Gizmos.DrawWireCube(deadZoneCenter, new Vector3(deadZoneWidth, deadZoneHeight, 0));
        }

        // Draw limits if enabled
        if (useLimits)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, transform.position.z);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
