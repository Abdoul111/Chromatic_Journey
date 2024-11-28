using UnityEngine;
using UnityEngine.UI;

public class LeverCameraView : MonoBehaviour
{
    [Header("Camera Settings")]
    public Vector2 viewportSize = new Vector2(640f, 360f); // Size in pixels
    public Vector2 viewportOffset = new Vector2(-15f, 15f); // Offset from bottom-right corner
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

        // Setup RectTransform for the UI container
        RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
        rectTransform.sizeDelta = viewportSize;
        rectTransform.anchorMin = new Vector2(1, 0); // Bottom right anchor
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(1, 0);
        rectTransform.anchoredPosition = viewportOffset;

        // Add Canvas Group for fading
        canvasGroup = uiContainer.AddComponent<CanvasGroup>();

        // Setup RawImage as a child of the UI container
        GameObject imageObj = new GameObject("CameraView");
        imageObj.transform.SetParent(uiContainer.transform);

        // Setup RawImage for displaying the camera view
        displayImage = imageObj.AddComponent<RawImage>();
        displayImage.texture = renderTexture;

        // Setup RectTransform for the RawImage
        RectTransform imageTransform = imageObj.GetComponent<RectTransform>();
        imageTransform.anchorMin = Vector2.zero; // Stretch to fit parent
        imageTransform.anchorMax = Vector2.one;
        imageTransform.sizeDelta = Vector2.zero; // Match parent size
        imageTransform.anchoredPosition = Vector2.zero;

        // Optionally add an AspectRatioFitter to maintain aspect ratio
        AspectRatioFitter aspectFitter = imageObj.AddComponent<AspectRatioFitter>();
        aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        aspectFitter.aspectRatio = viewportSize.x / viewportSize.y;
    }


    public void ShowLeverMovement(MovingLever lever)
    {
        // If already displaying, immediately override the current lever and timer
        if (isDisplaying)
        {
            // Clear any lingering display state
            HideDisplay();
        }

        currentLever = lever;

        // Calculate the center point between start and end positions
        Vector2 centerPoint = lever.startPoint.position;

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

        // Reset display timer and state
        displayTimer = displayDuration;
        isDisplaying = true;

        // Show the display and enable the mini camera
        ShowDisplay();
        miniCamera.enabled = true;
    }

    private void LateUpdate()
    {
        if (isDisplaying)
        {
            displayTimer -= Time.deltaTime;

            // Fade out near the end
            if (displayTimer <= 0.5f)
            {
                canvasGroup.alpha = displayTimer / 0.5f;
            }

            // Hide display when the timer ends
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
        // Disable mini camera and hide UI elements
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        miniCamera.enabled = false;
        borderPanel.SetActive(false);

        // Clear any remaining state
        currentLever = null;
    }


    private void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
        }
    }
}