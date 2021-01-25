using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] private GameObject _camerasHandler;
    private CameraHandlerScript cameraHandlerScript;

    public GameObject capsule;
    public float power = 5.0f;
    public float capsuleRotationSpeed = 45f;

    private Rigidbody rb;

    void Start()
    {
        cameraHandlerScript = _camerasHandler.GetComponent<CameraHandlerScript>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        Move();

        if (!cameraHandlerScript.isRotateMode) {
            RotateCapsule();
        }
        
    }

    private void RotateCapsule() {
        capsule.transform.Rotate(0f, (Input.GetAxis("Mouse X") * capsuleRotationSpeed * Time.deltaTime), 0f);
    }

    // temporary template movement function
    private void Move() {
        float moveForward = Input.GetAxis("Vertical") * power;

        rb.AddForce(Vector3.forward * moveForward);
    }
}
