using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuPanel;
    private Camera cam;
    AudioSource[] audioSources;

    private void Awake() {

        pauseMenuPanel.SetActive(false);

        cam = Camera.main;

        ActivateCursor(false);

        audioSources = FindObjectsOfType<AudioSource>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void GoToMenu() {
        SceneManager.LoadScene(Level.MainMenu);
        Time.timeScale = 1f;
    }

    public void Resume() {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        cam.GetComponent<CameraMovement>().enabled = true;
        ActivateCursor(false);

        foreach (AudioSource audio in audioSources) {
            audio.Play();
        }
    }

    private void Pause() {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        cam.GetComponent<CameraMovement>().enabled = false;
        ActivateCursor(true);

        foreach (AudioSource audio in audioSources) {
            audio.Pause();
        }
    }

    private void ActivateCursor(bool toActivate) {
        if (toActivate) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
