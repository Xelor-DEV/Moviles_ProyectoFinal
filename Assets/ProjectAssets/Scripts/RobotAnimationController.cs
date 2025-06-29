using UnityEngine;
using System.Collections;

public class RobotAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator robotAnimator;
    [SerializeField] private RobotStatsManager robotStats;

    [Header("Animation Settings")]
    [SerializeField] private float minDanceDelay = 5f;
    [SerializeField] private float maxDanceDelay = 8f;

    [Header("Animation Parameters")]
    [SerializeField] private string ArmorLowParam = "ArmorLow";
    [SerializeField] private string PowerLowParam = "PowerLow";
    [SerializeField] private string FunLowParam = "FunLow";

    [SerializeField] private string[] danceParams;
    private Coroutine danceRoutine;
    private string currentStateParam = "";
    private bool isInCriticalState = false;

    private void Start()
    {
        if (robotAnimator == null)
        {
            robotAnimator = GetComponent<Animator>();
        }

        if (robotStats == null)
        {
            robotStats = GetComponent<RobotStatsManager>();
        }

        StartDanceRoutine();
    }

    private void Update()
    {
        HandleStateAnimations();
    }

    private void HandleStateAnimations()
    {
        bool armorLow;
        if (robotStats.NeedsConfig.Armor < robotStats.NeedsConfig.Critical)
        {
            armorLow = true;
        }
        else
        {
            armorLow = false;
        }

        bool powerLow;
        if (robotStats.NeedsConfig.Power < robotStats.NeedsConfig.Critical)
        {
            powerLow = true;
        }
        else
        {
            powerLow = false;
        }

        bool funLow;
        if (robotStats.NeedsConfig.Fun < robotStats.NeedsConfig.Critical)
        {
            funLow = true;
        }
        else
        {
            funLow = false;
        }

        bool newCriticalState;
        if (armorLow == true || powerLow == true || funLow == true)
        {
            newCriticalState = true;
        }
        else
        {
            newCriticalState = false;
        }

        if (isInCriticalState == false && newCriticalState == true)
        {
            StopDanceRoutine();
        }
        else if (isInCriticalState == true && newCriticalState == false)
        {
            ResetAllStateBools();
            StartDanceRoutine();
        }

        isInCriticalState = newCriticalState;

        if (funLow == true)
        {
            SetStateAnimation(FunLowParam);
        }
        else if (powerLow == true)
        {
            SetStateAnimation(PowerLowParam);
        }
        else if (armorLow == true)
        {
            SetStateAnimation(ArmorLowParam);
        }
    }

    private void SetStateAnimation(string newStateParam)
    {
        if (currentStateParam == newStateParam)
        {
            return;
        }

        if (string.IsNullOrEmpty(currentStateParam) == false)
        {
            robotAnimator.SetBool(currentStateParam, false);
        }

        currentStateParam = newStateParam;
        robotAnimator.SetBool(currentStateParam, true);
    }

    private void ResetAllStateBools()
    {
        robotAnimator.SetBool(ArmorLowParam, false);
        robotAnimator.SetBool(PowerLowParam, false);
        robotAnimator.SetBool(FunLowParam, false);
        currentStateParam = "";
    }

    private void StartDanceRoutine()
    {
        if (danceRoutine != null)
        {
            StopCoroutine(danceRoutine);
        }
        danceRoutine = StartCoroutine(DanceBehavior());
    }

    private void StopDanceRoutine()
    {
        if (danceRoutine != null)
        {
            StopCoroutine(danceRoutine);
            danceRoutine = null;
        }
    }

    private IEnumerator DanceBehavior()
    {
        yield return new WaitForSeconds(Random.Range(minDanceDelay, maxDanceDelay));

        string selectedDance = danceParams[Random.Range(0, danceParams.Length)];
        robotAnimator.SetBool(selectedDance, true);

        yield return null;
        AnimatorStateInfo stateInfo = robotAnimator.GetCurrentAnimatorStateInfo(0);
        float danceDuration = stateInfo.length;

        yield return new WaitForSeconds(danceDuration);

        robotAnimator.SetBool(selectedDance, false);

        StartCoroutine(DanceBehavior());
    }
}