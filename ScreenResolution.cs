using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    public float targetAspectWidth = 9f;
    public float targetAspectHeight = 16f;

    [SerializeField] private Camera mainCamera;

    private float initialOrthographicSize;

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }

        initialOrthographicSize = mainCamera.orthographicSize;
        AdjustCamera();
    }

    // Kamera an das tatsächliche Bildschirmformat (Aspect Ratio) anpassen
    void AdjustCamera()
    {
        float targetRatio = targetAspectWidth / targetAspectHeight;
        float currentRatio = (float)Screen.width / Screen.height;

        if (currentRatio < targetRatio)
        {
            float ratioDifference = currentRatio / targetRatio;
            mainCamera.orthographicSize = initialOrthographicSize / ratioDifference;
        }
        else
        {
            mainCamera.orthographicSize = initialOrthographicSize;
        }
    }
}