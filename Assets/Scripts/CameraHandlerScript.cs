using UnityEngine;

public class CameraHandlerScript : MonoBehaviour
{
    private Camera mainCamera;
    
    [HideInInspector]
    public static bool isRotateMode = false;

    private void Start() {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            isRotateMode = !isRotateMode;
        }
    }
}
