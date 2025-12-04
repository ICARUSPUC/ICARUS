using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    private bool isPaused = false;

    void Start()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (optionsPanel != null)
            optionsPanel.SetActive(true);

        Debug.Log("PAUSE: timeScale=" + Time.timeScale);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        Debug.Log("RESUME: timeScale=" + Time.timeScale);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
