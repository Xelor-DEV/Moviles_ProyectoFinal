using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 5f;
    public float destroyXPosition = -15f;
    public ExternalRunner runner;
    public bool isPickup;

    private Vector3 movement;
    private float accumulatedTime;

    void Update()
    {
        if (RunnerManager.Instance.isGameOver) return;

        accumulatedTime += Time.deltaTime;

        float fixedStep = Time.fixedDeltaTime;
        while (accumulatedTime >= fixedStep)
        {
            movement = Vector3.right * -speed * fixedStep;
            accumulatedTime -= fixedStep;
        }

        transform.position += movement * (Time.deltaTime / fixedStep);

        if (transform.position.x < destroyXPosition)
        {
            runner?.ReturnToPool(gameObject, isPickup);
        }
    }
}