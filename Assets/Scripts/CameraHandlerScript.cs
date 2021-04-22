using UnityEngine;
using UnityEngine.UI;

public class CameraHandlerScript : MonoBehaviour
{
    private Camera mainCamera;
    
    [HideInInspector]
    public static bool isRotateMode = false;
    public static bool isMainMode = true;
    public static bool isAmingMode = false;

    public Camera aimCamera;
    public Image crosshairImage;
    public Image aimLayoutImage;

    private Vector3 prevMainCameraPosition;

    private void Start() {
        mainCamera = Camera.main;
        aimCamera.enabled = false;
        aimLayoutImage.enabled = false;

        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            if (isAmingMode) return;
            isMainMode = !isMainMode;
            isRotateMode = !isRotateMode;

            crosshairImage.enabled = !crosshairImage.enabled;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            if (isRotateMode) return;
            isAmingMode = !isAmingMode;
            isMainMode = !isMainMode;

            aimCamera.enabled = !aimCamera.enabled;
            mainCamera.enabled = !mainCamera.enabled;
            crosshairImage.enabled = !crosshairImage.enabled;
            aimLayoutImage.enabled = !aimLayoutImage.enabled;

            
        }

        Debug.Log("Main mode: " + isMainMode + ", Aiming mode: " + isAmingMode + ", RotateMode: " + isRotateMode);
    }
}
