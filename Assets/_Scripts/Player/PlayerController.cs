using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Base move speed for horizontal tilt movement")]
    public float moveSpeed = 5f;

    [Tooltip("Multiplier for how sensitive tilt is")]
    public float tiltSensitivity = 2f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;


    private Rigidbody2D rb;
    private SoundManager sm;

    public bool IsGrounded => isGrounded;
    private bool isGrounded;

    private float startY;
    private float currentY;

    private float inputX;
    private bool isDead = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sm = FindAnyObjectByType<SoundManager>();

        startY = transform.position.y;

        var input = FindAnyObjectByType<InputManager>();
        var tap = FindAnyObjectByType<TapDetection>();

        input.OnTiltChanged += HandleTilt;
        tap.OnTap += pos => Jump();
    }

    private void Update()
    {
        if (GameManager.Instance.isDead) Dead();

        isGrounded = CheckGrounded();

        if (isGrounded)
        {
            currentY = transform.position.y;
            GameManager.Instance._score = Mathf.Max(GameManager.Instance._score, Mathf.RoundToInt(currentY - startY));
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = inputX * moveSpeed;

        rb.linearVelocity = velocity;
    }

    private void HandleTilt(Vector3 tilt)
    {
        inputX = tilt.x * tiltSensitivity;
    }

    private void Jump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        sm.CreateSound(sm.playerJump);
    }
    private void Dead()
    {
        if (isDead) return;
        isDead = true;
        sm.CreateSound(sm.playerDeath);
    }

    private bool CheckGrounded()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        float rayLength = 0.1f; // adjust to your needs
        Vector2 bottomLeft = new Vector2(box.bounds.min.x, box.bounds.min.y);
        Vector2 bottomCenter = new Vector2(box.bounds.center.x, box.bounds.min.y);
        Vector2 bottomRight = new Vector2(box.bounds.max.x, box.bounds.min.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(bottomLeft, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitCenter = Physics2D.Raycast(bottomCenter, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(bottomRight, Vector2.down, rayLength, groundLayer);

        // Debug rays in scene view
        Debug.DrawRay(bottomLeft, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(bottomCenter, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(bottomRight, Vector2.down * rayLength, Color.red);

        return hitLeft || hitCenter || hitRight;
    }
}
