using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public enum OrientationMode
    {
        Portrait,
        Landscape
    }

    [Header("Orientation Settings")]
    [SerializeField] private OrientationMode orientation;

    private void Start()
    {
        if (orientation == OrientationMode.Portrait)
        {
            SetPortraitMode();
        }
        else if (orientation == OrientationMode.Landscape)
        {
            SetLandscapeMode();
        }
    }

    public void SetPortraitMode()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void SetLandscapeMode()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}