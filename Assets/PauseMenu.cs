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
            gameIsPaused = !gameIsPaused;
            if (gameIsPaused)
            {
                Pause();
            }
            else
            {
                Continue();
            }
        }
    }

    public void togglePause() {
        gameIsPaused = !gameIsPaused;
            if (gameIsPaused)
            {
                Pause();
            }
            else
            {
                Continue();
            }
    }

    public void Pause()
    {
        PauseButton.SetActive(false);
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void Continue()
    {
        PauseButton.SetActive(true);
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
