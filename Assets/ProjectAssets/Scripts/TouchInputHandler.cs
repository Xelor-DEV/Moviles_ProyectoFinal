using UnityEngine;
using UnityEngine.Events;

public class TouchInputHandler : MonoBehaviour
{
    [Header("Touch Events")]
    public UnityEvent onDragStart;
    public UnityEvent<Vector3> onDragStartWithPosition;
    public UnityEvent onDrag;
    public UnityEvent<Vector3> onDragWithPosition;
    public UnityEvent onDragEnd;
    public UnityEvent<Vector3> onDragEndWithPosition;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    [SerializeField] private float raycastDistance = 100f;

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
