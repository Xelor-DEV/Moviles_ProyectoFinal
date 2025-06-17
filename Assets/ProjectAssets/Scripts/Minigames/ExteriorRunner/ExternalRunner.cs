using UnityEngine;
using System.Collections.Generic;
public class ExternalRunner : MonoBehaviour
{
    public int maxGroundSegments = 3;
    public GameObject groundPrefab; 
    public float groundLength = 20f;
    public float groundSpeed = 5f;
    public float groundHeight = 0f;
    public GameObject obstaclePrefab1;
    public GameObject obstaclePrefab2;

    private Queue<GameObject> groundPool = new Queue<GameObject>();
    private float timer = 0f;
    public float speedIncreaseInterval = 5f;

    void Awake()
    {

        for (int i = 0; i < maxGroundSegments; i++)
        {
            CreateGroundSegment(i * groundLength);

        }
    }

    void Update()
    {
        MoveGround();
        RecycleGround();
        timer += Time.deltaTime;
        if(timer >= speedIncreaseInterval)
        {
            groundSpeed += 1f;
            timer = 0f;
        }
    }

    void MoveGround()
    {
        foreach (GameObject ground in groundPool)
        {
            ground.transform.Translate(Vector3.left * groundSpeed * Time.deltaTime);
        }
       
    }

    void RecycleGround()
    {
        GameObject firstGround = groundPool.Peek();

        if (firstGround.transform.position.x < -groundLength)
        {
            GameObject recycledGround = groundPool.Dequeue();
            GameObject lastGround = GetLastGround();

            float newX = lastGround.transform.position.x + groundLength;
            recycledGround.transform.position = new Vector3(newX, groundHeight, 0);
            groundPool.Enqueue(recycledGround);

            SpawnObstacles(newX);
        }
    }
    void CreateGroundSegment(float xPosition)
    {
        GameObject ground = Instantiate(groundPrefab, new Vector3(xPosition, groundHeight, 0), Quaternion.identity);
        groundPool.Enqueue(ground);
        SpawnObstacles(xPosition);
    }

    void SpawnObstacles(float baseX)
    {
        float groundX = baseX + groundLength / 2f;
        Vector3 spawnPosition;



        int obstacleType = Random.Range(1, 5);

        if (obstacleType == 1)
        {
           
            spawnPosition = new Vector3(groundX, 2.27f, 0);
            GameObject o = Instantiate(obstaclePrefab1, spawnPosition, Quaternion.identity);
          
            o.AddComponent<ObstacleMover>().speed = groundSpeed;
        }
        else if (obstacleType == 2) 
        {
            
            spawnPosition = new Vector3(groundX + 10f, 5.36f, 0);
            GameObject o = Instantiate(obstaclePrefab2, spawnPosition, Quaternion.identity);
         
            o.AddComponent<ObstacleMover>().speed = groundSpeed;
        }
        else if(obstacleType == 3)
        {
                spawnPosition = new Vector3(groundX + 10f, 4.36f, 0);
                GameObject o = Instantiate(obstaclePrefab2, spawnPosition, Quaternion.identity);
              
                o.AddComponent<ObstacleMover>().speed = groundSpeed;


        }
        else
        {
            spawnPosition = new Vector3(groundX + 10f, 2.86f, 0);
            GameObject o = Instantiate(obstaclePrefab2, spawnPosition, Quaternion.identity);
          
            o.AddComponent<ObstacleMover>().speed = groundSpeed;
        }


    }
    GameObject GetLastGround()
    {
        GameObject[] grounds = groundPool.ToArray();
        return grounds[grounds.Length - 1]; 
    }

}
