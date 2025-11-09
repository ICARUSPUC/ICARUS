using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer mixer;

    // nomes dos parâmetros expostos no mixer
    const string MUSIC_PARAM = "MusicVolume";
    const string SFX_PARAM = "SFXVolume";

    float musicVolume = 1f;
    float sfxVolume = 1f;

    private void Awake()
    {
        // padrão Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumes();
    }

    void LoadVolumes()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_PARAM, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_PARAM, 1f);

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        mixer.SetFloat(MUSIC_PARAM, Mathf.Log10(musicVolume) * 20f);
        PlayerPrefs.SetFloat(MUSIC_PARAM, musicVolume);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        mixer.SetFloat(SFX_PARAM, Mathf.Log10(sfxVolume) * 20f);
        PlayerPrefs.SetFloat(SFX_PARAM, sfxVolume);
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}
