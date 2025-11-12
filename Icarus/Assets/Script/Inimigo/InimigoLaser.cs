using UnityEngine;
using System.Collections; // Adicionado para suportar Coroutines

public class InimigoLaser : MonoBehaviour
{
    // =========================================================================
    // ⚙️ Configuração (MANTIDO TUDO ORIGINAL)
    // =========================================================================
    [Header("Configuração do inimigo de laser")]
    [SerializeField] GameObject laserPrefab;     // Prefab do laser
    [SerializeField] Transform spawnLaser;       // Ponto de spawn do laser
    [SerializeField] float intervaloTiro = 5f;  // Tempo entre disparos
    [SerializeField] float duracaoLaser = 2f;    // Quanto tempo o laser fica ativo
    [SerializeField] float tempoMorte = 2f;
    [SerializeField] float speed = 2f;

    [SerializeField] float tempoMovimento = 3f; // Tempo até ele parar 
    [SerializeField] float timerMove = 0f;
    [SerializeField] bool movendo = true;

    [Header("Status")]
    [SerializeField] private float vidaMax = 6f; // Vida máxima
    private float vidaAtual;

    // =========================================================================
    // 💾 Variáveis de Estado (MANTIDO TUDO ORIGINAL)
    // =========================================================================
    private GameObject laserAtual;
    private bool atirando = false;
    private bool parado = false;
    private Rigidbody rb;
    public GameManager GameManager;

    // =========================================================================
    // 🔄 Métodos Padrão do Unity
    // =========================================================================

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Otimização: Usando nameof()
        InvokeRepeating(nameof(AtirarLaser), 2f, intervaloTiro);
        vidaAtual = vidaMax;
    }

    void FixedUpdate()
    {
        if (atirando)
        {
            movendo = false;
        }

        if (movendo)
        {
            MovimentacaoInimigo();
            // CORREÇÃO: Usando Time.fixedDeltaTime para consistência em FixedUpdate
            timerMove += Time.fixedDeltaTime;

            if (timerMove >= tempoMovimento)
            {
                movendo = false;
            }
        }
    }

    // =========================================================================
    // ⚔️ Movimento e Ataque
    // =========================================================================

    void AtirarLaser()
    {
        if (atirando) return;
        // Otimização: Usando o namespace completo para evitar confusão se houver outra classe IEnumerator
        StartCoroutine(LaserRoutine());
    }

    System.Collections.IEnumerator LaserRoutine()
    {
        atirando = true;

        // Cria o laser
        GameObject laser = Instantiate(laserPrefab, spawnLaser.position, spawnLaser.rotation);

        // Destroi o laser depois de um tempo
        yield return new WaitForSeconds(duracaoLaser);

        // Otimização: Checagem de segurança (null-check)
        if (laser != null)
        {
            Destroy(laser);
        }

        atirando = false;
    }

    void MovimentacaoInimigo()
    {
        // movimento constante para a frente
        // CORREÇÃO: Usando Time.fixedDeltaTime para consistência em FixedUpdate
        Vector3 movimento = Vector3.left * speed * Time.fixedDeltaTime;
        Vector3 novaPos = rb.position + movimento;

        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        novaPos.x = Mathf.Clamp(novaPos.x, -22f, 0f);

        rb.MovePosition(novaPos);
    }

    // =========================================================================
    // 💥 Dano e Morte
    // =========================================================================

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
        if (GameManager != null && GameManager.Mestre != null)
        {
            GameManager.Mestre.AlterarPontos(100); // recompensa maior
        }

        // Otimização: Usando nameof()
        CancelInvoke(nameof(AtirarLaser));

        // Otimização: Adicionado StopAllCoroutines() para parar o ataque imediatamente
        StopAllCoroutines();

        gameObject.SetActive(false);
        // Otimização: Usando nameof()
        Invoke(nameof(Destruir), tempoMorte);
    }

    void Destruir()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Otimização: Usando null-conditional operator (?. ) para segurança
            other.GetComponent<Player>()?.Derrota();
        }
    }
}