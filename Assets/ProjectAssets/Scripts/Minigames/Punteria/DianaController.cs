using UnityEngine;

public class DianaController : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 moveRange = new Vector2(9.2f, 3.17f);

    private Vector3 targetPosition;
    private int phase = 0;

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            SetNewTarget();
    }

    public void Hit()
    {
        TargetGameUIManager.Instance.AddHit();
        phase = Mathf.Min(phase + 1, 2);
        speed += 0.5f;

        float minScale = 1;
        float scaleFactor = 0.9f;

        Vector3 newScale = transform.localScale * scaleFactor;
        if(newScale.x < minScale)
        {
            newScale = Vector3.one * minScale;
        }

        transform.localScale = newScale;
        transform.position = Vector3.zero;
        SetNewTarget();
    }

    void SetNewTarget()
    {
        switch (phase)
        {
            case 0:
                targetPosition = new Vector3(Random.Range(-moveRange.x, moveRange.x), 0, 0);
                break;
            case 1:
                targetPosition = new Vector3(0, Random.Range(-moveRange.y, moveRange.y), 0);
                break;
            case 2:
                targetPosition = new Vector3(Random.Range(-moveRange.x, moveRange.x), Random.Range(-moveRange.y, moveRange.y), 0);
                break;
        }
    }

    int CalculateReward()
    {
        return 3 + phase * 2; 
    }
}
