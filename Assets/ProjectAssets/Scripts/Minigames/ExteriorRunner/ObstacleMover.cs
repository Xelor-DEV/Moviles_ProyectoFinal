using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 5f;
    public bool moveRight = false;
    public float destroyXPosition = -15f;

    void Update()
    {
        if (RunnerManager.Instance.isGameOver) return;

   
        float direction = moveRight ? 1f : -1f;
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

    
        if ((!moveRight && transform.position.x < destroyXPosition) || (moveRight && transform.position.x > destroyXPosition))
        {
            Destroy(gameObject);
        }
    }
}
