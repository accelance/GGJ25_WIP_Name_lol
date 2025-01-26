using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject PauseButton;
    public GameObject PausePanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
    }

    public void togglePause() {
        if (gameIsPaused)
        {
            Continue();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        gameIsPaused = true;
        PauseButton.SetActive(false);
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void Continue()
    {
        gameIsPaused = false;
        PauseButton.SetActive(true);
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
