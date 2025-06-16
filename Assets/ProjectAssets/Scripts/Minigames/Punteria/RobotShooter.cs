using UnityEngine;

public class RobotShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 20f;
    private bool hasShot = false;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !hasShot)
        {
            Vector3 touchWorldPos = GetTouchWorldPosition(Input.GetTouch(0).position);
            Shoot(touchWorldPos);
            hasShot = true;
        }
    }

    Vector3 GetTouchWorldPosition(Vector3 touchPosition)
    {
        touchPosition.z = 10f;
        return Camera.main.ScreenToWorldPoint(touchPosition);
    }

    void Shoot(Vector3 targetPos)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        
        Projectile projScript = projectile.GetComponent<Projectile>();
        projScript.shooter = this;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 direction = (targetPos - firePoint.position).normalized;
        rb.linearVelocity = direction * shootForce;
    }

    public void ResetShot() => hasShot = false;
}
