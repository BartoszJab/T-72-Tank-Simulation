using UnityEngine;
using UnityEngine.UI;

public class TankCapsule : MonoBehaviour
{

    [Header("Capsule attributes")]
    public GameObject capsule;

    [Header("Rifle attributes")]
    public GameObject rifle;
    public float rifleRotationSpeed = 3f;
    public float maxXAngle = 20f;
    public float minXAngle = 0f;
    public float damping = 1f;

    public Image crosshairImage;
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
            crosshairImage.transform.position = cam.WorldToScreenPoint(rifle.transform.position + rifle.transform.forward * 50);
        } else {
            crosshairImage.enabled = false;
        }

        if (Input.GetKey(KeyCode.E)) {
            RotateRifle(maxXAngle);
        } else if (Input.GetKey(KeyCode.Q)) {
            //if (rifle.transform.localRotation.x < minXAngle) {
            RotateRifle(minXAngle);
            //}
            
        }

        
        
    }

    private void RotateCapsule() {
        Quaternion rot = cam.transform.rotation;
        rot.x = 0f;
        rot.z = 0f;
        capsule.transform.rotation = Quaternion.Slerp(capsule.transform.rotation, rot, Time.deltaTime * damping);
    }

    private void RotateRifle(float xAngleToRotate) {
        Quaternion rifleQuaternion = Quaternion.Euler(xAngleToRotate, 0f, 0f);
        rifle.transform.localRotation = Quaternion.Lerp(rifle.transform.localRotation, rifleQuaternion, Time.deltaTime * rifleRotationSpeed);
    }

}
