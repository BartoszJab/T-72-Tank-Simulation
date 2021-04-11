using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform tankTransform;

    [SerializeField] private float rotateModeMinAngleY;
    [SerializeField] private float rotateModeMaxAngleY;

    [SerializeField] private float aimModeMinAngleY = -25f;
    [SerializeField] private float aimModeMaxAngleY = -10f;

    [SerializeField] private float distance = 17f; // distance between player and the camera
    [SerializeField] private float xMouseSensitivity = 2f;
    [SerializeField] private float yMouseSensitivity = 1f;

    private float currentX = 0f;
    private float currentY = 0f;

    void Update() {
        // get mouse input and clamp it on Y axis
        currentX += Input.GetAxis("Mouse X") * xMouseSensitivity;
        currentY += Input.GetAxis("Mouse Y") * yMouseSensitivity;

        if (CameraHandlerScript.isRotateMode)
            currentY = Mathf.Clamp(currentY, rotateModeMinAngleY, rotateModeMaxAngleY);
        else
            currentY = Mathf.Clamp(currentY, aimModeMinAngleY, aimModeMaxAngleY);
    }

    // movement of tank is done inside update function so we want to set our camera after that
    void LateUpdate() {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
        // to the position of the tank we add the rotated 'dir' vector
        transform.position = tankTransform.position + rotation * dir;
        transform.LookAt(tankTransform.position);
    }
}
