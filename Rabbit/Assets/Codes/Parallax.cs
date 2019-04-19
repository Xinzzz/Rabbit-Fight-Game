using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public bool moveInOppositeDirection;
    public bool moveParallax = true;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void OnEnable()
    {
        GameObject gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraTransform = gameCamera.transform;
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (!moveParallax)
            return;

        Vector3 distance = cameraTransform.position - previousCameraPosition;
        transform.position += Vector3.Scale(distance, new Vector3(speedX, speedY));

        previousCameraPosition = cameraTransform.position;
    }

}
