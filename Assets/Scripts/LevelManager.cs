using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject bunkersHolder;
    public TMP_Text objectiveText;
    public static int bunkersCount;

    public GameObject EndGameCanvas;
    public TMP_Text completionText;
    private static float completionTime;
    private float timeToLoadMenu = 5f;

    void Awake()
    {
        bunkersCount = bunkersHolder.transform.childCount;
        objectiveText.text = "Buncers left to destroy: " + bunkersCount;
        EndGameCanvas.SetActive(false);
        completionTime = 0f;
    }

    private void Update() {
        objectiveText.text = "Buncers left to destroy: " + bunkersCount;

        if (bunkersCount > 0) {
            completionTime += Time.deltaTime;
        } else {
            EndGameCanvas.SetActive(true);
            completionText.text = "It took you " + Mathf.Round(completionTime) + " seconds!";

            if (timeToLoadMenu >= 0) {
                timeToLoadMenu -= Time.deltaTime;
            } else {
                SceneManager.LoadScene(Level.MainMenu);
            }
        }

    }


}
