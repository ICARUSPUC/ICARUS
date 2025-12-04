using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEndLoad : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string proximaCena = "Menu"; //Muda a cena quando o video acaba

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(proximaCena);
    }
}