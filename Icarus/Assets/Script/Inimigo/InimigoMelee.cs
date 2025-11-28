using UnityEngine;
using System.Collections;
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

    [Header("Feedback visual de dano")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    // =========================================================================
    // 💾 Variáveis de Estado (MANTIDO ORIGINAL)
    // =========================================================================

    private Rigidbody rb;
    private bool dash = false;
    private bool atacou = false;
    private Color[] originalColors;

    public GameManager GameManager;

    // =========================================================================
    // 🔄 Métodos Padrão do Unity
    // =========================================================================

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(renderers[i].material);

                if (renderers[i].material.HasProperty("_BaseColor"))
                    originalColors[i] = renderers[i].material.GetColor("_BaseColor");
                else if (renderers[i].material.HasProperty("_Color"))
                    originalColors[i] = renderers[i].material.color;
            }
        }
    }
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
            other.GetComponent<Player>()?.StartCoroutine("Derrota");
            atacou = true;
           
            MorrerFora();
           
        }
    }
    IEnumerator DanoVisual()
    {
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_BaseColor"))
                r.material.SetColor("_BaseColor", damageColor);
            else if (r.material.HasProperty("_Color"))
                r.material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_BaseColor"))
                renderers[i].material.SetColor("_BaseColor", originalColors[i]);
            else if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }
    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());

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
            GameManager.Mestre.AlterarChronosPontos(5);
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