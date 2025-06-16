using UnityEngine;

public class SpawnScrapt : MonoBehaviour
{
    public GameObject pickupPrefab;
    public float spawnDelay = 5f;
    public Transform[] spawnPoints;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPickup), 2f, spawnDelay);
    }

    void SpawnPickup()
    {
        if (spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(pickupPrefab, spawnPoints[index].position, Quaternion.identity);
    }
}
