using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPosX, startPosY, lengthX, lengthY;
    public GameObject cam;
    public float parallaxEffectX; // The speed at which the background should move relative to the camera for X
    public float parallaxEffectY; // The speed at which the background should move relative to the camera for Y

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = GetComponent<RectTransform>().anchoredPosition.x;
        lengthY = GetComponent<RectTransform>().anchoredPosition.y;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate distance background move based on cam movement
        float distanceX = cam.transform.position.x * parallaxEffectX; // 0 = move with cam || 1 = won't move || 0.5 = half
        float movementX = cam.transform.position.x * (1 - parallaxEffectX);

        float distanceY = cam.transform.position.y * parallaxEffectY; // 0 = move with cam || 1 = won't move || 0.5 = half
        float movementY = cam.transform.position.y * (1 - parallaxEffectY);

        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        // if background has reached the end of its length adjust its position for infinite scrolling
        if (movementX > startPosX + lengthX)
        {
            startPosX += lengthX;
        }
        else if (movementX < startPosX - lengthX)
        {
            startPosX -= lengthX;
        }

        // if background has reached the end of its length adjust its position for infinite scrolling
        if (movementY > startPosY + lengthY)
        {
            startPosY += lengthY;
        }
        else if (movementY < startPosY - lengthY)
        {
            startPosY -= lengthY;
        }
    }
}
