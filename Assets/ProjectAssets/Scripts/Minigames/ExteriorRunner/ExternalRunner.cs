using UnityEngine;
using System.Collections.Generic;

public class ExternalRunner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    public GameObject pickupPrefab;

    [Header("Spawn Settings")]
    public Transform[] lanes;
    public float spawnRate = 1f;
    public float objectSpeed = 5f;
    public float spawnXPosition = 15f;
    public float destroyXPosition = -15f;
    [Range(0f, 1f)] public float pickupProbability = 0.3f;

    private float speedIncreaseTimer = 0f;
    public float speedIncreaseInterval = 10f;
    public float speedIncrement = 0.2f;

    private float nextSpawnTime;
    private System.Random random = new System.Random();

 
    private Dictionary<GameObject, Queue<GameObject>> obstaclePools;
    private Queue<GameObject> pickupPool;
    private int initialPoolSize = 10;
    private Dictionary<GameObject, GameObject> instanceToPrefabMap;

    void Start()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();
        pickupPool = new Queue<GameObject>();
        instanceToPrefabMap = new Dictionary<GameObject, GameObject>();

  
        foreach (GameObject prefab in obstaclePrefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = CreatePooledObject(prefab);
                pool.Enqueue(obj);
            }
            obstaclePools.Add(prefab, pool);
        }


        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = CreatePooledObject(pickupPrefab);
            pickupPool.Enqueue(obj);
        }
    }

    private GameObject CreatePooledObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);

       
        PooledObject pooledObj = obj.AddComponent<PooledObject>();
        pooledObj.prefab = prefab;
        pooledObj.runner = this;

        return obj;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && !RunnerManager.Instance.isGameOver)
        {
            SpawnObjects();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            objectSpeed += speedIncrement;
            if (spawnRate < 3)
            {
                spawnRate = spawnRate + 0.2f;
            }

            speedIncreaseTimer = 0f;
            Debug.Log($"Velocidad aumentada a: {objectSpeed}");
        }
    }

    private void SpawnObjects()
    {
        bool spawnPickup = Random.value < pickupProbability;
        int randomLaneIndex = random.Next(0, lanes.Length);
        Transform selectedLane = lanes[randomLaneIndex];

        GameObject objectToSpawn = spawnPickup ? pickupPrefab : GetRandomObstacle();
        GameObject newObject = GetPooledObject(objectToSpawn);

        newObject.transform.position = new Vector3(spawnXPosition, selectedLane.position.y, selectedLane.position.z);
        newObject.transform.rotation = objectToSpawn.transform.rotation;
        newObject.SetActive(true);

        ObstacleMover mover = newObject.GetComponent<ObstacleMover>();
        if (mover == null)
        {
            mover = newObject.AddComponent<ObstacleMover>();
        }
        mover.speed = objectSpeed;
        mover.moveRight = false;
        mover.destroyXPosition = destroyXPosition;

        newObject.tag = spawnPickup ? "Pickup" : "Obstacle";
        Collider collider = newObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = spawnPickup;
        }
    }

    private GameObject GetPooledObject(GameObject prefab)
    {
        if (prefab == pickupPrefab)
        {
            if (pickupPool.Count > 0)
            {
                return pickupPool.Dequeue();
            }
            else
            {
                
                GameObject newObj = CreatePooledObject(prefab);
                return newObj;
            }
        }
        else
        {
            if (obstaclePools.ContainsKey(prefab) && obstaclePools[prefab].Count > 0)
            {
                return obstaclePools[prefab].Dequeue();
            }
            else
            {
                GameObject newObj = CreatePooledObject(prefab);
                return newObj;
            }
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        PooledObject pooled = obj.GetComponent<PooledObject>();
        if (pooled != null)
        {
            obj.SetActive(false);

            if (pooled.prefab == pickupPrefab)
            {
                pickupPool.Enqueue(obj);
            }
            else
            {
                if (obstaclePools.ContainsKey(pooled.prefab))
                {
                    obstaclePools[pooled.prefab].Enqueue(obj);
                }
            }
        }
        else
        {
            Destroy(obj);
        }
    }

    private GameObject GetRandomObstacle()
    {
        return obstaclePrefabs[random.Next(0, obstaclePrefabs.Length)];
    }
}


public class PooledObject : MonoBehaviour
{
    public GameObject prefab;
    public ExternalRunner runner;

    private ObstacleMover mover;

    void Awake()
    {
        mover = GetComponent<ObstacleMover>();
    }

    void Update()
    {
        if (mover != null)
        {
            if ((!mover.moveRight && transform.position.x < mover.destroyXPosition) ||
                (mover.moveRight && transform.position.x > mover.destroyXPosition))
            {
                runner.ReturnToPool(gameObject);
            }
        }
    }
}
