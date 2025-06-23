using UnityEngine;
using UnityEngine.Events;

public class TouchInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [Header("Drag Events")]
    public UnityEvent onDragStart;
    public UnityEvent<Vector3> onDragStartWithPosition;
    public UnityEvent onDrag;
    public UnityEvent<Vector3> onDragWithPosition;
    public UnityEvent onDragEnd;
    public UnityEvent<Vector3> onDragEndWithPosition;

    [Header("Drag Settings")]
    [SerializeField] private float raycastDistance = 100f;

    [Header("Swipe Events")]
    public UnityEvent onSwipeLeft;
    public UnityEvent onSwipeRight;

    [Header("Swipe Settings")]
    private Vector2 touchStartPosition;
    [SerializeField] private float minSwipeDistance = 50f;

    private bool isDragging = false;
    private Vector3 lastPosition;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found. Please assign a camera to the TouchInputHandler.");
            }
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit, raycastDistance))
                    {
                        isDragging = true;
                        lastPosition = hit.point;
                        touchStartPosition = touch.position;
                        onDragStart?.Invoke();
                        onDragStartWithPosition?.Invoke(hit.point);
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging && Physics.Raycast(ray, out hit, raycastDistance))
                    {
                        onDrag?.Invoke();
                        onDragWithPosition?.Invoke(hit.point);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false;
                        onDragEnd?.Invoke();
                        onDragEndWithPosition?.Invoke(lastPosition);

                        Vector2 touchEndPosition = touch.position;
                        Vector2 swipeDirection = touchEndPosition - touchStartPosition;

                        if (swipeDirection.magnitude >= minSwipeDistance)
                        {
                            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                            {
                                if (swipeDirection.x > 0)
                                {
                                    onSwipeRight?.Invoke();
                                }
                                else
                                {
                                    onSwipeLeft?.Invoke();
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (mainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * raycastDistance);
        }
    }
}
