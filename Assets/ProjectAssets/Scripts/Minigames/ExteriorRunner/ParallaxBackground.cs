using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float scrollSpeed = 2f; 
    public float backgroundWidth = 20f;

    private Transform[] backgrounds;

    void Start()
    {
     
        backgrounds = new Transform[transform.childCount];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * scrollSpeed * Time.deltaTime;

       
            if (bg.position.x < -backgroundWidth)
            {
                float rightMostX = GetRightMostX();
                bg.position = new Vector3(rightMostX + backgroundWidth, bg.position.y, bg.position.z);
            }
        }
    }

    float GetRightMostX()
    {
        float maxX = float.MinValue;
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > maxX)
                maxX = bg.position.x;
        }
        return maxX;
    }
}
