using UnityEngine;

public class RepairPartDrag : MonoBehaviour
{
    private bool isDragging = false;
    private bool isRepairing = false;
    private Vector3 offset;
    private Vector3 originalPosition;

    public float repairRate = 0.1f;
    public float scrapConsumptionRate = 1f;
    public float returnSpeed = 5f;
    public float repairDistance = 1.5f;

    [Header("VFX & SFX")]
    public ParticleSystem weldingParticles;
    public Transform weldingTarget;
    public AudioSource weldingSound;

    private Camera mainCamera;
    private RobotStats currentRobot;
    private float repairTimer = 0f;
    private float scrapAccumulator = 0f;
    private bool effectsActive = false;
    private bool hadSufficientScrapLastFrame = true;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;

        // Configuración inicial de partículas y sonido
        if (weldingParticles != null)
        {
            weldingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (weldingSound != null)
        {
            weldingSound.Stop();
            weldingSound.loop = true;
        }
    }

    void Update()
    {
        HandleTouchInput();

        bool hasSufficientScrap = false;
        bool shouldRepair = false;

        // Manejar la reparación continua
        if (isRepairing && currentRobot != null)
        {
            // Verificar si hay al menos 1 de chatarra
            hasSufficientScrap = currentRobot.CanRepair(1);

            if (hasSufficientScrap)
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
                        shouldRepair = true;
                    }
                }
                else
                {
                    // Reparación parcial sin consumir chatarra aún
                    currentRobot.RepairArmor(repairThisFrame, 0);
                    shouldRepair = true;
                }
            }
        }

        // Control de efectos de reparación
        bool shouldShowEffects = isRepairing && currentRobot != null && shouldRepair;

        // Solo activar efectos si podemos reparar realmente
        if (shouldShowEffects)
        {
            if (!effectsActive || !hadSufficientScrapLastFrame)
            {
                StartRepairEffects();
            }

            // Mover partículas al objetivo
            if (weldingTarget != null)
            {
                weldingParticles.transform.position = weldingTarget.position;
            }
        }
        else if (effectsActive)
        {
            StopRepairEffects();
        }

        // Guardar estado para el próximo frame
        hadSufficientScrapLastFrame = shouldRepair;

        // Regresar a posición original si no estamos arrastrando
        if (!isDragging && !isRepairing && Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
        }
    }

    void StartRepairEffects()
    {
        effectsActive = true;

        if (weldingParticles != null)
        {
            weldingParticles.Play();
        }

        if (weldingSound != null)
        {
            weldingSound.Stop();
            weldingSound.time = 0;
            weldingSound.Play();
        }
    }

    void StopRepairEffects()
    {
        effectsActive = false;

        if (weldingParticles != null)
        {
            weldingParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (weldingSound != null && weldingSound.isPlaying)
        {
            weldingSound.Stop();
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
                        isRepairing = false;
                        StopRepairEffects();
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
                        isRepairing = false;
                        StopRepairEffects();
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