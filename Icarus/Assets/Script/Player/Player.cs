using System.Collections;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // =========================================================================
    // ⚙️ Variáveis de Configuração (Ajustáveis no Inspector)
    // =========================================================================
    [Header("Audio")]

    [SerializeField] AudioSource SomTiro;
    [SerializeField] AudioSource DilatacaoTemporal;


    [Header("Movimento Principal")]
    [SerializeField] float speedPrincipal = 5f;
    [SerializeField] float MaxtiltAngle = 15f;
    [SerializeField] float tiltspeed = 7f;
    public static float CurrentHorizontalInput;

    [Header("Movimento Rápido")]
    [SerializeField] float pontosPraFormarapida = 25f;
    [SerializeField] float speedRapida = 6f;
    [SerializeField] float TempoLimiteModoRapido = 3f; // Tempo máximo no Modo Rápido
    [SerializeField] float transicaoDeltaTime = 0.5f;

    [Header("Tiro")]
    [SerializeField] Bala[] Tiro = new Bala[1]; // Assumindo que Bala é um MonoBehaviour
    [SerializeField] GameObject Spawn; // Ponto de spawn do tiro
    [SerializeField] float FireRate = 0.1f;

    [Header("Defesa e Invencibilidade")]
    [SerializeField] float TrocaCD = 1f; // Cooldown para trocar de modo
    [SerializeField] float TempoInvencivel = 2f; // Duração da invencibilidade
    [SerializeField] GameObject shield; // Referência direta ao objeto do escudo
    [SerializeField] GameObject escudoVisual; // Mantido o nome original

    [Header("Particulas")]
    [SerializeField] GameObject explosao;
    [SerializeField] GameObject destrocos;
    [SerializeField] GameObject Explosaotemporal;
    [SerializeField] GameObject ExplosaoSecondForm;
    [SerializeField] GameObject SombraPlayer;
    [SerializeField] GameObject TrailNormal;
    [SerializeField] GameObject TrailTempo;

    [Header("Chronos")]

    public bool Chronos = false;
    public float GlobalRewindDuration = 4f;
    public float chronospontos = 0f;
    float tempoNoModoRapido = 0f;
    public bool isRewinding;

    //GUGU TEMPORÁRIO




    // =========================================================================
    // 🔑 Variáveis de Estado Públicas (NOMES ORIGINAIS RESTAURADOS)
    // =========================================================================

    public bool temEscudo = false;
    public static bool PlayerVivo = true;
    public bool Modo = true; // true = Modo Principal, false = Modo Rápido
    public float TrocaTimer = 0f;
    public bool invencivel = false;
    
    // =========================================================================
    // ♻️ Variáveis de Estado Privadas
    // =========================================================================

    float anguloRapido = 35f;
    bool direcaoangulo = false;
    bool direcao = true;
    float FireTimer = 0f;

    // Variáveis de referência de componentes
    private Rigidbody rb;
    private Vector3 moveInput;
    private Renderer playerRenderer; // Mantido o nome original

    // =========================================================================
    // 🔗 Referências de Componentes/Scripts
    // =========================================================================

    GameManager GameManager; // Mantido o nome original
    public TimeManager TimeManager; // Mantido o nome original
    private TimeBody timeBody; // Referência ao TimeBody

    // =========================================================================
    // 🏷️ Tags
    // =========================================================================

    private const string BOSS_TAG = "Boss";

    // =========================================================================
    // 🔄 Métodos Padrão do Unity
    // =========================================================================

    void Start()
    {
        PlayerVivo = true;

        rb = GetComponent<Rigidbody>();
        timeBody = GetComponent<TimeBody>();
        playerRenderer = GetComponent<Renderer>();

        if (timeBody == null)
        {
            Debug.LogError("O script Player requer um componente TimeBody.");
        }

        // Inicializa o GameManager se for um Singleton Mestre
        if (GameManager.Mestre != null)
        {
            GameManager = GameManager.Mestre;
            GameManager.Mestre.Pontos = 0; // Usando o Mestre para garantir o acesso
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string TagColidida = other.tag;

        // 1. Lógica de DANO pelo Boss
        if (TagColidida == BOSS_TAG)
        {
            if (!invencivel)
            {
                StartCoroutine("Derrota");
            }
            return;
        }

        // 2. Lógica de COLISÃO com inimigos no Modo Rápido (Se Modo == false)
        if (Modo == false)
        {
            switch (TagColidida)
            {
                case "Inimigo":
                    other.GetComponent<Inimigo>()?.Morrer();
                    break;
                case "InimigoMelee":
                    other.GetComponent<InimigoMelee>()?.Morrer();
                    break;
                case "InimigoLaser":
                    other.GetComponent<InimigoLaser>()?.Morrer();
                    break;
                case "TiroInimigo": // Adicionado o tratamento para projéteis inimigos
                    Destroy(other.gameObject);
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (!PlayerVivo) return;

        if (Modo == true)
        {
            Modoprincipal();
        }
        else
        {

            ModoRapidoMovimento();
        }
    }

    void Update()
    {
        if (!PlayerVivo) return;

        if (Modo == true)
        {
            Atirar(); // O tiro é baseado em Input/Timer, então fica no Update
        }
        else
        {
            ModoRapidoInput();
        }

        // Inputs de Escudo (Originalmente de teste, mantidos para não alterar o comportamento)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            AtivarEscudo();
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            QuebrarEscudo();
        }

        InputTrocadeformas();
        Ganhar();
    }

    // =========================================================================
    // 🏃 Movimento e Input
    // =========================================================================

    void Modoprincipal()
    {
        if (timeBody != null && timeBody.isRewinding)
            return;

        float MoveZ = 0f;
        float MoveY = 0f;

        if (Input.GetKey(KeyCode.A)) MoveZ = 1f;
        if (Input.GetKey(KeyCode.S)) MoveY = -1f;
        if (Input.GetKey(KeyCode.D)) MoveZ = -1f;
        if (Input.GetKey(KeyCode.W)) MoveY = 1f;

        moveInput = new Vector3(0f, MoveY, MoveZ);
        CurrentHorizontalInput = MoveZ;

        // Movimento
        Vector3 Limite = (rb.position + moveInput * speedPrincipal * Time.fixedDeltaTime); // Usando FixedDeltaTime para movimento
        Limite.z = Mathf.Clamp(Limite.z, -11f, 4f);
        Limite.y = Mathf.Clamp(Limite.y, 0.9f, 9f);
        rb.MovePosition(Limite);

        ApplyTilt(MoveZ);
    }

    public float ApplyTilt(float horizontalInput) // da uma tombadinha quando movimenta o player
    {
        float targetTiltX = horizontalInput * MaxtiltAngle;

        float smoothedTiltX = Mathf.LerpAngle(
            transform.eulerAngles.x,
            targetTiltX,
            tiltspeed * Time.fixedDeltaTime
        );

        return smoothedTiltX;
    }

    void ModoRapidoMovimento()
    {
       
        tempoNoModoRapido += Time.unscaledDeltaTime;

        float movimentoDeltaTime;

     
        if (tempoNoModoRapido < transicaoDeltaTime)
        {
            movimentoDeltaTime = Time.deltaTime;
        }
       
        else
        {
            movimentoDeltaTime = Time.unscaledDeltaTime;
        }


        transform.rotation = Quaternion.Euler(0f, anguloRapido, 0f);

        moveInput = direcao ? new Vector3(0.8f, 0, 1f) : new Vector3(0.8f, 0, -1f);

        Vector3 Posicao = (rb.position + moveInput * speedRapida * movimentoDeltaTime);

        Posicao.z = Mathf.Clamp(Posicao.z, -13.5f, 6f);

        rb.MovePosition(Posicao);
    }




    void ModoRapidoInput()
    {
        // Lógica para determinar o ângulo
        anguloRapido = direcaoangulo ? 35f : -35f;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            transform.rotation = Quaternion.Euler(0f, anguloRapido, 0f);
            direcao = !direcao;
            direcaoangulo = !direcaoangulo;
        }
    }

    // =========================================================================
    // 🔫 Tiro
    // =========================================================================

    void Atirar()
    {
        if (timeBody != null && timeBody.isRewinding == true)
        {
            return;
        }

        FireTimer += Time.deltaTime;
        if ((Input.GetMouseButton(0) || (Input.GetKey(KeyCode.K))) && FireTimer >= FireRate)
        {
            // Assumindo que Tiro[0] é a bala a ser instanciada
            if (Tiro.Length > 0 && Tiro[0] != null)
            {
                SomTiro.Play();
                Instantiate(Tiro[0].gameObject, Spawn.transform.position, Spawn.transform.rotation);
                FireTimer = 0f;
            }
        }
    }

    // =========================================================================
    // 🛡️ Escudo e Dano
    // =========================================================================

    IEnumerator TornarInvencivel()
    {
        invencivel = true;
        // Lógica visual de piscar, se necessário, pode ser adicionada aqui
        yield return new WaitForSeconds(TempoInvencivel);

        invencivel = false;
    }

    
    public IEnumerator Derrota()
    {
        if (Modo == false) yield break;
        if (invencivel) yield break;

        if (temEscudo)
        {
            QuebrarEscudo();
            // Dá um breve período de invencibilidade após perder o escudo
            StartCoroutine(TornarInvencivel());
            yield break;
        }

        if (Chronos)
        {
            invencivel = true;
            Instantiate(Explosaotemporal, transform.position, transform.rotation);
            Instantiate(SombraPlayer, transform.position, transform.rotation);
            TrailNormal.SetActive(false);
            TrailTempo.SetActive(true);
            DilatacaoTemporal.Play();
            TimeManager.StartCoroutine("Chronos");

            yield return new WaitForSecondsRealtime(0.3f);
            // 1. encontra todos os componentes TimeBody na cena
            TimeBody[] allTimeBodies = FindObjectsOfType<TimeBody>();

            foreach (TimeBody tb in allTimeBodies)
            {
                tb.StartRewind();
            }
            isRewinding = true;

            Invoke("StopGlobalRewind", GlobalRewindDuration);
            yield return new WaitForSecondsRealtime(GlobalRewindDuration);
            TrailNormal.SetActive(true);
            TrailTempo.SetActive(false);
            Destroy(SombraPlayer);
            Chronos = false;
            invencivel = false;
            isRewinding = false;
            yield break;
        }


        // Lógica de Morte
        gameObject.SetActive(false);
        Instantiate (explosao, transform.position, transform.rotation);
        Instantiate(destrocos, transform.position, transform.rotation);
        PlayerVivo = false;

        // Checagem de segurança para o Singleton
        if (GameManager.Mestre != null)
        {
            // Lógica de pontuação ou estado do GameManager
        }
        Invoke(nameof(VaiproMenu), 3f); // Usando nameof() para segurança
    }
    public void StopGlobalRewind()
    {

        TimeBody[] allTimeBodies = FindObjectsOfType<TimeBody>();

        
        foreach (TimeBody tb in allTimeBodies)
        {
            tb.StopRewind();
        }
    }
    public void AtivarEscudo()
    {
        temEscudo = true;
        shield?.SetActive(true); // Usando ? para checagem de null
    }

    public void QuebrarEscudo()
    {
        temEscudo = false;
        shield?.SetActive(false); // Usando ? para checagem de null
    }

    // =========================================================================
    // ⚔️ Troca de Modos e Fim de Jogo
    // =========================================================================

    void InputTrocadeformas()
    {
        TrocaTimer += Time.deltaTime;
        if (GameManager.chronospontos <= pontosPraFormarapida)
            return;

        if (TrocaTimer  <  TrocaCD)
            return;

        if (timeBody != null && timeBody.isRewinding == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrocaTimer = 0f;
            StartCoroutine(TransitarParaModoRapido());
        }
    }

    IEnumerator TransitarParaModoRapido()
    {
     
        Instantiate(ExplosaoSecondForm, transform.position, transform.rotation);
        TrocaTimer = 0;
        StartCoroutine(TornarInvencivel());
        tempoNoModoRapido = 0f;

        if (TimeManager != null)
        {
           
            TimeManager.StartCoroutine("TempoNaFormaRapida");
        }


        yield return new WaitForSecondsRealtime(0.01f); // Espera 1 frame IMPORTANTISSIMO, É OQUE RESOLVE O PROBLEMA DO TELEPORTE NÃO MEXA NESSA MERDA!!!!!!!!!!!

        Instantiate(SombraPlayer, transform.position, transform.rotation);
        Modo = false; 


        if (timeBody != null)
        {
            timeBody.SaveCheckpoint();
            
            StartCoroutine(nameof(ContagemRegressivaTeleporte));
        }

    }
    // COROUTINE: Conta o tempo e executa o teleporte de emergência
    IEnumerator ContagemRegressivaTeleporte()
    {
        yield return new WaitForSecondsRealtime(TempoLimiteModoRapido);

        if (Modo == false)
        {
            if (timeBody != null)
            {
                timeBody.TeleportToCheckpoint();
                TimeManager?.BulletTime();
            }

            Modo = true; // Volta forçadamente para o Modo Principal
            StartCoroutine(TornarInvencivel()); // Dá invencibilidade

            while (Time.timeScale < 1f)

            {
                TimeManager?.VoltaroTempoaoNormal();
                yield return null;
            }
            GameManager.chronospontos = (GameManager.chronospontos - pontosPraFormarapida);
        }
    }

    void VaiproMenu()
    {
        SceneManager.LoadScene("Lose");
    }

    void Ganhar()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene("Victory");
            // Lógica do GameManager foi removida para simplificar
        }
    }
}