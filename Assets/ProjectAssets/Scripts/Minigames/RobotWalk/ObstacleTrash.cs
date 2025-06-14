using UnityEngine;

public class ObstacleTrash : MonoBehaviour, IObstacle
{
    public string ObstacleType => "Right";
    private ObstacleSpawner pooler;
    public void Clear()
    {
        Debug.Log("Basura destruida");
        pooler.ReturnToPool(gameObject);
    }
    public void SetPooler(ObstacleSpawner pool)
    {
        pooler = pool; 
    }

}
