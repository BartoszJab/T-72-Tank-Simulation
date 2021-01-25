using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModeScript : MonoBehaviour
{
    public Transform tankTransform;

    public float rotateSpeed = 45f;
    
    // rotate camera around tank using mouse X axis
    void Update()
    {
        transform.RotateAround(tankTransform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
    }
}
