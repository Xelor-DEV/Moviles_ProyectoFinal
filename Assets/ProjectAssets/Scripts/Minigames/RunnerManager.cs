using UnityEngine;
using TMPro;
public class RunnerManager : MonoBehaviour
{
    [Header("Referencias")]
    public ScrashCounter counterData; 
    public TextMeshProUGUI pickupText;

    void Update()
    {
        pickupText.text = "Recolectados: " + counterData.pickupsCollected.ToString();
    }
}
