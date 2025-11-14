using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownHowlong = 2f;
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
        // Retorna o tempo ao normal suavemente
        Time.timeScale += (1f / slowdownHowlong) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownFactor, 1f);

        // Sincroniza o fixedDeltaTime com o timeScale atual
        Time.fixedDeltaTime = normalFixedDeltaTime * Time.timeScale;
    }
}
