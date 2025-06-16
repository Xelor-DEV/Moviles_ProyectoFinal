using UnityEngine;

public class test : MonoBehaviour
{
    public Texture2D baseMapTexture;
    public Texture2D emissionMapTexture;
    public Material material;

    void Start()
    {

        // Cambiar Base Map
        if (baseMapTexture != null)
            material.SetTexture("_BaseMap", baseMapTexture);

        // Cambiar Emission Map y activar emisión
        if (emissionMapTexture != null)
        {
            material.SetTexture("_EmissionMap", emissionMapTexture);
        }
    }
}