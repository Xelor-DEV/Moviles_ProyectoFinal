using UnityEngine;

public class ObstacleBat : MonoBehaviour, IObstacle
{
    public string ObstacleType => "Up";
    private ObstacleSpawner pooler;
    public void Clear()
    {
        Debug.Log("Murcielago destruido");
        pooler.ReturnToPool(gameObject); ;
    }
    public void SetPooler(ObstacleSpawner pool)
    {
        pooler = pool;
    }
}
