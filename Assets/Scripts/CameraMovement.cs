using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform tankTransform;

    [SerializeField] private float minAngleY;
    [SerializeField] private float maxAngleY;


    [SerializeField] private float distance = 17f; // distance between player and the camera
    [SerializeField] private float mouseSensitivity = 2f;

    private float currentX = 0f;
    private float currentY = 0f;

    void Update() {
        // get mouse input and clamp it on Y axis
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        currentY = Mathf.Clamp(currentY, minAngleY, maxAngleY);
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
