using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float rotationSpeed = 30.0f; // Rotation speed in degrees per second

    // Update is called once per frame
    void Update()
    {
        // Rotate the GameObject around the X-axis
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
