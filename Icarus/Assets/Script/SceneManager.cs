using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Fase1");
    }

    private void Update()
    {
        Ganhar();
    }

    void Ganhar() //Ganha se Fizer 1000 pontos
    {
        if (GameManager.Mestre == null) return;
        if (GameManager.Mestre.Pontos >= 1000)
        {
            SceneManager.LoadScene("Victory");
            GameManager.Mestre.Pontos = 0;    
        }

    }
}
