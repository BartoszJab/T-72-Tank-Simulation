using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandlerScript : MonoBehaviour
{
    public Camera mainCamera;
    public Camera rotateModeCamera;
    
    [HideInInspector]
    public bool isRotateMode = false;

    void Start()
    {
        mainCamera.enabled = true;
        rotateModeCamera.enabled = false;
    }

    void Update()
    {
        // switch between main 'game mode' camera and 'rotate mode' camera
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            isRotateMode = !isRotateMode;

            ChangeCameras();

            // set second camera position to the position of the main camera
            rotateModeCamera.transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
        }
    }

    private void ChangeCameras() {
        mainCamera.enabled = !mainCamera.enabled;
        rotateModeCamera.enabled = !rotateModeCamera.enabled;
    }
}
