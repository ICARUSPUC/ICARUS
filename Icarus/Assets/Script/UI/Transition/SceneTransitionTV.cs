using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class SceneTransitionTV : MonoBehaviour
{
    public RawImage videoOverlay;
    public VideoPlayer videoPlayer;
    public AudioSource tvSound;
    public float fadeInSpeed = 1f;
    public float delayBeforeLoad = 0.2f;

    void Start()
    {
        Color c = videoOverlay.color;
        c.a = 0;
        videoOverlay.color = c;
    }

    public void PlayTransitionAndLoad(string sceneName)
    {
        StartCoroutine(PlayAndLoad(sceneName));
    }

    IEnumerator PlayAndLoad(string sceneName)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime * fadeInSpeed)
        {
            Color c = videoOverlay.color;
            c.a = i;
            videoOverlay.color = c;
            yield return null;
        }
        if (tvSound != null) tvSound.Play();

        videoPlayer.Play();
        yield return new WaitForSeconds((float)videoPlayer.length + delayBeforeLoad);

        SceneManager.LoadScene(sceneName);
    }
}
