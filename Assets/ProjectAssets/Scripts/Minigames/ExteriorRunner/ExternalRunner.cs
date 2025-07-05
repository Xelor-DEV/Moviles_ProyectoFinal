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

    void Start()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();
        pickupPool = new Queue<GameObject>();

        foreach (GameObject prefab in obstaclePrefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);

                if (obj.TryGetComponent(out PooledObject pooledObj))
                {
                    pooledObj.prefab = prefab;
                }

                if (obj.TryGetComponent(out ObstacleMover mover))
                {
                    mover.runner = this;
                }

                pool.Enqueue(obj);
            }
            obstaclePools.Add(prefab, pool);
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(pickupPrefab);
            obj.SetActive(false);

            if (obj.TryGetComponent(out PooledObject pooledObj))
            {
                pooledObj.prefab = pickupPrefab;
            }

            if (obj.TryGetComponent(out ObstacleMover mover))
            {
                mover.runner = this;
            }

            pickupPool.Enqueue(obj);
        }
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
            if (spawnRate < 3) spawnRate += 0.15f;
            speedIncreaseTimer = 0f;
        }
    }

    private void SpawnObjects()
    {
        bool spawnPickup = Random.value < pickupProbability;
        int randomLaneIndex = random.Next(0, lanes.Length);
        Transform selectedLane = lanes[randomLaneIndex];

        GameObject objectToSpawn = spawnPickup ? pickupPrefab : obstaclePrefabs[random.Next(0, obstaclePrefabs.Length)];
        GameObject newObject = GetPooledObject(objectToSpawn, spawnPickup);

        newObject.transform.position = new Vector3(spawnXPosition, selectedLane.position.y, selectedLane.position.z);
        newObject.SetActive(true);

        ObstacleMover mover = newObject.GetComponent<ObstacleMover>();
        mover.speed = objectSpeed;
        mover.destroyXPosition = destroyXPosition;
        mover.isPickup = spawnPickup;

        newObject.tag = spawnPickup ? "Pickup" : "Obstacle";
        Collider collider = newObject.GetComponent<Collider>();
        if (collider != null) collider.isTrigger = spawnPickup;
    }

    private GameObject GetPooledObject(GameObject prefab, bool isPickup)
    {
        if (isPickup)
        {
            if (pickupPool.Count > 0)
            {
                return pickupPool.Dequeue();
            }
            else
            {
                GameObject newObj = Instantiate(pickupPrefab);
                if (newObj.TryGetComponent(out ObstacleMover newMover))
                {
                    newMover.runner = this;
                }
                if (newObj.TryGetComponent(out PooledObject newPooled))
                {
                    newPooled.prefab = pickupPrefab;
                }
                return newObj;
            }
        }
        else
        {
            if (obstaclePools.TryGetValue(prefab, out Queue<GameObject> pool))
            {
                if (pool.Count > 0)
                {
                    return pool.Dequeue();
                }
            }

            GameObject newObj = Instantiate(prefab);
            if (newObj.TryGetComponent(out ObstacleMover newMover))
            {
                newMover.runner = this;
            }
            if (newObj.TryGetComponent(out PooledObject newPooled))
            {
                newPooled.prefab = prefab;
            }
            return newObj;
        }
    }

    public void ReturnToPool(GameObject obj, bool isPickup)
    {
        obj.SetActive(false);

        if (isPickup)
        {
            pickupPool.Enqueue(obj);
        }
        else
        {
            if (obj.TryGetComponent(out PooledObject pooledObj) && pooledObj.prefab != null)
            {
                if (obstaclePools.ContainsKey(pooledObj.prefab))
                {
                    obstaclePools[pooledObj.prefab].Enqueue(obj);
                }
                else
                {
                    Debug.LogWarning($"No pool found for prefab: {pooledObj.prefab.name}");
                }
            }
            else
            {
                Debug.LogWarning("Returned object missing PooledObject component or prefab reference");
            }
        }
    }
}