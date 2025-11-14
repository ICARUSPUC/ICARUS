using UnityEngine;

public class InimigoMelee : MonoBehaviour
{
    // =========================================================================
    // ⚙️ Configuração do Inimigo (MANTIDO ORIGINAL)
    // =========================================================================

    [Header("Configuração do inimigo corpo a corpo")]
    [SerializeField] float speed = 5f;               // velocidade de avanço
    [SerializeField] float dano = 1f;                // Dano (mantido, embora não usado na colisão)
    [SerializeField] float tempoMorte = 2f;          // tempo até ser destruído após colidir

    [SerializeField] float tempoMovimento = 0f;      // tempo até parar (se quiser limitar)
    [SerializeField] float timerMove = 0f;

    [SerializeField] bool movendo = true;

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida máxima
    private float vidaAtual;

    // =========================================================================
    // 💾 Variáveis de Estado (MANTIDO ORIGINAL)
    // =========================================================================

    private Rigidbody rb;
    private bool dash = false;
    private bool atacou = false;

    public GameManager GameManager;

    // =========================================================================
    // 🔄 Métodos Padrão do Unity
    // =========================================================================

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Otimização: Usando nameof()
        Invoke(nameof(Dash), 2);
        vidaAtual = vidaMax;
    }

    void FixedUpdate()
    {
        if (movendo && !atacou)
        {
            MovimentacaoInimigo();

            // CORREÇÃO: Usando Time.fixedDeltaTime para consistência em FixedUpdate
            timerMove += Time.fixedDeltaTime;

            if (dash == true)
            {
                speed = 15;
            }

            if (tempoMovimento > 0 && timerMove >= tempoMovimento)
            {
                movendo = false;
            }
        }
    }

    void Update()
    {
        // Se o inimigo sair muito da tela (por exemplo, para a esquerda ou para baixo), ele morre
        if (transform.position.x < -30f || transform.position.z < -20f || transform.position.z > 10f)
        {
            MorrerFora();
        }
    }

    // =========================================================================
    // ⚔️ Movimento e Dash
    // =========================================================================

    void MovimentacaoInimigo()
    {
        // movimento constante para a esquerda
        // CORREÇÃO: Usando Time.fixedDeltaTime para consistência em FixedUpdate e física
        Vector3 movimento = Vector3.left * speed * Time.fixedDeltaTime;
        Vector3 novaPos = rb.position + movimento;

        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        // O limite de X foi deixado comentado, como no original
        // novaPos.x = Mathf.Clamp(novaPos.x, -22f, 22f); 

        rb.MovePosition(novaPos);
    }

    void Dash()
    {
        dash = true;
    }

    // =========================================================================
    // 💥 Dano, Colisão e Morte
    // =========================================================================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Otimização: Usando null-conditional operator (?. ) para segurança
            other.GetComponent<Player>()?.Derrota();
            atacou = true;
            MorrerFora();
        }
    }

    public void LevarDano(float dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    public void Morrer()
    {
        // Adiciona pontos ao jogador
        if (GameManager.Mestre != null)
        {
            GameManager.Mestre.AlterarPontos(75); // recompensa diferente do inimigo normal
        }

        // Otimização: Cancelando o Dash específico e outros Invokes
        CancelInvoke(nameof(Dash));
        CancelInvoke();

        gameObject.SetActive(false);
        // Otimização: Usando nameof()
        Invoke(nameof(Destruir), tempoMorte);
    }

    public void MorrerFora()
    {
        // Otimização: Cancelando o Dash específico e outros Invokes
        CancelInvoke(nameof(Dash));
        CancelInvoke();

        gameObject.SetActive(false);
        // Otimização: Usando nameof()
        Invoke(nameof(Destruir), tempoMorte);
    }

    void Destruir()
    {
        Destroy(gameObject);
    }
}