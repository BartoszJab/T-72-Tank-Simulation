using UnityEngine;
using UnityEngine.UI;

public class TankCapsule : MonoBehaviour
{
    public GameObject body;

    [Header("Capsule attributes")]
    public GameObject capsule;

    [Header("Rifle attributes")]
    public GameObject rifle;
    public float rifleRotationSpeed = 3f;
    public float maxXAngle = 20f;
    public float minXAngle = 0f;
    public float damping = 1f;

    [Header("Crosshair")]
    public Image crosshairImage;
    public float crosshairDistance = 20f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {

        if (!CameraHandlerScript.isRotateMode) {
            crosshairImage.enabled = true;
            RotateCapsule();

            // set position of a crosshair that indicates what the rifle is aiming at
            crosshairImage.transform.position = cam.WorldToScreenPoint(rifle.transform.position + rifle.transform.forward * crosshairDistance);
        } else {
            crosshairImage.enabled = false;
        }

        if (Input.GetKey(KeyCode.E)) {
            RotateRifle(maxXAngle);
        } else if (Input.GetKey(KeyCode.Q)) {
            RotateRifle(minXAngle);
        }

    }

    private void RotateCapsule() {
        Quaternion rot = cam.transform.rotation;

        capsule.transform.rotation = Quaternion.Slerp(capsule.transform.rotation, rot, Time.deltaTime * damping); ;
        capsule.transform.localEulerAngles = new Vector3(0, capsule.transform.localEulerAngles.y, 0f);
    }

    private void RotateRifle(float xAngleToRotate) {
        Quaternion rifleQuaternion = Quaternion.Euler(xAngleToRotate, 0f, 0f);
        rifle.transform.localRotation = Quaternion.Lerp(rifle.transform.localRotation, rifleQuaternion, Time.deltaTime * rifleRotationSpeed);
    }

}
