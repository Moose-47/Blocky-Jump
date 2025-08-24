using UnityEngine;

public class BlockDeleter : MonoBehaviour
{
    public Transform player;
    public float heightOffset = 4f;
    public Vector2 deletionAreaSize = new Vector2(10f, 1f);

    private void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(0f, player.position.y - heightOffset, 0f);

            //Detect all colliders in deletion area
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, deletionAreaSize, 0f);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("block"))
                {
                    Destroy(hit.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, deletionAreaSize);
    }
}
