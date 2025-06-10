using UnityEngine;

public class RepairPartDrag : MonoBehaviour
{
    private bool isDragging = false;
    private bool isRepairing = false;
    private Vector3 offset;
    private Vector3 originalPosition;

    public float repairRate = 0.1f; // Armadura reparada por segundo
    public float scrapConsumptionRate = 1f; // Chatarra consumida por segundo
    public float returnSpeed = 5f;
    public float repairDistance = 1.5f;

    private Camera mainCamera;
    private RobotStats currentRobot;
    private float repairTimer = 0f;
    private float scrapAccumulator = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    void Update()
    {
        HandleTouchInput();

        // Manejar la reparación continua
        if (isRepairing && currentRobot != null)
        {
            repairTimer += Time.deltaTime;

            // Calcular la reparación y consumo de chatarra
            float repairThisFrame = repairRate * Time.deltaTime;
            scrapAccumulator += scrapConsumptionRate * Time.deltaTime;

            // Aplicar reparación cuando acumulamos suficiente chatarra
            if (scrapAccumulator >= 1f)
            {
                int scrapToConsume = Mathf.FloorToInt(scrapAccumulator);
                if (currentRobot.CanRepair(scrapToConsume))
                {
                    currentRobot.RepairArmor(repairThisFrame * scrapToConsume, scrapToConsume);
                    scrapAccumulator -= scrapToConsume;
                }
                else
                {
                    // No hay suficiente chatarra, detener reparación
                    isRepairing = false;
                }
            }
            else
            {
                // Reparación parcial sin consumir chatarra aún
                currentRobot.RepairArmor(repairThisFrame, 0);
            }
        }
        else
        {
            scrapAccumulator = 0f; // Resetear acumulador si no estamos reparando
        }

        // Regresar a posición original si no estamos arrastrando
        if (!isDragging && !isRepairing && Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit, 100f) && hit.collider.gameObject == gameObject)
                    {
                        isDragging = true;
                        offset = transform.position - hit.point;
                        isRepairing = false; // Resetear estado de reparación
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        if (Physics.Raycast(ray, out hit, 100f))
                        {
                            transform.position = hit.point + offset;
                        }
                        CheckRobotProximity();
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false;
                        isRepairing = false; // Detener reparación al soltar
                    }
                    break;
            }
        }
    }

    void CheckRobotProximity()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, repairDistance);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Robot"))
            {
                RobotStats robot = hit.GetComponent<RobotStats>();
                if (robot != null)
                {
                    currentRobot = robot;
                    isRepairing = true;
                    return;
                }
            }
        }

        // Si no encontramos robot, detenemos la reparación
        isRepairing = false;
        currentRobot = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, repairDistance);
    }
}