using UnityEngine;

public class EdgeBounds : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
            transform.position = new Vector3(0f, player.position.y, 0f);
    }
}
