using UnityEngine;
using System.Collections;

public class Inimigo : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource Tirosom;
    public AudioSource DeathSound;
    [Header("Tiro e Movimento")]
   [SerializeField] GameObject EnemyShot; //Prefab do tiro do Inimigo
    [SerializeField] GameObject SpawnEnemy; //Spawn do Tiro do Inimigo
    [SerializeField] float ShotFrequency = 10f; // Quão rapido ele atira
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
    [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida máxima
    private float vidaAtual;

    [Header("Particulas")]

    [SerializeField] GameObject Explosao;

    bool movendo = true;
    float InimigoFireTimer = 1;
    public GameManager GameManager; //Fala quem é o GameManager Pra esse Script
    private Vector3 moveEnemy; //Variavel pra mover o inimigo
    private Rigidbody rbEnemy; //Variavel Pro rigidBody do Inimigo

    [SerializeField] private Renderer[] renderers; // arraste aqui os meshes do inimigo
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color[] originalColors;
    private TimeBody timeBody;

    public DialogueSequence dialogo;
    void Awake()
    {

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
        InvokeRepeating("Atirar", InimigoFireTimer, ShotFrequency); //Atira depois de tanto tempo depois repete
        rbEnemy = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;

    }
    void FixedUpdate()
    {

        if (timeBody != null && timeBody.isRewinding)
        {
            RewindSolution();
        }

        if (movendo)
        {
            MovimentacaoInimigo();
            timerMove += Time.deltaTime;

            if (timerMove >= tempoMovimento)
                movendo = false;
        }
    }

    private void Update()
    {


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
        }

    }

    public void OnRewindStart()
    {

        CancelInvoke("Atirar");
    }

    public void OnRewindStop()
    {

        InvokeRepeating("Atirar", InimigoFireTimer, ShotFrequency);
    }


    IEnumerator RewindSolution()
    {
        OnRewindStart();



        yield return new WaitUntil(() => timeBody.isRewinding == false);

        OnRewindStop();

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
            if (EnemyShot != null)
            {
                GameObject tiro = Instantiate(EnemyShot, SpawnEnemy.transform.position, SpawnEnemy.transform.rotation);
                Tirosom.Play();
            }
        }
    }

    void Destruir() //Apaga o inimigo da cena
    {
        Destroy(gameObject);
    }
    public void Morrer() // Desativa e depois de um tempo deleta o inimigo
    {

        Instantiate(Explosao, transform.position, transform.rotation);

        if (dialogo)
        {
            DialogueManager.Instance.StartDialogue(dialogo);
        }
        
      
        GameManager.Mestre.AlterarPontos(50);
        GameManager.Mestre.AlterarChronosPontos(5);
        InimigoSpawnSequence.AddWavePoints();
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke("Destruir", 6f);
    }

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;
        Vector3 Limite = rbEnemy.position + movimento;
        Limite.z = Mathf.Clamp(Limite.z, -13.5f, 6f);
        Limite.x = Mathf.Clamp(Limite.x, -22f, 40f);
        rbEnemy.MovePosition(Limite);
    }
}    


