using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public Transform player;
    public float heightAbovePlayer = 15f;
    public float spawnInterval = 2f;
    public float spawnRangeX = 8f;

    [Header("Blocks")]
    public GameObject[] blockPrefabs;

    private float timer;

    private void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(0f, player.position.y + heightAbovePlayer, 0f);
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBlock();
            timer = 0f;
        }
    }

    private void SpawnBlock()
    {
        if (blockPrefabs.Length == 0)
        {
            Debug.LogError("No block Prefabs in block spawner");
            return;
        }

        int randomIndex = Random.Range(0, blockPrefabs.Length);
        GameObject blockPrefab = blockPrefabs[randomIndex];

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0f);

        Quaternion randomRotation = Quaternion.Euler(0f, 0f, 90f * Random.Range(0, 4));

        Instantiate(blockPrefab, spawnPos, randomRotation);
    }
}
