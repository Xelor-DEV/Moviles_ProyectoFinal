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
            if(spawnRate < 3)
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


        GameObject newObject = Instantiate(
            objectToSpawn,
            new Vector3(spawnXPosition, selectedLane.position.y, selectedLane.position.z),
            objectToSpawn.transform.rotation
        );

  
        ObstacleMover mover = newObject.AddComponent<ObstacleMover>();
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

    private GameObject GetRandomObstacle()
    {
        return obstaclePrefabs[random.Next(0, obstaclePrefabs.Length)];
    }

}