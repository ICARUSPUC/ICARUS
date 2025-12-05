using UnityEngine;
using System.Collections;

public class AngelProbe : MonoBehaviour
{


//Mexer no Atirar
   
    [Header("Pontos de Movimento (Waypoints)")]
    public Transform pontoInicial;
    public Transform pontoMeio;
    public Transform pontoDireito;
    public Transform pontoSaida;

    public Vector3 rotacaoPontoInicial;
    public Vector3 rotacaoPontoMeio;
    public Vector3 rotacaoPontoDireito;
    public Vector3 rotacaoPontoSaida;

    [Header("Configuração do Padrão")]
    public float velocidade = 8f;
    public float anguloRotacao = -20f;
    public float anguloNormal = 0;

    public GameManager GameManager;



    [Header("Tiro e Movimento")]
   [SerializeField] GameObject EnemyShot; //Prefab do tiro do Inimigo
    [SerializeField] GameObject SpawnEnemy; //Spawn do Tiro do Inimigo

    private bool executando = true;
    private TimeBody timeBody;

    private Rigidbody rbEnemy;

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida máxima
    private float vidaAtual;

     
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color[] originalColors;

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
        vidaAtual = vidaMax;
        
        timeBody = GetComponent<TimeBody>();

        if (executando)
            StartCoroutine(ExecutarPadrão());
    }

    IEnumerator ExecutarPadrão()
    {
        executando = true;

        // 1 — vai para o ponto inicial
        if (pontoInicial != null)
            yield return MoveToComRotacao(pontoInicial.position,rotacaoPontoDireito);

        yield return PadrãoTiro();

        // 2 — vai para o meio
        if (pontoMeio != null)
            yield return MoveToComRotacao(pontoMeio.position, rotacaoPontoDireito);

        yield return PadrãoTiro();

        // 3 — vai para o direito
        if (pontoDireito != null)
            yield return MoveToComRotacao(pontoDireito.position, rotacaoPontoDireito);

        yield return PadrãoTiro();

        if (pontoSaida != null)
        {
        yield return MoveToComRotacao(pontoSaida.position, rotacaoPontoSaida);

        Destroy(gameObject);
        
        }

        executando = false;
    }

    IEnumerator PadrãoTiro()
    {
        Quaternion rotacaoOriginal = transform.rotation;

        for (int i = 0; i < 3; i++)
        {
            if (timeBody != null && timeBody.isRewinding)
                yield break;

            Atirar();
            transform.Rotate(0f, anguloRotacao, 0f);

            yield return new WaitForSeconds(0.3f);
        }

             float t = 0f;
        float velocidadeRetorno = 5f;

        while (t < 1f)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoOriginal, t);
        t += Time.deltaTime * velocidadeRetorno;
        yield return null;
    }

    // força a rotação final ser exatamente a original
    transform.rotation = rotacaoOriginal;

    }

     void Atirar()
    {   
            GameObject tiro = Instantiate(EnemyShot, SpawnEnemy.transform.position, SpawnEnemy.transform.rotation);
           // Tirosom.Play();
        
    }

    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());
        Debug.Log($"{name} levou {dano} de dano. Vida atual: {vidaAtual}/{vidaMax}");
        if (vidaAtual <= 0f) Morrer();
    }
    public void Morrer()
    {
        InimigoSpawnSequence.AddWavePoints();
        gameObject.SetActive(false);
        Invoke(nameof(Destruir), 6f);
    }

    void Destruir()
    {
        Destroy(gameObject);
    }

    IEnumerator MoveToComRotacao(Vector3 alvo, Vector3 rotacaoAlvoEuler)
{
    Quaternion rotacaoAlvo = Quaternion.Euler(rotacaoAlvoEuler);

    while (Vector3.Distance(transform.position, alvo) > 0.1f)
    {
        if (timeBody != null && timeBody.isRewinding)
            yield break;

        // move
        transform.position = Vector3.MoveTowards(
            transform.position,
            alvo,
            velocidade * Time.deltaTime
        );

        // gira suavemente para a rotação alvo
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 3f);

        yield return null;
    }

    // garante rotação exata
    transform.rotation = rotacaoAlvo;
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



}
