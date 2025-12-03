using UnityEngine;

public class MusicaFundo : MonoBehaviour
{
    public AudioSource musicaFundo;
    public TimeManager TimeManager;
    public Player player;
    public Boss boss;
    public float pitchChangeSpeed = 3f;


    private float targetPitch;

    void Update()
    { 
        if (boss.BossinScene == true)
        {
            Destroy(this.gameObject);
        }
        if (TimeManager = null)
        {
            return;
        }

        if (TimeManager != null && TimeManager.isbullettime || player.Modo == false)
        {

            targetPitch = 0.7f;
        }
        else
        {

            targetPitch = 1f;
        }


        musicaFundo.pitch = Mathf.Lerp(
            musicaFundo.pitch,
            targetPitch,
            Time.unscaledDeltaTime * pitchChangeSpeed
        );


        if (Mathf.Abs(musicaFundo.pitch - targetPitch) < 0.01f)
        {
            musicaFundo.pitch = targetPitch;
        }

       
    }

}
   