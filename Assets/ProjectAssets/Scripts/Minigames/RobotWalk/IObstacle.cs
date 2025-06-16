using UnityEngine;

public interface IObstacle
{
    string ObstacleType { get; }

    void SetPooler(ObstacleSpawner pool);
    void Clear();
}