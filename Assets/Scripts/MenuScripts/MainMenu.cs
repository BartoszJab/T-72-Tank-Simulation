using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Awake() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayScenario() {
        SceneManager.LoadScene(Level.Scenario);
    }

    public void PlayFreeride() {
        SceneManager.LoadScene(Level.FreeRide);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
