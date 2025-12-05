using UnityEngine;
using System.Collections;

public class ChaserBall : MonoBehaviour
{
    //Mexer no Atirar

   
    
    [Header("Tiro e Movimento")]
   [SerializeField] GameObject EnemyShot; //Prefab do tiro do Inimigo
    [SerializeField] Transform SpawnEnemy; //Spawn do Tiro do Inimigo
    [SerializeField] float ShotFrequency = 0.7f; // Qu�o rapido ele atira
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
     [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;

    [Header("Mira no jogador")]
    [SerializeField] private float rotateSpeed = 5f;
 
    private Rigidbody rbEnemy;
    private Transform player;

    [Header("Particulas")]

    [SerializeField] GameObject Explosao;



    [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida m�xima
    private float vidaAtual;

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
                else 
                originalColors[i] = renderers[i].material.color;
            }
        }
    }

    void Start()
    {
        vidaAtual = vidaMax;

        // procura o jogador na cena
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;

        // come�a a atirar periodicamente
        InvokeRepeating(nameof(Atirar), ShotFrequency, ShotFrequency);
    }

    void Update()
    {
        

        if (player == null) return;

        transform.LookAt(player,Vector3.up);
    }
    void Atirar()
    {
            if (EnemyShot != null && SpawnEnemy != null)
        {
            Instantiate(EnemyShot, SpawnEnemy.position, SpawnEnemy.rotation);
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
    



    void Destruir() //Apaga o inimigo da cena
    {
        Destroy(gameObject);
    }
    void Morrer() // Desativa e depois de um tempo deleta o inimigo
    {

        GameManager.Mestre.AlterarPontos(100);
        GameManager.Mestre.AlterarChronosPontos(5);
        Instantiate(Explosao, transform.position, transform.rotation);
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke("Destruir", 6f);
    }

}
