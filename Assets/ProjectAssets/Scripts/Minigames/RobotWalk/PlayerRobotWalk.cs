using UnityEngine;
using System.Collections;
public class PlayerRobotWalk : MonoBehaviour
{
    private Vector2 startTouchPosition, endTouchPosition;
    public float moveSpeed = 2f;
    public float moveDistance = 0.2f;
    private Rigidbody rb;
    public float detectionRadius = 1.5f;


    private Animator robotAnimator;
    public float attackAnimDuration = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
        robotAnimator = GetComponentInChildren<Animator>();

        
        if (robotAnimator != null)
        {
            robotAnimator.SetFloat("AttackSpeed", 1f / attackAnimDuration); // Ajusta velocidad
        }
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            endTouchPosition = touch.position;
            Vector2 direction = endTouchPosition - startTouchPosition;

            if (direction.magnitude < 50) return;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0) HandleSwipe("Right");
            }
            else
            {
                if (direction.y > 0) HandleSwipe("Up");
                else HandleSwipe("Down");
            }
        }
    }

    void HandleSwipe(string type)
    {
        Debug.Log($"Swipe detectado: {type}");

 
        if (robotAnimator != null)
        {
            robotAnimator.Play("Attack", -1, 0f); 
        }

    
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        bool found = false;
        foreach (Collider col in colliders)
        {
            IObstacle obstacle = col.GetComponent<IObstacle>();

            if (obstacle != null && obstacle.ObstacleType == type)
            {
                obstacle.Clear();
                StartCoroutine(SmoothMove(transform.position, transform.position + Vector3.right * moveDistance, moveSpeed));
                GameUIManager.Instance.HandleCorrectSwipe();
                found = true;
                break;
            }
        }
        if (!found)
        {
            GameUIManager.Instance.HandleWrongSwipe();
        }
    }

    IEnumerator SmoothMove(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }
}
