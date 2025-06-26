using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public GameObject groundPrefab;
    public float groundLength = 10f;
    public float moveSpeed = 5f;
    public Vector3 startPosition = new Vector3(0, 0, 0);
    public float leftBoundary = -20f;

    private GameObject[] groundPieces = new GameObject[4];
    private int nextIndex = 0;

    void Start()
    {
        for (int i = 0; i < groundPieces.Length; i++)
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

        foreach (GameObject piece in groundPieces)
        {
            piece.transform.Translate(Vector3.left * movement, Space.World);
        }

        GameObject firstPiece = groundPieces[nextIndex];

        if (firstPiece.transform.position.x < leftBoundary)
        {
            int lastIndex = (nextIndex - 1 + groundPieces.Length) % groundPieces.Length;
            Vector3 lastPos = groundPieces[lastIndex].transform.position;

            firstPiece.transform.position = new Vector3(
                lastPos.x + groundLength,
                startPosition.y,
                startPosition.z
            );

            nextIndex = (nextIndex + 1) % groundPieces.Length;
        }
    }
}