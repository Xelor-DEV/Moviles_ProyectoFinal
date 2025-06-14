using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 5f;
    public float destroyX = -30f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
