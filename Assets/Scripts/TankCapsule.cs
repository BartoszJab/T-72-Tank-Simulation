using UnityEngine;
using UnityEngine.UI;

public class TankCapsule : MonoBehaviour
{

    [Header("Capsule attributes")]
    public GameObject capsule;
    public Image capsuleImage;
    public float damping = 1f;

    [Header("Rifle attributes")]
    public GameObject rifle;
    public float rifleRotationSpeed = 3f;
    public float xMinAngle = -8;
    public float xMaxAngle = 3;
    public float xOffset;

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
            RotateCapsule();

            // set position of a crosshair that indicates what the rifle is aiming at
            crosshairImage.transform.position = cam.WorldToScreenPoint(rifle.transform.position + rifle.transform.forward * crosshairDistance);
            RotateRifle();
        }     
    }

    private void RotateCapsule() {
        Quaternion rot = cam.transform.rotation;

        capsule.transform.rotation = Quaternion.Slerp(capsule.transform.rotation, rot, Time.deltaTime * damping);
        capsule.transform.localEulerAngles = new Vector3(0f, capsule.transform.localEulerAngles.y, 0f);

        capsuleImage.transform.localEulerAngles = new Vector3(0f, 0f, -capsule.transform.localEulerAngles.y);
    }

    
    private void RotateRifle() {
        rifle.transform.localEulerAngles = new Vector3(Mathf.Clamp(cam.transform.localEulerAngles.x + xOffset, xMinAngle, xMaxAngle), 0f, 0f);
    }

}
