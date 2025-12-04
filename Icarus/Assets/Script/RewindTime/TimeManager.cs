using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeBody ZaWarudo;
    [Header("FormaRapida Tempo")]
    public float slowdownFactor = 0.05f; // O minimo de desaceleracao do tempo
    public float slowdownHowlong = 2f; //Quanto tempo dura a desaceleracao do tempo
    public float slowdownHoldDuration = 0.5f;
    public bool isbullettime = false;

    [Header("Chronos Tempo")]
    public float ChronoslowdownFactor = 0.05f; // O minimo de desaceleracao do tempo
    public float ChronoslowdownHowlong = 2f; //Quanto tempo dura a desaceleracao do tempo
    public float ChronoslowdownHoldDuration = 0.5f;

    private float slowdownStartTime;
    private float normalFixedDeltaTime;

    private void Awake()
    {
        normalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void BulletTime()
    {
        isbullettime = true;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = normalFixedDeltaTime * slowdownFactor;


        Time.timeScale += (1f / slowdownHowlong) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownFactor, 1f);

        // Sincroniza o fixedDeltaTime com o timeScale atual
        Time.fixedDeltaTime = normalFixedDeltaTime * Time.timeScale;
    }

    private void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine("TempoNaFormaRapida");
        }

     }
        public IEnumerator TempoNaFormaRapida()
        {

        isbullettime = true;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowdownHoldDuration);

        while (Time.timeScale < 1f)

        {
            VoltaroTempoaoNormal();
            yield return null;
        }

        isbullettime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

    }


    public IEnumerator Chronos()
    {

        isbullettime = true;
        Time.timeScale = ChronoslowdownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(ChronoslowdownHoldDuration);

        while (Time.timeScale < 1f)

        {
            ChronosVoltaroTempoaoNormal();
            yield return null;
        }

        isbullettime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

    }
    public void ChronosVoltaroTempoaoNormal()
    {
        // Retorna o tempo ao normal suavemente
        Time.timeScale += (1f / ChronoslowdownHowlong) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownFactor, 1f);

        // Sincroniza o fixedDeltaTime com o timeScale atual
        Time.fixedDeltaTime = normalFixedDeltaTime * Time.timeScale;
    }

    public void VoltaroTempoaoNormal()
    {
        // Retorna o tempo ao normal suavemente
        Time.timeScale += (1f / slowdownHowlong) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownFactor, 1f);

        // Sincroniza o fixedDeltaTime com o timeScale atual
        Time.fixedDeltaTime = normalFixedDeltaTime * Time.timeScale;
    }

}
