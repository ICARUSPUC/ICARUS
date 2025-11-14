using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneManger : MonoBehaviour
{
    [Header("Fade Config")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeedOut = 1.5f;
    [SerializeField] private float fadeSpeedIn = 1.5f;
    private bool isFading = false;

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.color = Color.black;
            StartCoroutine(FadeIn());
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        isFading = true;

        // FADE OUT (escurecendo)
        for (float i = 0; i <= 1; i += Time.deltaTime * fadeSpeedOut)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }

        // Espera um pouquinho pra suavizar
        yield return new WaitForSeconds(0.2f);

        // Carrega a nova cena (mantendo tudo preto)
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeIn()
    {
        isFading = true;

        // FADE IN (clareando)
        for (float i = 1; i >= 0; i -= Time.deltaTime * fadeSpeedIn)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        isFading = false;
    }

    void Update()
    {
        Ganhar();
    }

    void Ganhar()
    {
        if (GameManager.Mestre == null) return;
        if (GameManager.Mestre.Pontos >= 2000)
        {
            GameManager.Mestre.Pontos = 0;
            LoadScene("Victory");
        }
    }
}

