using UnityEngine;
using UnityEngine.EventSystems;

public class FoodDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public float feedAmount = 20f;
    public GameObject foodPrefab;
    public GameObject Spawner;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
                    {
                        isDragging = true;
                        offset = transform.position - touchPos;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        transform.position = touchPos + offset;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {

                        Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f);
                        foreach (Collider het in hits)
                        {
                            if (het.CompareTag("Alien"))
                            {
                                het.GetComponent<AlienStats>().Feed(feedAmount);
                                SpawnNewFood();
                                Destroy(gameObject);
                                break;
                            }
                        }
                        isDragging = false;
                    }
                    break;
            }
        }
    }
    void SpawnNewFood()
    {
        Instantiate(foodPrefab,Spawner.transform.position,Quaternion.identity);
        
    }
}