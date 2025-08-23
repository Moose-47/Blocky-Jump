using UnityEngine;

public class BlockDeleter : MonoBehaviour
{
    public Transform player;
    public float heightOffSet = 4f;
    private void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(0f, player.position.y - heightOffSet, 0f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("block"))
        {
            Destroy(collision.gameObject);
        }
    }
}
