using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float riseSpeed = 0.5f; // Speed at which the water rises

    void Update()
    {
        // Move the water upwards over time
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
    }
}