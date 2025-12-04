using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("UI Sliders (opcional)")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private const string MASTER_PARAM = "MasterVolume";
    private const string MUSIC_PARAM = "MusicVolume";
    private const string SFX_PARAM = "SFXVolume";

    void Start()
    {
        // MASTER
        if (masterSlider != null)
        {
            float v = PlayerPrefs.GetFloat(MASTER_PARAM, 1f);
            masterSlider.value = v;
            SetMasterVolume(v);
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        // MUSIC
        if (musicSlider != null)
        {
            float v = PlayerPrefs.GetFloat(MUSIC_PARAM, 1f);
            musicSlider.value = v;
            SetMusicVolume(v);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        // SFX
        if (sfxSlider != null)
        {
            float v = PlayerPrefs.GetFloat(SFX_PARAM, 1f);
            sfxSlider.value = v;
            SetSFXVolume(v);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // -------------------------------------------------
    // LOG SCALE - melhor controle perceptivo
    // 20 * log10(x)
    // -------------------------------------------------

    float SliderToDB(float sliderValue)
    {
        if (sliderValue <= 0.0001f)
            return -80f;   // praticamente mudo

        return Mathf.Log10(sliderValue) * 20f;
    }

    // -------------------------------------------------
    // MASTER
    // -------------------------------------------------
    public void SetMasterVolume(float value)
    {
        float dB = SliderToDB(value);
        audioMixer.SetFloat(MASTER_PARAM, dB);
        PlayerPrefs.SetFloat(MASTER_PARAM, value);
    }

    // -------------------------------------------------
    // MUSIC
    // -------------------------------------------------
    public void SetMusicVolume(float value)
    {
        float dB = SliderToDB(value);
        audioMixer.SetFloat(MUSIC_PARAM, dB);
        PlayerPrefs.SetFloat(MUSIC_PARAM, value);
    }

    // -------------------------------------------------
    // SFX
    // -------------------------------------------------
    public void SetSFXVolume(float value)
    {
        float dB = SliderToDB(value);
        audioMixer.SetFloat(SFX_PARAM, dB);
        PlayerPrefs.SetFloat(SFX_PARAM, value);
    }
}