using UnityEngine;
using System.Collections.Generic;
public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObstaclePair
    {
        public GameObject obstacle3D;
        public GameObject object2D;
        public string obstacleType;
    }

    public List<ObstaclePair> obstaclePairs;
    public int initialCount = 10;
    public int batchSize = 5;
    public float distanceBetween = 4f;
    public Vector3 startPosition = new Vector3(6f, 0f, 0f);
    public Vector3 offset2DObject = new Vector3(0, 2f, 0);

    private Queue<GameObject> activeObstacles = new Queue<GameObject>();
    private Vector3 nextSpawnPosition;
    private int clearedSinceLastSpawn = 0;
    private int totalSpawned = 0;

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
        for (int i = 0; i < count; i++)
        {

            int randomIndex = Random.Range(0, obstaclePairs.Count);
            ObstaclePair pair = obstaclePairs[randomIndex];

            
            GameObject obstacle3D = Instantiate(pair.obstacle3D, nextSpawnPosition, Quaternion.identity);

            
            if (totalSpawned < initialCount)
            {
                
                GameObject object2D = Instantiate(
                    pair.object2D,
                    nextSpawnPosition + offset2DObject,
                    Quaternion.identity
                );

       
                object2D.transform.SetParent(obstacle3D.transform);
                totalSpawned++;
            }

            if (obstacle3D.TryGetComponent<IObstacle>(out var obs))
            {
                obs.SetPooler(this);
            }

            activeObstacles.Enqueue(obstacle3D);
            nextSpawnPosition += new Vector3(distanceBetween, 0, 0);
        }
    }
}
