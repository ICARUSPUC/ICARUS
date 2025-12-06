using UnityEngine;
using System.Collections;

public class SimpleEnemy : MonoBehaviour
{
   //Mexer no Atirar

   
    [Header("Tiro e Movimento")]
   [SerializeField] GameObject EnemyShot; //Prefab do tiro do Inimigo
    [SerializeField] GameObject SpawnEnemy; //Spawn do Tiro do Inimigo
    [SerializeField] float ShotFrequency = 0.7f; // Qu�o rapido ele atira
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
     [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;
 

       [Header("Limites de Movimento (Eixos Z)")]
    public float limiteEsquerda = -20f;
    public float limiteDireita = 20f;

     private int direcao = -1; // come�a indo para a esquerda
    private Rigidbody rbEnemy;
    

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida m�xima
    private float vidaAtual;


    [Header("Particulas")]

    [SerializeField] GameObject Explosao;

    bool movendo = true;
    float InimigoFireTimer = 1;
    public GameManager GameManager; //Fala quem � o GameManager Pra esse Script
    private Vector3 moveEnemy; //Variavel pra mover o inimigo
     //Variavel Pro rigidBody do Inimigo

    [SerializeField] private Renderer[] renderers; // arraste aqui os meshes do inimigo
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color[] originalColors;


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
        vidaAtual = vidaMax;

        InvokeRepeating("Atirar", InimigoFireTimer, ShotFrequency); //Atira depois de tanto tempo depois repete
        rbEnemy = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;

    }

    private void Update()
    {


           MovimentacaoInimigo();
                
        

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
        
            GameObject tiro = Instantiate(EnemyShot, SpawnEnemy.transform.position, SpawnEnemy.transform.rotation);
           
       
    }

    void Destruir() //Apaga o inimigo da cena
    {
        Destroy(gameObject);
    }
    public void Morrer() // Desativa e depois de um tempo deleta o inimigo
    {
        InimigoSpawnSequence.AddWavePoints();
        GameManager.Mestre.AlterarPontos(50);
        GameManager.Mestre.AlterarChronosPontos(5);
        CancelInvoke();
        Instantiate(Explosao, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Invoke("Destruir", 6f);
    }

    void MovimentacaoInimigo()
    {
        Vector3 movimento = new Vector3(0, 0, direcao * speedInimigo * Time.deltaTime);
        Vector3 novaPos = rbEnemy.position + movimento;

         // troca dire��o ao chegar no limite
        if (novaPos.z <= limiteEsquerda)
        {
            novaPos.z = limiteEsquerda;
            direcao = 1; // vai para a direita
        }
        else if (novaPos.z >= limiteDireita)
        {
            novaPos.z = limiteDireita;
            direcao = -1; // vai para a esquerda
        }

        rbEnemy.MovePosition(novaPos);

       // Limite.z = Mathf.Clamp(Limite.z, -13.5f, 6f);
       // Limite.x = Mathf.Clamp(Limite.x, -22f, 22f);
       // rbEnemy.MovePosition(Limite);
    }
}    




