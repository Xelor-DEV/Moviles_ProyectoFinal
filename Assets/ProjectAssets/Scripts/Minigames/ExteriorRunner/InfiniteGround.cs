using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public GameObject groundPrefab;
    public float groundLength = 10f;
    public float moveSpeed = 5f;
    public Vector3 startPosition = new Vector3(0, 0, 0); 
    public float leftBoundary = -20f;

    private GameObject[] groundPieces = new GameObject[5];

    void Start()
    {
        
        for (int i = 0; i < 5; i++)
        {
            groundPieces[i] = Instantiate(
                groundPrefab,
                startPosition + new Vector3(i * groundLength, 0, 0),
                Quaternion.identity,
                transform
            );
        }
    }

    void Update()
    {
        if (RunnerManager.Instance.isGameOver) return;

        float movement = moveSpeed * Time.deltaTime;

        for (int i = 0; i < 5; i++)
        {
            groundPieces[i].transform.Translate(Vector3.left * movement, Space.World);

            if (groundPieces[i].transform.position.x < leftBoundary)
            {
                RepositionSegment(i);
            }
        }
    }

    void RepositionSegment(int index)
    {
        float maxX = groundPieces[0].transform.position.x;
        for (int i = 1; i < 5; i++)
        {
            if (groundPieces[i].transform.position.x > maxX)
            {
                maxX = groundPieces[i].transform.position.x;
            }
        }

        groundPieces[index].transform.position = new Vector3(
            maxX + groundLength,
            startPosition.y,
            startPosition.z  
        );
    }
}