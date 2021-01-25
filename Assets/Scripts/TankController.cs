using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] private GameObject _camerasHandler;
    private CameraHandlerScript cameraHandlerScript;

    [Header("Capsule attributes")]
    public GameObject capsule;
    public float capsuleRotationSpeed = 45f;

    [Header("Rifle attributes")]
    public GameObject rifle;
    public float rifleRotationSpeed = 3f;
    public float maxXAngle = 20f;
    public float minXAngle = 0f;

    [Header("Tank attributes")]
    public float power = 5.0f;
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

        if (Input.GetKey(KeyCode.E)) {
            RotateRifle(maxXAngle);
        } else if (Input.GetKey(KeyCode.Q)) {
            if (rifle.transform.localRotation.x < minXAngle) {
                RotateRifle(minXAngle);
            }
            
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

    private void RotateRifle(float xAngleToRotate) {
        Quaternion rifleQuaternion = Quaternion.Euler(xAngleToRotate, 0f, 0f);
        rifle.transform.localRotation = Quaternion.Slerp(rifle.transform.localRotation, rifleQuaternion, Time.deltaTime * rifleRotationSpeed);
    }

}
