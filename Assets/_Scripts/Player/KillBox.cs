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
            if (collision.transform.position.y > transform.position.y && player.IsGrounded) 
                GameManager.Instance.PlayerDeath();
    }
}
