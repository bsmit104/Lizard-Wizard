using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        Debug.Log("Cam started");
        Vector3 desiredPosition = new Vector3(transform.position.x, target.position.y, transform.position.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        if (smoothedPosition.y > transform.position.y)
        {
            transform.position = smoothedPosition;
        }
    }
}