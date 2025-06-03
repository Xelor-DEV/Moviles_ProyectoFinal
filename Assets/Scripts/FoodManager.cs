using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance; 
    public GameObject[] foodPrefabs;    
    public Transform spawnPoint;        
    private int currentIndex = 0;

    void Awake()
    {
        Instance = this;
        SpawnInitialFood();
    }

    void SpawnInitialFood()
    {
        Instantiate(foodPrefabs[currentIndex], spawnPoint.position, Quaternion.identity);
    }

    public void ChangeFood(int direction) 
    {
        currentIndex = (currentIndex + direction + foodPrefabs.Length) % foodPrefabs.Length;
        SpawnNewFood();
    }

    public void SpawnNewFood()
    {
        Instantiate(foodPrefabs[currentIndex], spawnPoint.position, Quaternion.identity);
    }

    public GameObject GetCurrentFoodPrefab()
    {
        return foodPrefabs[currentIndex];
    }
}
