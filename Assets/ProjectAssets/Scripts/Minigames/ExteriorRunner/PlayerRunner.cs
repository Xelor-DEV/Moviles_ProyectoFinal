using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRunner : MonoBehaviour
{
    public enum Lane { Left, Middle, Right }
    private Lane currentLane = Lane.Middle;

    [Header("Movement Settings")]
    public float laneDistance = 2f;
    public float swipeThreshold = 50f;
    public float moveSpeed = 5f;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private bool isSwiping = false;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleTouchInput();

        if (isMoving)
        {
            MoveToTargetPosition();
        }
    }

    private void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    fingerDownPosition = touch.position;
                    fingerUpPosition = touch.position;
                    isSwiping = false;
                    break;

                case TouchPhase.Moved:
                    fingerUpPosition = touch.position;
                    if (!isMoving)
                    {
                        CheckSwipe();
                    }
                    break;

                case TouchPhase.Ended:
                    isSwiping = false; 
                    break;
            }
        }
    }

    private void MoveToTargetPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;
        }
    }

    private void CheckSwipe()
    {
        if (isSwiping || isMoving) return;

        float deltaX = fingerUpPosition.x - fingerDownPosition.x;

        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            isSwiping = true;

            if (deltaX > 0)
            {
                MoveLeft();
                
            }
            else
            {
                MoveRight();
            }
        }
    }

    private void MoveLeft()
    {
        switch (currentLane)
        {
            case Lane.Right:
                currentLane = Lane.Middle;
                targetPosition = new Vector3(transform.position.x, transform.position.y, 0);
                break;
            case Lane.Middle:
                currentLane = Lane.Left;
                targetPosition = new Vector3(transform.position.x, transform.position.y, -laneDistance);
                break;
        }
        isMoving = true;
    }

    private void MoveRight()
    {
        switch (currentLane)
        {
            case Lane.Left:
                currentLane = Lane.Middle;
                targetPosition = new Vector3(transform.position.x, transform.position.y, 0);
                break;
            case Lane.Middle:
                currentLane = Lane.Right;
                targetPosition = new Vector3(transform.position.x, transform.position.y, laneDistance);
                break;
        }
        isMoving = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            RunnerManager.Instance.ShowGameOverPanel();
            AudioManager.Instance.PlaySfx(2);
        }
        else if (other.CompareTag("Pickup"))
        {
            RunnerManager.Instance.counterData.AddPickup();
            other.gameObject.SetActive(false);
            AudioManager.Instance.PlaySfx(1);
        }
    }
}