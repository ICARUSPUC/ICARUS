using UnityEngine;

public class InimigoMelee : MonoBehaviour
{
    
    [Header("Configuração do inimigo corpo a corpo")]
    [SerializeField] float speed = 5f;           // velocidade de avanço
    [SerializeField] float dano = 1f;  
    [SerializeField] float tempoMorte = 2f;      // tempo até ser destruído após colidir

    [SerializeField] float tempoMovimento = 0f;     // tempo até parar (se quiser limitar)
    [SerializeField] float timerMove = 0f;

    [SerializeField] bool movendo = true;

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida máxima
    private float vidaAtual;

    private Rigidbody rb;
    private bool dash = false;
    private bool atacou = false;

    public GameManager GameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("Dash",2);
         vidaAtual = vidaMax;
    }

    void FixedUpdate()
    {
        if (movendo && !atacou)
        {
            MovimentacaoInimigo();
            timerMove += Time.deltaTime;



            if(dash == true)
            {
                speed = 15;
            }

            // anda sempre pra esquerda (ou em direção ao Player)
           
            if (tempoMovimento > 0 && timerMove >= tempoMovimento)
            {
                movendo = false;
            }
        }
    }

     void MovimentacaoInimigo()
    {
        // movimento constante para a esquerda (ou em direção ao player)
        Vector3 movimento = Vector3.left * speed * Time.deltaTime;
        Vector3 novaPos = rb.position + movimento;

        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
       // novaPos.x = Mathf.Clamp(novaPos.x, -22f, 22f);

        rb.MovePosition(novaPos);
    }

    void Dash()
    {
        dash = true;
    }

   private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        other.GetComponent<Player>().Derrota();
        atacou = true;
        Morrer();
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
        // adiciona pontos ao jogador
        if (GameManager.Mestre != null)
        {
            GameManager.Mestre.AlterarPontos(75); // recompensa diferente do inimigo normal
        }

        CancelInvoke(); // caso tenha algo invocado (segurança)
        gameObject.SetActive(false);
        Invoke("Destruir", tempoMorte);
    }

     public void MorrerFora()
    {
       

        CancelInvoke(); // caso tenha algo invocado (segurança)
        gameObject.SetActive(false);
        Invoke("Destruir", tempoMorte);
    }



    void Destruir()
    {
        Destroy(gameObject);
    }
}

