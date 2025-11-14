using UnityEngine;
using System.Collections;

public class InimigoMelee : MonoBehaviour
{
    [Header("Configuração do inimigo corpo a corpo")]
    [SerializeField] float speed = 5f;
    [SerializeField] float dano = 1f;
    [SerializeField] float tempoMorte = 2f;

    [SerializeField] float tempoMovimento = 0f;
    [SerializeField] float timerMove = 0f;
    [SerializeField] bool movendo = true;

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f;
    private float vidaAtual;

    private Rigidbody rb;
    private bool dash = false;
    private bool atacou = false;

    public GameManager GameManager;

    [Header("Feedback visual de dano")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color[] originalColors;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(renderers[i].material);

                if (renderers[i].material.HasProperty("_BaseColor"))
                    originalColors[i] = renderers[i].material.GetColor("_BaseColor");
                else if (renderers[i].material.HasProperty("_Color"))
                    originalColors[i] = renderers[i].material.color;
            }
        }
    }

    void Start()
    {
        Invoke("Dash", 2f);
        vidaAtual = vidaMax;
    }

    void FixedUpdate()
    {
        if (movendo && !atacou)
        {
            MovimentacaoInimigo();
            timerMove += Time.deltaTime;

            if (dash)
                speed = 15;

            if (tempoMovimento > 0 && timerMove >= tempoMovimento)
                movendo = false;
        }
    }

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speed * Time.deltaTime;
        Vector3 novaPos = rb.position + movimento;

        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        rb.MovePosition(novaPos);
    }

    void Dash() => dash = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Derrota();
            atacou = true;
            Morrer();
        }
    }

    void Update()
    {
        if (transform.position.x < -30f || transform.position.z < -20f || transform.position.z > 10f)
            MorrerFora();
    }

    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());

        if (vidaAtual <= 0f)
            Morrer();
    }

    IEnumerator DanoVisual()
    {
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_BaseColor"))
                r.material.SetColor("_BaseColor", damageColor);
            else if (r.material.HasProperty("_Color"))
                r.material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_BaseColor"))
                renderers[i].material.SetColor("_BaseColor", originalColors[i]);
            else if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }

    public void Morrer()
    {
        if (GameManager.Mestre != null)
            GameManager.Mestre.AlterarPontos(75);

        CancelInvoke();
        gameObject.SetActive(false);
        Invoke("Destruir", tempoMorte);
    }

    public void MorrerFora()
    {
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke("Destruir", tempoMorte);
    }

    void Destruir() => Destroy(gameObject);
}
