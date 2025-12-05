using Unity.Collections;
using UnityEngine;
using System.Collections;

public class MachineEnemy : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject tiroPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer = 0;

    [Header("Movimento")]
        [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
    [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;

    bool movendo = true;

    public GameManager GameManager;

    [Header("Anima��o")]
    public Animator anim;
    private bool girando = false;

    [Header("Status")]
    [SerializeField] private float vidaMax = 5f;
    private float vidaAtual;

    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Particulas")]

    [SerializeField] GameObject Explosao;

    private Color[] originalColors;

    private Rigidbody rbEnemy;
   

    private void Awake()
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
        rbEnemy = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;
    }

    void Update()
    {
        // Anima��o controla o tiro
        girando = anim.GetBool("Girando");

        if (girando)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Disparar();
                fireTimer = 0;
            }
        }
    }

    void FixedUpdate()
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

    void Disparar()
    {
        Instantiate(tiroPrefab, firePoint.position, firePoint.rotation);
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

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;

        Vector3 novaPos = rbEnemy.position + movimento;

        // limites
        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        novaPos.x = Mathf.Clamp(novaPos.x, -22f, 22f);

        rbEnemy.MovePosition(novaPos);
    }

    public void Morrer()
    {
        if (GameManager != null && GameManager.Mestre != null)
            GameManager.Mestre.AlterarPontos(200);
        GameManager.Mestre.AlterarChronosPontos(10);
        InimigoSpawnSequence.AddWavePoints();
        Instantiate(Explosao, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Invoke(nameof(Destruir), 6f);
    }

    void Destruir()
    {
        Destroy(gameObject);
    }
}
