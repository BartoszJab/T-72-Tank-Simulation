using UnityEngine;

public class TankCapsule : MonoBehaviour
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
    public float damping = 1f;

    public Transform crosshair;
    private Camera cam;

    void Start()
    {
        cameraHandlerScript = _camerasHandler.GetComponent<CameraHandlerScript>();
        cam = Camera.main;
    }

    void Update()
    {

        if (!cameraHandlerScript.isRotateMode) {
            RotateCapsule();
        }

        if (Input.GetKey(KeyCode.E)) {
            RotateRifle(maxXAngle);
        } else if (Input.GetKey(KeyCode.Q)) {
            //if (rifle.transform.localRotation.x < minXAngle) {
                RotateRifle(minXAngle);
            //}
            
        }

        // set position of a simple crosshair that indicates what the rifle is aiming at
        crosshair.transform.position = cam.WorldToScreenPoint(rifle.transform.position + rifle.transform.forward * 50);
        
    }

    private void RotateCapsule() {
        //var targetRotation = Quaternion.LookRotation(cam.transform.position - capsule.transform.position, Vector3.up);
        Quaternion rot = cam.transform.rotation;
        rot.x = 0f;
        rot.z = 0f;
        capsule.transform.rotation = Quaternion.Slerp(capsule.transform.rotation, rot, Time.deltaTime * damping);
        //capsule.transform.rotation = Quaternion.Slerp(capsule.transform.rotation, targetRotation, Time.deltaTime * damping);
    }

    private void RotateRifle(float xAngleToRotate) {
        Quaternion rifleQuaternion = Quaternion.Euler(xAngleToRotate, 0f, 0f);
        rifle.transform.localRotation = Quaternion.Lerp(rifle.transform.localRotation, rifleQuaternion, Time.deltaTime * rifleRotationSpeed);
    }

}
