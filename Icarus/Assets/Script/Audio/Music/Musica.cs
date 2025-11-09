using UnityEngine;
using UnityEngine.Video;

public class MusicaFundo : MonoBehaviour

{
    public VideoPlayer videoPlayer;
    public AudioSource musicaFundo;
    public float segundosAntes = 2f; // segundos antes do vídeo terminar para começar a música

    void Start()
    {
        // dispara a música alguns segundos antes do final do vídeo
        Invoke("TocarMusica", (float)videoPlayer.length - segundosAntes);
    }

    void TocarMusica()
    {
        musicaFundo.Play();
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // mantém o objeto entre cenas
    }
}