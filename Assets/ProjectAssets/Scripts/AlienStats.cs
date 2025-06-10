using UnityEngine;
using UnityEngine.UI;
public class AlienStats : MonoBehaviour
{
    
    public float hunger = 100f;
    public float happiness = 100f;
    public float hygiene = 100f;
    public float energy = 100f;

    [Header("UI")]
    public Slider hungerSlider;
    void Start()
    {
        hungerSlider.maxValue = 100; 
        hungerSlider.value = hunger;
    }
    void Update()
    {
        hunger = Mathf.Clamp(hunger - Time.deltaTime, 0.1f, 100);
        hungerSlider.value = hunger; 
    }

  
    public void Feed(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, 100);
        Debug.Log("ALimentado, Habre ahora: " + hunger);
    }
}
