using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    public float lockVelocityThreshold = 0.01f; //consider "at rest" if velocity is near zero
    public float spawnDelay = 0.1f; //short delay to prevent locking immediately on spawn

    private Rigidbody2D rb;
    private Collider2D col;
    private SoundManager sm;

    private bool isLocked = false;
    private bool isTouchingGround = false;
    private float spawnTime;

    public LayerMask groundLayers;
    private void Awake()
    {
        sm = FindAnyObjectByType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (isLocked) return;

        // Skip early frames after spawn
        if (Time.time - spawnTime < spawnDelay) return;

        // Check if touching ground
        isTouchingGround = IsGrounded();

        // Only lock if velocity is near zero AND grounded
        if (isTouchingGround &&
            Mathf.Abs(rb.linearVelocity.x) < lockVelocityThreshold &&
            Mathf.Abs(rb.linearVelocity.y) < lockVelocityThreshold)
        {
            rb.bodyType = RigidbodyType2D.Static;
            gameObject.tag = "LockedBlock";
            sm.CreateSound(sm.blockLand);
            isLocked = true;
        }
    }

    private bool IsGrounded()
    {
        //BoxCast slightly below the collider to detect ground or another block
        Bounds bounds = col.bounds;
        float extraHeight = 0.05f; //small margin
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, extraHeight, groundLayers);
        return hit.collider != null;
    }
}
