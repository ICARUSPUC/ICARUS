using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemy : MonoBehaviour
{
    // =========================================================================
    // ⚙️ Variáveis de Configuração (Ajustáveis no Inspector)
    // =========================================================================

    [Header("Configurações da Bala")]
    [SerializeField] float speedEnemy = 10f;
    [SerializeField] float DeathTimeEnemy = 1f; // **Importante:** Este é o tempo inicial.

    // NOVO: Valor fixo para o tempo limite, tornando DeathTimeEnemy um timer
    private const float MAX_LIFETIME = 6f;

    // =========================================================================
    // 🔗 Referências
    // =========================================================================

    // Variável privada para o TimeBody, buscada no Start
    private TimeBody timeBody;

    void Start()
    {
        // 1. Obtém o TimeBody para checagens de Rewinding
        timeBody = GetComponent<TimeBody>();

        if (timeBody == null)
        {
            Debug.LogError("O script BalaEnemy requer um componente TimeBody.");
        }

        // 2. Garante que a bala seja destruída após um tempo máximo
        // Usamos Invoke, pois o método KillBalaEnemy() do original não estava funcionando
        // corretamente como um timer de vida útil no Update.
        Invoke(nameof(DestruirBala), MAX_LIFETIME);
    }

    // =========================================================================
    // 🔄 Update
    // =========================================================================

    void Update()
    {
        // Se a bala estiver rebobinando, o movimento será tratado pelo TimeBody, então saímos.
        if (timeBody != null && timeBody.isRewinding)
        {
            return;
        }

        // Os métodos MoveBalaEnemy e KillBalaEnemy foram integrados aqui para melhor fluxo:
        MoveBalaEnemy();
        // A lógica de KillBalaEnemy original foi substituída pelo Invoke no Start.
    }

    // =========================================================================
    // 💥 Colisão (OnTriggerEnter)
    // =========================================================================

    private void OnTriggerEnter(Collider other)
    {
        // Checagem de Rewind: Se estiver rebobinando, a bala não deve causar dano/destruição.
        if (timeBody != null && timeBody.isRewinding)
        {
            // Tratamento especial para o SpawnPoint ao rebobinar (lógica original)
            if (other.CompareTag("SpawnPoint"))
            {
                Invoke(nameof(DestruirBala), 0.05f);
            }

            return;
        }

        // Lógica de Dano ao Jogador e Destruição
        if (other.CompareTag("Player"))
        {
            // Assumindo que o Player tem um método 'Derrota' que trata escudo/invencibilidade
            other.GetComponent<Player>()?.StartCoroutine("Derrota"); ;
            DestruirBala();
            return; // Destruiu o jogador, saia
        }

    }

    // =========================================================================
    // 🚀 Movimento e Destruição (Métodos Originais)
    // =========================================================================

    void MoveBalaEnemy()
    {
        // Usa Time.deltaTime para movimento baseado em frame rate
        transform.Translate(Vector3.right * speedEnemy * Time.deltaTime, Space.Self);
    }

    // Método original, mas a lógica de timer foi ajustada (ver Start)
    void KillBalaEnemy()
    {
        // REMOVIDO a lógica do timer de vida aqui, que estava incorreta:
        // DeathTimeEnemy += Time.deltaTime; 
        // if (DeathTimeEnemy > 6f) Destroy(gameObject);

        // A vida útil agora é tratada pelo Invoke no Start.
    }

    void DestruirBala()
    {
        Destroy(gameObject);
    }
}