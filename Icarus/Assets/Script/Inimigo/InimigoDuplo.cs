using UnityEngine;
using System.Collections;

public class InimigoDuplo : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource Tirosom;
    public AudioSource DeathSound;

    [Header("Tiro e Movimento")]
    [SerializeField] GameObject EnemyShot; //Prefab do tiro do Inimigo
    [SerializeField] GameObject SpawnEnemyR;//Spawn do Tiro do Inimigo Direito
    [SerializeField] GameObject SpawnEnemyL;//Spawn do Tiro do Inimigo Esquerdo
    [SerializeField] float ShotFrequency = 10f; // Quão rapido ele atira
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
    [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida máxima
    private float vidaAtual;
    [SerializeField] GameObject Explosao;

    bool movendo = true;
    float InimigoFireTimer = 1;
    float InimigoFireTimer2 = 1;
    public GameManager GameManager; //Fala quem é o GameManager Pra esse Script
    private Vector3 moveEnemy; //Variavel pra mover o inimigo
    private Rigidbody rbEnemy; //Variavel Pro rigidBody do Inimigo

    [SerializeField] private Renderer[] renderers; // arraste aqui os meshes do inimigo
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color[] originalColors;
    private Transform player; // Posicão do Player
    private TimeBody timeBody;


    void Awake()
    {
        // Guarda as cores originais dos materiais

        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(renderers[i].material);

                if (renderers[i].material.HasProperty("_TintColor"))
                    originalColors[i] = renderers[i].material.GetColor("_TintColor");
                else if (renderers[i].material.HasProperty("_Color"))
                    originalColors[i] = renderers[i].material.color;
            }
        }
    }



    void Start()
    {
        float ShotFrequency2 = ShotFrequency+0.4f;
        InvokeRepeating("Atirar", InimigoFireTimer, ShotFrequency); //Atira depois de tanto tempo depois repete
        InvokeRepeating("Atirar2",InimigoFireTimer2, ShotFrequency2);
        rbEnemy = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // Descobre quem é o player

    }

    private void Update()
    { 
            if (movendo)
            {
                MovimentacaoInimigo();
                timerMove += Time.fixedDeltaTime;

                if (timerMove >= tempoMovimento)
                {
                    movendo = false; // para o inimigo
                }

            }

        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; 

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }

    }


    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());
        Debug.Log($"{name} levou {dano} de dano. Vida atual: {vidaAtual}/{vidaMax}");
        if (vidaAtual <= 0f) Morrer();
    }

    IEnumerator DanoVisual()
    {
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_TintColor"))
                r.material.SetColor("_TintColor", damageColor);
            else if (r.material.HasProperty("_Color"))
                r.material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_TintColor"))
                renderers[i].material.SetColor("_TintColor", originalColors[i]);
            else if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }

    void Atirar()
    {
        if (GetComponent<TimeBody>().isRewinding == true)
        {
            return;
        }
        else
        {
            
            GameObject tiroR = Instantiate(EnemyShot, SpawnEnemyR.transform.position, SpawnEnemyR.transform.rotation);
            Tirosom.Play();
        }
    }

    void Atirar2()
    {
        if (GetComponent<TimeBody>().isRewinding == true)
        {
            return;
        }
        else
        {

            GameObject tiroL = Instantiate(EnemyShot, SpawnEnemyL.transform.position, SpawnEnemyL.transform.rotation);
            Tirosom.Play();
        }
    }


    public void OnRewindStart()
    {

        CancelInvoke("Atirar");
    }

    public void OnRewindStop()
    {

        InvokeRepeating("Atirar", InimigoFireTimer, ShotFrequency);


        timerMove = 0f;
        movendo = true;
    }


    IEnumerator RewindSolution()
    {
        OnRewindStart();



        yield return new WaitUntil(() => timeBody.isRewinding == false);

        OnRewindStop();

    }
    void Destruir() //Apaga o inimigo da cena
    {
        Destroy(gameObject);
    }
    public void Morrer() // Desativa e depois de um tempo deleta o inimigo
    {

        GameManager.Mestre.AlterarPontos(100);
        CancelInvoke();
        InimigoSpawnSequence.AddWavePoints();
        Instantiate(Explosao, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Invoke("Destruir", 6f);
    }

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;
        Vector3 Limite = rbEnemy.position + movimento;
        Limite.z = Mathf.Clamp(Limite.z, -13.5f, 6f);
        Limite.x = Mathf.Clamp(Limite.x, -22f, 22f);
        rbEnemy.MovePosition(Limite);
    }
}


