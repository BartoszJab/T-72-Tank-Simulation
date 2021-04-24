using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BunkerIndicator : MonoBehaviour
{
    //public Image markBunkerImage;
    public TMP_Text targetDistanceText;
    public Transform targetPos;
    public Image img;
    public float yOffset;

    private Transform tank;
    private Camera cam;
    private float distance;


    private void Start() {
        cam = Camera.main;
        tank = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {

        distance = Vector3.Distance(transform.position, tank.position) - 15f;

        Vector2 pos = cam.WorldToScreenPoint(targetPos.position);

        if (Vector3.Dot((transform.position - cam.transform.position), cam.transform.forward) < 0 || CameraHandlerScript.isAmingMode) {
            // behind player
            img.enabled = false;
            targetDistanceText.enabled = false;
        } else {
            // in front of player
            img.enabled = true;
            targetDistanceText.enabled = true;
        }
        pos.y += yOffset;
        img.transform.position = pos;
        targetDistanceText.text = distance.ToString("F0");

    }
}

   

