using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Settings")]
    public float smoothSpeed = 5f;
    [Tooltip("Percentage of camera height from bottom where camera starts following player upwards (0-1)")]
    public float upwardThreshold = 0.3f;  //dead zone from bottom
    [Tooltip("Percentage of camera height from bottom where camera starts following player downwards (0-1)")]
    public float downwardThreshold = 0.1f; //lower dead zone
    [Header("Limits")]
    public float minCameraY = 8f;

    private float camHeight;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraFollow: Player reference not assigned.");
            enabled = false;
            return;
        }

        camHeight = Camera.main.orthographicSize * 2f;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        float cameraBottomY = transform.position.y - Camera.main.orthographicSize;
        float cameraTopY = transform.position.y + Camera.main.orthographicSize;

        // Determine thresholds in world space
        float upwardY = cameraBottomY + camHeight * upwardThreshold;
        float downwardY = cameraBottomY + camHeight * downwardThreshold;

        float targetY = transform.position.y;

        if (player.position.y > upwardY)
        {
            //Player above upper dead zone, move camera up
            targetY += player.position.y - upwardY;
        }
        else if (player.position.y < downwardY)
        {
            //Player below lower dead zone, move camera down
            targetY -= downwardY - player.position.y;
        }

        //Clamp camera bottom
        float minY = minCameraY + Camera.main.orthographicSize;
        targetY = Mathf.Max(targetY, minY);

        //Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), smoothSpeed * Time.deltaTime);
    }
}
