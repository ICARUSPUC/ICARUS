using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownHowlong = 2f; // Tempo que leva para retornar ao normal
    public bool isbullettime = false;
    private float normalFixedDeltaTime;

    private void Awake()
    {
        normalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Método para INICIAR o Bullet Time
    public void BulletTime()
    {
        // 1. Define a escala de tempo e o fixedDeltaTime imediatamente
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = normalFixedDeltaTime * slowdownFactor;

        // 2. Define a flag booleana como TRUE
        isbullettime = true;
    }

    private void Update()
    {
        // 3. Acelera suavemente de volta ao tempo normal (Time.timeScale)
        if (Time.timeScale < 1f)
        {
            Time.timeScale += (1f / slowdownHowlong) * Time.unscaledDeltaTime;
            // Garante que não ultrapasse 1f
            Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownFactor, 1f);
        }

        // 4. Se o tempo estiver quase normal, define a flag como FALSE
        if (Time.timeScale >= 0.99f && isbullettime)
        {
            isbullettime = false;
            // Opcional: Chama a função Reset do FixedDeltaTime para garantir precisão
            Time.fixedDeltaTime = normalFixedDeltaTime;
        }

        // 5. Garante que o fixedDeltaTime acompanhe o timeScale enquanto ele se move
        if (Time.timeScale < 1f)
        {
            Time.fixedDeltaTime = normalFixedDeltaTime * Time.timeScale;
        }
    }
}