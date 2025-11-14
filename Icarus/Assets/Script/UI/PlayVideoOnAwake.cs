using UnityEngine;
using UnityEngine.Video;

public class StartVideo : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    void Awake()
    {
        videoPlayer.Play();
    }
}
