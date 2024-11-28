using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private float targetFOV;
    private float transitionSpeed = 1f;
    private bool isTransitioning = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetFOV = cam.fieldOfView; // Start with the current FOV
    }

    void Update()
    {
        if (isTransitioning)
        {
            // Smoothly transition to the target FOV
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, transitionSpeed * Time.deltaTime);

            // Stop transitioning if the FOV is close enough to the target
            if (Mathf.Abs(cam.fieldOfView - targetFOV) < 0.1f)
            {
                cam.fieldOfView = targetFOV;
                isTransitioning = false;
            }
        }
    }

    public void SetTargetFOV(float newFOV)
    {
        targetFOV = newFOV;
        isTransitioning = true;
    }
}
