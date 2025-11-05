using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float speedPrincipal = 5f; //Velocidade modo principal
    [SerializeField] float speedRapida = 6f;  //Velocidade modo Rapido
    [SerializeField] Bala[] Tiro = new Bala[1]; //Tiro Instacia
    [SerializeField] GameObject Spawn; //Spawn do tiro
    [SerializeField] float FireRate = 0.1f;
    [SerializeField] float TrocaCD = 1f;
    [SerializeField] float TempoInvencivel = 2f; //Duração da invencibilidade
    [SerializeField] GameObject escudoVisual; // arraste o círculo azul do player aqui no Inspector
    public bool temEscudo = false; // controla se o escudo está ativo
    float anguloRapido = 35f;
    bool direcaoangulo = false;
    public static bool PlayerVivo = true;
    bool direcao = true; //Direcao do modo Rapido
    public bool Modo = true; //Define o modo
    float FireTimer = 0f;
    public float TrocaTimer = 0f;
    public bool invencivel = false; //Define se ele esta invencivel

    // --- Variáveis da Inclinação (Tilt) ---
    public float MaxtiltAngle = 15f; // O ângulo máximo que a nave vai inclinar
    public float tiltspeed = 7f;     // A velocidade que ela inclina/desinclina
                                     

    GameManager GameManager;

    [SerializeField] GameObject shield;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //GameManager.Mestre.Player = this;
    }


    void TrocaCDR()
    {
        TrocaTimer += Time.deltaTime;

    }

    IEnumerator TornarInvencivel()
    {
        invencivel = true;

        yield return new WaitForSeconds(TempoInvencivel);

        invencivel = false;
    }


    public void Derrota()
    {
        if (invencivel) return;

        // Se tiver escudo, quebra ele e cancela o dano
        if (temEscudo)
        {
            QuebrarEscudo();
            return;
        }

        gameObject.SetActive(false);
        PlayerVivo = false;
        Invoke("VaiproMenu", 1f);
        GameManager.Mestre.Pontos = 0;
    }


    void Ganhar() //Ganha se apertar V
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene("Victory");
            GameManager.Mestre.Pontos = 0;
        }
    }


    void VaiproMenu() //Muda a cena pra da de derrota
    {
        SceneManager.LoadScene("Lose");
    }

    void InputTrocadeformas() //define A troca de formas e sua invencibilidade
    {
        TrocaTimer += Time.deltaTime;
        if (TrocaCD >= TrocaTimer)
            return;
        if (Input.GetKeyDown(KeyCode.Space))

        {
            TrocaTimer = 0;
            StartCoroutine(TornarInvencivel());
            Modo = !Modo;
        }
    }


    public void AtivarEscudo()
    {
        temEscudo = true;
        //escudoVisual.SetActive(true);

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

    void Modoprincipal() //Movimento da Nave no modo principal
    {
        float MoveZ = 0f;
        float MoveX = 0f;
        {
            if (Input.GetKey(KeyCode.A)) MoveZ = 1f; ;
            if (Input.GetKey(KeyCode.S)) MoveX = -1f; ; // <-- Input horizontal
            if (Input.GetKey(KeyCode.D)) MoveZ = -1f; ;
            if (Input.GetKey(KeyCode.W)) MoveX = 1f; ;  // <-- Input horizontal

            moveInput = new Vector3(MoveX, 0f, MoveZ);

            Vector3 Limite = (rb.position + moveInput * speedPrincipal * Time.fixedDeltaTime);
            Limite.z = Mathf.Clamp(Limite.z, -13.5f, 6f);
            Limite.x = Mathf.Clamp(Limite.x, -24f, 22f);
            rb.MovePosition(Limite);

            
            ApplyTilt(MoveZ);
        }
    }


    private void ApplyTilt(float horizontalInput)
    {

        float targetTiltX = horizontalInput * MaxtiltAngle;


        float smoothedTiltX = Mathf.LerpAngle(
            transform.eulerAngles.x,   // Rotação X atual
            targetTiltX,               // Rotação X alvo
            tiltspeed * Time.fixedDeltaTime // Velocidade
        );


        transform.rotation = Quaternion.Euler(smoothedTiltX, 0f, 0f);
    }


    void Atirar()
    {
        if (GetComponent<TimeBody>().isrewinding == true)
        {
            return;
        }
        else
        {
            FireTimer += Time.deltaTime;
            if ((Input.GetMouseButton(0) || (Input.GetKey(KeyCode.K))) && FireTimer >= FireRate)

            {
                GameObject novaBala = Instantiate(Tiro[0].gameObject, Spawn.transform.position, Spawn.transform.rotation);

                FireTimer = 0f;
            }
        }
    }


    void ModoRapidoMovimento() //Movimento do Modo Rapido
    {
        // Esta linha força a rotação, o que zera a inclinação (o que é bom para a troca de modo)
        transform.rotation = Quaternion.Euler(0f, anguloRapido, 0f);

        moveInput = direcao ? new Vector3(0, 0, 1f) : new Vector3(0, 0, -1f);
        Vector3 Posicao = (rb.position + moveInput * speedRapida * Time.fixedDeltaTime);
        Posicao.z = Mathf.Clamp(Posicao.z, -13.5f, 6f);
        rb.MovePosition(Posicao);
    }

    void ModoRapidoInput() //Controles do Modo Rapido
    {
        if (direcaoangulo == true)
        {
            anguloRapido = 35f;

        }
        else
        {
            anguloRapido = -35f;
        }
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = Quaternion.Euler(0f, anguloRapido, 0f);
            direcao = !direcao;
            direcaoangulo = !direcaoangulo;
        }

        if (Input.GetMouseButtonDown(1))
        {
            transform.rotation = Quaternion.Euler(0f, anguloRapido, 0f);
            direcao = !direcao;
            direcaoangulo = !direcaoangulo;
        }
    }
}