using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float wrapBuffer = 0.5f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask blockLayers;

    private BoxCollider2D bc;

    private float leftBound;
    private float rightBound;

    bool wrapped = false;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    private void LateUpdate()
    {
        if (!player) return;

        //Keep wrapper aligned with player's Y
        Vector3 pos = transform.position;
        pos.y = player.position.y;
        transform.position = pos;

        float halfWidth = bc.size.x / 2f;
        leftBound = transform.position.x - halfWidth;
        rightBound = transform.position.x + halfWidth;

        Vector3 playerPos = player.position;

        //Calculate intended wrap positions
        Vector3 wrapLeft = new Vector3(rightBound - wrapBuffer, playerPos.y, playerPos.z);
        Vector3 wrapRight = new Vector3(leftBound + wrapBuffer, playerPos.y, playerPos.z);

        //Check left wrap
        if (playerPos.x < leftBound - wrapBuffer)
        {
            if (CanTeleport(wrapLeft))
            {
                playerPos = wrapLeft; //safe to wrap
                wrapped = true;
            }
            else
                playerPos.x = leftBound; //blocked, stay at boundary
        }
        //Check right wrap
        else if (playerPos.x > rightBound - wrapBuffer)
        {
            if (CanTeleport(wrapRight))
            {
                playerPos = wrapRight;
                wrapped = true;
            }
            else
                playerPos.x = rightBound - wrapBuffer;
        }

        if (wrapped)
            player.position = playerPos;
    }

    private bool CanTeleport(Vector3 targetPos)
    {
        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
        if (!playerCollider) return true;

        // Shrink width by 25%, height by 50%
        Vector2 checkSize = new Vector2(playerCollider.size.x * 0.75f, playerCollider.size.y * 0.25f);

        // Center the slice vertically on the player's middle
        Vector3 offsetTarget = targetPos + Vector3.up * (checkSize.y);

        // Check if any blocking object is at the target position
        Collider2D hit = Physics2D.OverlapBox(
            offsetTarget,
            checkSize,
            0f,
            blockLayers
        );

        return hit == null; // true = safe, false = blocked
    }

    // Draw Gizmo for visualization
    private void OnDrawGizmosSelected()
    {
        if (!player) return;

        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
        if (!playerCollider) return;

        Vector2 checkSize = new Vector2(playerCollider.size.x * 0.75f, playerCollider.size.y * 0.25f);
        Vector3 offsetTarget = player.position + Vector3.up * (checkSize.y);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(offsetTarget, checkSize);
    }
}
