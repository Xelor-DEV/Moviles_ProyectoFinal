using UnityEngine;

public class ObstacleRat : MonoBehaviour, IObstacle
{
    public string ObstacleType => "Down";
    private ObstacleSpawner pooler;
    public void Clear()
    {
        Debug.Log("Rata destruida");
        pooler.ReturnToPool(gameObject);
    }
    public void SetPooler(ObstacleSpawner pool)
    {
        pooler = pool;  
    }
}
