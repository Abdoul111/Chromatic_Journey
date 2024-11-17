using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Camera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 offset = new Vector3(0, 0, -10);
    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position + offset;
        }
    }
}
