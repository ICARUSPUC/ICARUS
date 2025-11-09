using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour
{
    [Header("Panels (Main always ON)")]
    public GameObject mainMenuPanel;   // nunca será desativado
    public GameObject optionsPanel;
    public GameObject volumePanel;
    // public GameObject gameModePanel;  // futuro

    private Stack<GameObject> history = new Stack<GameObject>();
    private GameObject currentPanel = null;

    void Start()
    {
        // garante que só o main está ativo
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        volumePanel.SetActive(false);
        // if (gameModePanel) gameModePanel.SetActive(false);

        currentPanel = mainMenuPanel;
    }

    // abre o panel desejado
    void OpenPanel(GameObject panel)
    {
        if (currentPanel != null && currentPanel != mainMenuPanel)
            currentPanel.SetActive(false);

        panel.SetActive(true);

        if (currentPanel != mainMenuPanel)
            history.Push(currentPanel);

        currentPanel = panel;
    }

    public void OpenOptions()
    {
        OpenPanel(optionsPanel);
    }

    public void OpenVolume()
    {
        OpenPanel(volumePanel);
    }

    /*
    public void OpenGameMode()
    {
        OpenPanel(gameModePanel);
    }
    */

    // volta ao último panel
    public void Back()
    {
        // se não há histórico, volta ao Main
        if (history.Count == 0)
        {
            if (currentPanel != mainMenuPanel)
                currentPanel.SetActive(false);

            currentPanel = mainMenuPanel;
            return;
        }

        GameObject last = history.Pop();

        if (currentPanel != mainMenuPanel)
            currentPanel.SetActive(false);

        last.SetActive(true);
        currentPanel = last;
    }

    // botão Play
    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // botão Quit
    public void Quit()
    {
        Application.Quit();
    }
}
