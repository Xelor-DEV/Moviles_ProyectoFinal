using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UI_NeedsBars : MonoBehaviour
{
    [Serializable]
    public class NeedBar
    {
        public Image barImage;
        public Color startColor = Color.green;
        public Color endColor = Color.red;
        [HideInInspector] public float currentValue;
        [HideInInspector] public float targetValue;
        [HideInInspector] public float velocity;
        [HideInInspector] public Coroutine animationRoutine;
    }

    [SerializeField] private NeedBar[] bars;

    public void SetBarValue(int index, float value)
    {
        if (index < 0 || index >= bars.Length) return;

        NeedBar bar = bars[index];
        bar.targetValue = Mathf.Clamp01(value);
        bar.currentValue = bar.targetValue;

        if (bar.animationRoutine != null)
        {
            StopCoroutine(bar.animationRoutine);
        }

        bar.barImage.fillAmount = bar.targetValue;
        bar.barImage.color = Color.Lerp(bar.endColor, bar.startColor, bar.targetValue);
    }

    public void SetBarValueAnimated(int index, float value, float duration = 0.2f)
    {
        if (index < 0 || index >= bars.Length) return;

        NeedBar bar = bars[index];
        bar.targetValue = Mathf.Clamp01(value);

        if (bar.animationRoutine != null)
        {
            StopCoroutine(bar.animationRoutine);
        }

        bar.animationRoutine = StartCoroutine(AnimateBar(bar, duration));
    }

    private IEnumerator AnimateBar(NeedBar bar, float duration)
    {
        float startValue = bar.barImage.fillAmount;
        float endValue = bar.targetValue;
        float currentVelocity = 0f;
        float currentValue = startValue;

        while (Mathf.Abs(currentValue - endValue) > 0.001f)
        {
            currentValue = Mathf.SmoothDamp(
                currentValue,
                endValue,
                ref currentVelocity,
                duration
            );

            bar.barImage.fillAmount = currentValue;
            bar.barImage.color = Color.Lerp(
                bar.endColor,
                bar.startColor,
                currentValue
            );

            yield return null;
        }

        bar.barImage.fillAmount = endValue;
        bar.barImage.color = Color.Lerp(bar.endColor, bar.startColor, endValue);
        bar.animationRoutine = null;
    }
}