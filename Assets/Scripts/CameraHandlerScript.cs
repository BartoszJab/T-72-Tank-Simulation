using UnityEngine;

public class CameraHandlerScript : MonoBehaviour
{
    private Camera mainCamera;
    
    [HideInInspector]
    public static bool isRotateMode = false;
    public static bool isMainMode = true;
    public static bool isAmingMode = false;

    public Camera aimCamera;

    private Vector3 prevMainCameraPosition;

    private void Start() {
        mainCamera = Camera.main;
        aimCamera.enabled = false;

        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            if (isAmingMode) return;
            isAmingMode = false;
            isRotateMode = !isRotateMode;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            if (isRotateMode) return;
            isAmingMode = !isAmingMode;

            if (isAmingMode) {
                mainCamera.enabled = false;
                aimCamera.enabled = true;
            } else {
                mainCamera.enabled = true;
                aimCamera.enabled = false;
            }
        }

    }
}
