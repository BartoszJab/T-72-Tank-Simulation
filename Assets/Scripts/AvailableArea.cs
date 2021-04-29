using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AvailableArea : MonoBehaviour
{
    private bool isPlayerInside;
    public GameObject outsideAreaCanvas;
    public float timeToReturn = 10;
    public TMP_Text timeToReturnText;

    private void OnTriggerStay(Collider other) {
        if (other.tag == "PlayerBody") {
            isPlayerInside = true;
            timeToReturn = 10;
            outsideAreaCanvas.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "PlayerBody") {
            isPlayerInside = false;
            outsideAreaCanvas.SetActive(true);
        }

    }


    private void Update() {
        
        if (!isPlayerInside) {
            // player has time to return to the available area
            if (timeToReturn >= 0) {
                timeToReturn -= Time.deltaTime;
                timeToReturnText.text = "Time to return: " + Mathf.Round(timeToReturn);
            // player has been in unavailable area for too long
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        } 
    }
}
