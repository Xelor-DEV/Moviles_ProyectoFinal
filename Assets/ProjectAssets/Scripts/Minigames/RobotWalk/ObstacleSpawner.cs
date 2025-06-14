using UnityEngine;
using System.Collections.Generic;
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public int initialCount = 10;
    public int batchSize = 5;
    public float distanceBetween = 4f;
    public Vector3 startPosition = new Vector3(6f, 0f, 0f);

    private Queue<GameObject> activeObstacles = new Queue<GameObject>();
    private Vector3 nextSpawnPosition;
    private int clearedSinceLastSpawn = 0;
    private Dictionary<string, float> obstacleYPositions = new Dictionary<string, float>
{
    { "Trash", 0f },
    { "Bat", 2f },
    { "Rat", -1f }
};
    void Start()
    {
        nextSpawnPosition = startPosition;
        SpawnObstacles(initialCount);
    }

    public void ReturnToPool(GameObject obj)
    {
        Destroy(obj);
        activeObstacles.Dequeue(); 
        clearedSinceLastSpawn++;

        if (clearedSinceLastSpawn >= batchSize)
        {
            clearedSinceLastSpawn = 0;
            SpawnObstacles(batchSize);
        }
    }

    void SpawnObstacles(int count)
    {
        List<GameObject> shuffledPrefabs = new List<GameObject>(obstaclePrefabs);

        
        for (int i = 0; i < shuffledPrefabs.Count; i++)
        {
            int randIndex = Random.Range(i, shuffledPrefabs.Count);
            var temp = shuffledPrefabs[i];
            shuffledPrefabs[i] = shuffledPrefabs[randIndex];
            shuffledPrefabs[randIndex] = temp;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject prefab = shuffledPrefabs[Random.Range(0, shuffledPrefabs.Count)];
            GameObject obj = Instantiate(prefab, nextSpawnPosition, Quaternion.identity);

            if (obj.TryGetComponent<IObstacle>(out var obs))
            {
                obs.SetPooler(this);
            }

            activeObstacles.Enqueue(obj);
            nextSpawnPosition += new Vector3(distanceBetween, 0, 0);
        }
    }

}
