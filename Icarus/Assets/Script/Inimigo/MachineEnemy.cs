using Unity.Collections;
using UnityEngine;
using System.Collections;

public class MachineEnemy : MonoBehaviour
{

    [Header("disparo")]
    public GameObject tiroPrefab; // local para alocar perfab do tiro inimigo
    public Transform firePoint; //origem do tiro
    public float fireRate = 0.2f; // tempo de disparo 
    public float fireTimer; 

    [Header("Animação")]
    public Animator anim; // animação 
    private bool girando = false; // ativa e desativa a animacao


    [Header("Status")]
    [SerializeField] private float vidaMax = 5f; // Vida máxima
    private float vidaAtual;


    [SerializeField] private Renderer[] renderers; // arraste aqui os meshes do inimigo
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;


    bool movendo = true;
    float InimigoFireTimer = 1;
    public GameManager GameManager; //Fala quem é o GameManager Pra esse Script
    private Vector3 moveEnemy; //Variavel pra mover o inimigo
    private Rigidbody rbEnemy; //Variavel Pro rigidBody do Inimigo
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
    [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;

    private Color[] originalColors;



    // Update is called once per frame
    void Update()
    {
        girando = anim.GetBool("Girando");
        

        if (girando)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer > fireRate)
            {
                Disparar();
                fireTimer = 0;
            }


        }
    }

    void Disparar()
    {
        Instantiate(tiroPrefab, firePoint.position, firePoint.rotation);

    }


    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());
        Debug.Log($"{name} levou {dano} de dano. Vida atual: {vidaAtual}/{vidaMax}");
        if (vidaAtual <= 0f) Morrer();
    }


    void Destruir() //Apaga o inimigo da cena
    {
        Destroy(gameObject);
    }

    public void Morrer() // Desativa e depois de um tempo deleta o inimigo
    {

        GameManager.Mestre.AlterarPontos(50);
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke("Destruir", 6f);
    }

    void Start()
    {
       
        rbEnemy = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;

    }

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;
        Vector3 Limite = rbEnemy.position + movimento;
        Limite.z = Mathf.Clamp(Limite.z, -13.5f, 6f);
        Limite.x = Mathf.Clamp(Limite.x, -22f, 22f);
        rbEnemy.MovePosition(Limite);
    }

    IEnumerator DanoVisual()
    {
        // muda a cor
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_Color"))
                r.material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        // volta à cor original
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }
}    

