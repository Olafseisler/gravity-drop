using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.125f;
    
    private Vector3 velocity = Vector3.zero;
    
    private void Start()
    {
        target = GameObject.Find("Ball").transform;
    }

    void Update()
    {
        // Smoothly follow the target using SmoothDamp
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}