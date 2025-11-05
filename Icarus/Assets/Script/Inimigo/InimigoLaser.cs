using UnityEngine;

public class InimigoLaser : MonoBehaviour
{
    [Header("Configuração do inimigo de laser")]
    [SerializeField] GameObject laserPrefab;    // Prefab do laser
    [SerializeField] Transform spawnLaser;      // Ponto de spawn do laser
    [SerializeField] float intervaloTiro = 5f;  // Tempo entre disparos
    [SerializeField] float duracaoLaser = 2f;   // Quanto tempo o laser fica ativo
    [SerializeField] float tempoMorte = 2f;
    [SerializeField] float speed = 2f;

    [SerializeField] float tempoMovimento = 3f; // Tempo até ele parar 
    [SerializeField] float timerMove = 0f;
    [SerializeField] bool movendo = true;

    [Header("Status")]
    [SerializeField] private float vidaMax = 6f; // Vida máxima
    private float vidaAtual;

    
     private GameObject laserAtual;

    private bool atirando = false;
     private bool parado = false;
    private Rigidbody rb;
    public GameManager GameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            timerMove += Time.deltaTime;

            if (timerMove >= tempoMovimento)
            {
                movendo = false;
            
            }
        }

       
    }
    

    void AtirarLaser()
    {
        if (atirando) return; // impede atirar de novo enquanto o laser atual existe
        StartCoroutine(LaserRoutine());
    }

    System.Collections.IEnumerator LaserRoutine()
    {
        atirando = true;


        // Cria o laser
        GameObject laser = Instantiate(laserPrefab, spawnLaser.position, spawnLaser.rotation);

        // Destroi o laser depois de um tempo
        yield return new WaitForSeconds(duracaoLaser);
        Destroy(laser);

        atirando = false;
    }

      void MovimentacaoInimigo()
    {
        // movimento constante para a frente
        Vector3 movimento = Vector3.left * speed * Time.deltaTime;
        Vector3 novaPos = rb.position + movimento;

        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        novaPos.x = Mathf.Clamp(novaPos.x, -22f, 0f);

        rb.MovePosition(novaPos);
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
        if (GameManager != null && GameManager.Mestre != null)
        {
            GameManager.Mestre.AlterarPontos(100); // recompensa maior
        }

        

        CancelInvoke();
        gameObject.SetActive(false);
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
            other.GetComponent<Player>().Derrota();
        }

       
    }
}
   