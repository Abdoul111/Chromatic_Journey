using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVTrigger : MonoBehaviour
{
    public float newFOV = 60f; // Target FOV when entering the zone
    public float defaultFOV = 45f; // Default FOV when exiting the zone

    private CameraController cameraController;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraController.SetTargetFOV(newFOV);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraController.SetTargetFOV(defaultFOV);
        }
    }
}
