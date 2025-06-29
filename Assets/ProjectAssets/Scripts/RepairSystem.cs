using UnityEngine;

public class RepairSystem : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float returnSpeed = 5f;

    [Header("Repair Settings")]
    [SerializeField] private float repairRate = 0.1f;
    [SerializeField] private float scrapConsumptionRate = 1f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 1.5f;
    [SerializeField] private Vector3 detectionOffset = Vector3.zero;
    [SerializeField] private string robotTag = "Robot";
    [SerializeField] private Color normalGizmoColor = Color.yellow;
    [SerializeField] private Color detectionGizmoColor = Color.green;

    [Header("VFX & SFX")]
    [SerializeField] private ParticleSystem weldingParticles;
    [SerializeField] private Transform weldingTarget;
    [SerializeField] private int weldingSoundIndex = -1;
    [SerializeField] private bool loopWeldingSound = true;

    private Vector3 offset;
    private Vector3 originalPosition;
    private bool isDragging = false;
    private bool isRepairing = false;
    private RobotStatsManager currentRobot;
    private float scrapAccumulator = 0f;
    private bool effectsActive = false;
    private bool hadSufficientScrapLastFrame = true;

    private void Start()
    {
        originalPosition = transform.position;

        if (weldingParticles != null)
        {
            weldingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        AudioSource weldingSoundSource = AudioManager.Instance.GetAudioSourceByIndex(weldingSoundIndex);
        if (weldingSoundSource != null)
        {
            weldingSoundSource.loop = loopWeldingSound;
        }
    }

    private void Update()
    {
        HandleRepairProcess();
        HandleReturnToOriginalPosition();
    }

    public void OnDragStart(Vector3 startPosition)
    {
        isDragging = true;
        offset = transform.position - new Vector3(startPosition.x, startPosition.y, originalPosition.z);
        isRepairing = false;
        StopRepairEffects();
    }

    public void OnDrag(Vector3 currentPosition)
    {
        if (isDragging)
        {
            Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y, originalPosition.z) + offset;
            transform.position = newPosition;
            CheckRobotProximity();
        }
    }


    public void OnDragEnd()
    {
        if (isDragging)
        {
            isDragging = false;
            isRepairing = false;
            StopRepairEffects();
        }
    }

    private void HandleRepairProcess()
    {
        if (!isRepairing || currentRobot == null) return;

        bool hasSufficientScrap = currentRobot.CanRepair(1);
        bool shouldRepair = false;

        if (hasSufficientScrap)
        {

            float repairPerSecond = currentRobot.NeedsConfig.armorRepairPerScrap * scrapConsumptionRate;
            float repairThisFrame = repairPerSecond * Time.deltaTime;

            scrapAccumulator += scrapConsumptionRate * Time.deltaTime;

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
                currentRobot.RepairArmor(repairThisFrame, 0);
                shouldRepair = true;
            }
        }

        // Execute Effects
        bool shouldShowEffects = shouldRepair;

        if (shouldShowEffects)
        {
            if (!effectsActive || !hadSufficientScrapLastFrame)
            {
                StartRepairEffects();
            }

            if (weldingTarget != null)
            {
                weldingParticles.transform.position = weldingTarget.position;
            }
        }
        else if (effectsActive)
        {
            StopRepairEffects();
        }

        hadSufficientScrapLastFrame = shouldRepair;
    }

    private void HandleReturnToOriginalPosition()
    {
        if (!isDragging && !isRepairing && Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            Vector3 targetPosition = new Vector3(
                originalPosition.x,
                originalPosition.y,
                transform.position.z
            );

            transform.position = Vector3.Lerp(transform.position, targetPosition, returnSpeed * Time.deltaTime);
        }
    }

    private void CheckRobotProximity()
    {
        Vector3 detectionCenter = transform.position + detectionOffset;
        Collider[] hits = Physics.OverlapSphere(detectionCenter, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(robotTag))
            {
                RobotStatsManager robot = hit.GetComponent<RobotStatsManager>();
                if (robot != null)
                {
                    currentRobot = robot;
                    isRepairing = true;
                    return;
                }
            }
        }

        isRepairing = false;
        currentRobot = null;
    }

    private void StartRepairEffects()
    {
        effectsActive = true;

        if (weldingParticles != null)
        {
            weldingParticles.Play();
        }

        if (weldingSoundIndex >= 0)
        {
            AudioSource source = AudioManager.Instance.GetAudioSourceByIndex(weldingSoundIndex);
            source.clip = AudioManager.Instance.SfxClips[weldingSoundIndex];
            source.Play();
        }
    }

    private void StopRepairEffects()
    {
        effectsActive = false;

        if (weldingParticles != null)
        {
            weldingParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        AudioSource source = AudioManager.Instance.GetAudioSourceByIndex(weldingSoundIndex);
        source.Stop();
    }

    private void OnDrawGizmos()
    {
        if (currentRobot != null)
        {
            Gizmos.color = detectionGizmoColor;
        }
        else
        {
            Gizmos.color = normalGizmoColor;
        }

        Vector3 center = transform.position + detectionOffset;
        Gizmos.DrawWireSphere(center, detectionRadius);
    }
}