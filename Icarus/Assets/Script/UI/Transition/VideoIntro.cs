using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class SceneIntroVideo : MonoBehaviour
{
    [Header("Referências")]
    public VideoPlayer videoPlayer;
    public CanvasGroup fadeCanvas;

    [Header("Configuração")]
    public float fadeSpeed = 2f;

    void Start()
    {
        if (fadeCanvas != null)
            fadeCanvas.alpha = 1f; // Começa tudo preto

        StartCoroutine(PlayTransition());
    }

    IEnumerator PlayTransition()
    {
        // Espera 0.1s pra garantir que o load ac
        // Inicia o vídeo
        videoPlayer.Play();

        // Espera 0.2s antes do fade
        yield return new WaitForSeconds(0.2f);

        // Faz o fade-in do vídeo (escurecimento saindo)
        while (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

       
    }
}
