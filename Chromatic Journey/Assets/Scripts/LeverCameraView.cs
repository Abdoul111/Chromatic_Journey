using UnityEngine;
using UnityEngine.UI;

public class LeverCameraView : MonoBehaviour
{
    [Header("Camera Settings")]
    public Vector2 viewportSize = new Vector2(200f, 150f); // Size in pixels
    public Vector2 viewportOffset = new Vector2(-220f, 20f); // Offset from bottom-right corner
    public float displayDuration = 3f;

    private Camera mainCamera;
    private Camera miniCamera;
    private RawImage displayImage;
    private RenderTexture renderTexture;
    private float displayTimer;
    private CanvasGroup canvasGroup;
    private bool isDisplaying;
    private MovingLever currentLever;

    public GameObject borderPanel;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        SetupCamera();
        SetupUI();
        HideDisplay();
    }

    private void SetupCamera()
    {
        GameObject cameraObj = new GameObject("LeverMiniCamera");
        cameraObj.transform.SetParent(transform);

        // Setup camera and copy properties from main camera
        miniCamera = cameraObj.AddComponent<Camera>();
        CopyMainCameraSettings();

        // Create render texture for the mini view
        renderTexture = new RenderTexture((int)viewportSize.x, (int)viewportSize.y, 16);
        miniCamera.targetTexture = renderTexture;
        miniCamera.enabled = false;
    }

    private void CopyMainCameraSettings()
    {
        // Copy all relevant camera settings
        miniCamera.orthographic = mainCamera.orthographic;
        miniCamera.orthographicSize = mainCamera.orthographicSize;
        miniCamera.backgroundColor = mainCamera.backgroundColor;
        miniCamera.clearFlags = mainCamera.clearFlags;
        miniCamera.cullingMask = mainCamera.cullingMask;
        miniCamera.depth = mainCamera.depth;
        miniCamera.farClipPlane = mainCamera.farClipPlane;
        miniCamera.nearClipPlane = mainCamera.nearClipPlane;
        miniCamera.fieldOfView = mainCamera.fieldOfView;

        // Copy any post-processing or rendering settings if using URP/HDRP
        miniCamera.allowHDR = mainCamera.allowHDR;
        miniCamera.allowMSAA = mainCamera.allowMSAA;
    }

    private void SetupUI()
    {
        // Create UI container
        GameObject uiContainer = new GameObject("LeverCameraUI");
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No canvas found in scene!");
            return;
        }
        uiContainer.transform.SetParent(canvas.transform);

        // Setup RectTransform
        RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
        rectTransform.sizeDelta = viewportSize;
        rectTransform.anchorMin = new Vector2(1, 0); // Bottom right anchor
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(1, 0);
        rectTransform.anchoredPosition = viewportOffset;

        // Add Canvas Group for fading
        canvasGroup = uiContainer.AddComponent<CanvasGroup>();

        // Setup RawImage
        GameObject imageObj = new GameObject("CameraView");
        imageObj.transform.SetParent(uiContainer.transform);
        displayImage = imageObj.AddComponent<RawImage>();
        displayImage.texture = renderTexture;

        // Setup image RectTransform
        RectTransform imageTransform = imageObj.GetComponent<RectTransform>();
        imageTransform.anchorMin = Vector2.zero;
        imageTransform.anchorMax = Vector2.one;
        imageTransform.sizeDelta = Vector2.zero;
        imageTransform.anchoredPosition = Vector2.zero;
    }

    public void ShowLeverMovement(MovingLever lever)
    {
        currentLever = lever;

        // Calculate the center point between start and end positions
        Vector2 centerPoint = (lever.startPoint.position + lever.endPoint.position) / 2f;

        // Position mini camera with same Z distance as main camera
        miniCamera.transform.position = new Vector3(
            centerPoint.x,
            centerPoint.y,
            mainCamera.transform.position.z
        );

        // Match the main camera's size for consistent view
        if (mainCamera.orthographic)
        {
            miniCamera.orthographicSize = mainCamera.orthographicSize;
        }

        displayTimer = displayDuration;
        isDisplaying = true;
        ShowDisplay();
        miniCamera.enabled = true;
    }

    private void LateUpdate()
    {
        if (isDisplaying)
        {
            if (currentLever != null)
            {
                // Update camera to follow lever while maintaining main camera's view properties
                Vector3 leverPos = currentLever.transform.position;
                miniCamera.transform.position = new Vector3(
                    leverPos.x,
                    leverPos.y,
                    mainCamera.transform.position.z
                );
            }

            displayTimer -= Time.deltaTime;

            // Fade out near the end
            if (displayTimer <= 0.5f)
            {
                canvasGroup.alpha = displayTimer / 0.5f;
            }

            if (displayTimer <= 0)
            {
                HideDisplay();
                isDisplaying = false;
                currentLever = null;
            }
        }
    }

    private void ShowDisplay()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        borderPanel.SetActive(true);
    }

    private void HideDisplay()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        miniCamera.enabled = false;

        borderPanel.SetActive(false);

    }

    private void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
        }
    }
}