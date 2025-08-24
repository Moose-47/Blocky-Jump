using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    public float lockVelocityThreshold = 0.01f; //consider "at rest" if velocity is near zero
    public float spawnDelay = 0.1f; //short delay to prevent locking immediately on spawn

    private Rigidbody2D rb;
    private bool isLocked = false;
    private float spawnTime;
    private SoundManager sm;
    private void Awake()
    {
        sm = FindAnyObjectByType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (isLocked) return;

        // Don't check immediately after spawn
        if (Time.time - spawnTime < spawnDelay) return;

        // Only lock if both horizontal and vertical velocity are near zero
        if (Mathf.Abs(rb.linearVelocity.x) < lockVelocityThreshold &&
            Mathf.Abs(rb.linearVelocity.y) < lockVelocityThreshold)
        {
            rb.bodyType = RigidbodyType2D.Static;
            sm.CreateSound(sm.blockLand);
            isLocked = true;
        }
    }
}
