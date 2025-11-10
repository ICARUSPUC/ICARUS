using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
   
    // Variáveis com valores padrão para ajuste no Inspector
    [SerializeField] float speedPrincipal = 5f;
    [SerializeField] float speedRapida = 6f;
    [SerializeField] Bala[] Tiro = new Bala[1]; // Assumindo que Bala é um MonoBehaviour
    [SerializeField] GameObject Spawn;
    [SerializeField] float FireRate = 0.1f;
    [SerializeField] float TrocaCD = 1f;
    [SerializeField] float TempoInvencivel = 2f;
    [SerializeField] GameObject escudoVisual;
    [SerializeField] float TempoLimiteModoRapido = 3f;


    public bool temEscudo = false;
    float anguloRapido = 35f;
    bool direcaoangulo = false;
    public static bool PlayerVivo = true;
    bool direcao = true;
    public bool Modo = true;
    float FireTimer = 0f;
    public float TrocaTimer = 0f;
    public bool invencivel = false;

    public float MaxtiltAngle = 15f;
    public float tiltspeed = 7f;

    // Referências (preenchidas no Start ou Inspector)
    GameManager GameManager;
    public TimeManager TimeManager;
    private TimeBody timeBody; // Referência ao TimeBody (obtida no Start)

    [SerializeField] GameObject shield;

    private Rigidbody rb;
    private Vector3 moveInput;

    // NOVO: Referência para o componente visual (para piscar)
    private Renderer playerRenderer;

    // NOVO: Tag do Boss para detecção de acerto
    private const string BOSS_TAG = "Boss";

    void Start()
    {

        GameManager.Mestre.Pontos = 0;

        rb = GetComponent<Rigidbody>();
        timeBody = GetComponent<TimeBody>(); // Obtém a referência do TimeBody

        // NOVO: Obtém o renderizador para o efeito visual
        playerRenderer = GetComponent<Renderer>();

        if (timeBody == null)
        {
            Debug.LogError("O script Player requer um componente TimeBody no mesmo objeto.");
        }

        // Inicializa o GameManager se for um Singleton Mestre
        if (GameManager.Mestre != null)
        {
            GameManager = GameManager.Mestre;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        string TagColidida = other.tag;

        // 1. NOVO: Lógica de DANO pelo Boss
        // Verifica o Boss, e se o Player não está invencível.
        if (TagColidida == BOSS_TAG)
        {
            if (!invencivel)
            {
                Derrota();
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
                    // Adicione aqui a Tag do Projétil Inimigo, se houver
                    // case "TiroInimigo":
                    //     Destroy(other.gameObject); // Destroi o tiro inimigo ao invés do Player no modo rápido
                    //     break;
            }
        }
    }


    IEnumerator TornarInvencivel()
    {
        invencivel = true;

        yield return new WaitForSeconds(TempoInvencivel);

        invencivel = false;

        // O PiscarVisual já garante que o renderizador esteja ligado
    }

    // MODIFICADO: Adiciona invencibilidade após quebra do escudo
    public void Derrota()
    {
        if (Modo == false) return;
        if (temEscudo)
        {
            QuebrarEscudo();
            // NOVO: Dá um breve período de invencibilidade após perder o escudo
            StartCoroutine(TornarInvencivel());
            return;
        }

        // Lógica de Morte
        gameObject.SetActive(false);
        PlayerVivo = false;

        // Checagem de segurança para o Singleton
        if (GameManager.Mestre != null)
        {
         ;
        }
        Invoke("VaiproMenu", 1f);
    }

    void Ganhar()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene("Victory");
            if (GameManager.Mestre != null)
            {
               
            }
        }
    }

    void VaiproMenu()
    {
        SceneManager.LoadScene("Lose");
    }

    void InputTrocadeformas()
    {
        TrocaTimer += Time.deltaTime;
        if (TrocaCD >= TrocaTimer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimeManager.BulletTime();
            TrocaTimer = 0;
            StartCoroutine(TornarInvencivel());
            Modo = !Modo;


            if (Modo == false)
            {
                if (timeBody != null)
                {
                    timeBody.SaveCheckpoint();
                    StartCoroutine(ContagemRegressivaTeleporte());
                }
            }
            else
            {

                StopCoroutine("ContagemRegressivaTeleporte");
            }
        }
    }

    // COROUTINE NOVA: Conta o tempo e executa o teleporte de emergência
    IEnumerator ContagemRegressivaTeleporte()
    {
        yield return new WaitForSeconds(TempoLimiteModoRapido);


        if (Modo == false)
        {

            if (timeBody != null)
            {
                timeBody.TeleportToCheckpoint();
                TimeManager.BulletTime();
            }

            Modo = true; // Volta forçadamente para o Modo Principal
            StartCoroutine(TornarInvencivel()); // Dá invencibilidade
        }
    }


    public void AtivarEscudo()
    {
        temEscudo = true;
        shield.SetActive(true);
    }

    public void QuebrarEscudo()
    {
        temEscudo = false;
        shield.SetActive(false);
    }

    void FixedUpdate()
    {
        if (Modo == true)
        {
            Modoprincipal();
            Atirar();
        }
        else ModoRapidoMovimento();
    }

    void Update()
    {
        if (Modo == false)
        {
            ModoRapidoInput();
        }

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

    void Modoprincipal()
    {
        if (timeBody != null && timeBody.isrewinding)
            return;

        float MoveZ = 0f;
        float MoveX = 0f;

        if (Input.GetKey(KeyCode.A)) MoveZ = 1f;
        if (Input.GetKey(KeyCode.S)) MoveX = -1f;
        if (Input.GetKey(KeyCode.D)) MoveZ = -1f;
        if (Input.GetKey(KeyCode.W)) MoveX = 1f;

        moveInput = new Vector3(MoveX, 0f, MoveZ);

        Vector3 Limite = (rb.position + moveInput * speedPrincipal * Time.unscaledDeltaTime);
        Limite.z = Mathf.Clamp(Limite.z, -13.5f, 6f);
        Limite.x = Mathf.Clamp(Limite.x, -24f, 22f);
        rb.MovePosition(Limite);

        ApplyTilt(MoveZ);
    }

    private void ApplyTilt(float horizontalInput)
    {
        float targetTiltX = horizontalInput * MaxtiltAngle;

        float smoothedTiltX = Mathf.LerpAngle(
            transform.eulerAngles.x,
            targetTiltX,
            tiltspeed * Time.fixedDeltaTime
        );

        transform.rotation = Quaternion.Euler(smoothedTiltX, 0f, 0f);
    }

    void Atirar()
    {
        if (timeBody != null && timeBody.isrewinding == true)
        {
            return;
        }
        else
        {
            FireTimer += Time.deltaTime;
            if ((Input.GetMouseButton(0) || (Input.GetKey(KeyCode.K))) && FireTimer >= FireRate)
            {
                // Assumindo que Tiro[0] é a bala a ser instanciada
                if (Tiro.Length > 0 && Tiro[0] != null)
                {
                    Instantiate(Tiro[0].gameObject, Spawn.transform.position, Spawn.transform.rotation);
                    FireTimer = 0f;
                }
            }
        }
    }

    void ModoRapidoMovimento()
    {
        transform.rotation = Quaternion.Euler(0f, anguloRapido, 0f);

        moveInput = direcao ? new Vector3(0.8f, 0, 1f) : new Vector3(0.8f, 0, -1f);
        Vector3 Posicao = (rb.position + moveInput * speedRapida * Time.deltaTime);
        Posicao.z = Mathf.Clamp(Posicao.z, -13.5f, 6f);
        rb.MovePosition(Posicao);
    }

    void ModoRapidoInput()
    {
        if (direcaoangulo == true)
        {
            anguloRapido = 35f;
        }
        else
        {
            anguloRapido = -35f;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            transform.rotation = Quaternion.Euler(0f, anguloRapido, 0f);
            direcao = !direcao;
            direcaoangulo = !direcaoangulo;
        }
    }
}