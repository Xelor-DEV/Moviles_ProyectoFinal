using UnityEngine;

public class ScrashRunner : MonoBehaviour
{
    public ScrashCounter counterData;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            counterData.AddPickup();
            Destroy(gameObject);
        }
    }
}
