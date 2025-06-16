using UnityEngine;

public class Projectile : MonoBehaviour
{
    public RobotShooter shooter;
    public float lifeTime = 2f;
    private bool hashit = false;

    void Start()
    {
        Invoke(nameof(HandleMiss), lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Target"))
        {
            hashit = true;
            other.GetComponent<DianaController>().Hit();
            shooter.ResetShot(); 
            Destroy(gameObject);
        }

    }

    void HandleMiss()
    {
        if (!hashit)
        {
            TargetGameUIManager.Instance.ShowGameOverPanel();
            Destroy(gameObject);
        }
    }
}
