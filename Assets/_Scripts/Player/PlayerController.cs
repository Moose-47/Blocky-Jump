using UnityEngine;

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
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private SoundManager sm;

    public bool IsGrounded => isGrounded;
    private bool isGrounded;

    private float startY;
    private float currentY;

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

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            currentY = transform.position.y;
            GameManager.Instance._score = Mathf.Max(GameManager.Instance._score, Mathf.RoundToInt(currentY - startY));
        }
    }

    private void HandleTilt(Vector3 tilt)
    {
        float move = tilt.x * tiltSensitivity;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        sm.CreateSound(sm.playerJump);
    }
}
