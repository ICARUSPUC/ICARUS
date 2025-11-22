using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f; // O minimo de desaceleracao do tempo
    public float slowdownHowlong = 2f; //Quanto tempo dura a desaceleracao do tempo
    public bool isbullettime = false;

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
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(TempoNaFormaRapida());
        }

     }
        public IEnumerator TempoNaFormaRapida()
        {

        isbullettime = true;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(3.5f);

        while (Time.timeScale < 1f)

        {
            VoltaroTempoaoNormal();
            yield return null;
        }

        isbullettime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

    }


    void VoltaroTempoaoNormal()
    {
        // Retorna o tempo ao normal suavemente
        Time.timeScale += (1f / slowdownHowlong) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownFactor, 1f);

        // Sincroniza o fixedDeltaTime com o timeScale atual
        Time.fixedDeltaTime = normalFixedDeltaTime * Time.timeScale;
    }

}
