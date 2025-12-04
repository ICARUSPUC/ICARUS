using UnityEngine;
using System.Collections;

public class PowerUpSlowMotion : MonoBehaviour
{
    [SerializeField] float duracao = 5f;          // Duração real do slow motion
    [SerializeField] float fatorLentidao = 0.5f;  // 0.5 = metade da velocidade normal
    private bool efeitoAtivo = false;             // Evita sobreposição do efeito

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null && !efeitoAtivo)
        {
            StartCoroutine(AtivarSlowMotion());
            Destroy(gameObject);
        }
    }

    IEnumerator AtivarSlowMotion()
    {
        efeitoAtivo = true;

        // Reduz a velocidade do tempo
        Time.timeScale = fatorLentidao;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        Debug.Log("Slow Motion ativado");

        // Espera 5 segundos reais, sem ser afetado pelo timeScale
        yield return new WaitForSecondsRealtime(duracao);

        // Restaura o tempo normal
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        efeitoAtivo = false;
        Debug.Log("Slow Motion terminou");
    }
}
