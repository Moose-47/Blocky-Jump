using UnityEngine;

public class KillBox : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("block"))
            if (collision.transform.position.y > transform.position.y)
            {
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                Debug.Log("Block velocity: " + rb.linearVelocity);
                if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
                    GameManager.Instance.PlayerDeath();
            }
    }
}
