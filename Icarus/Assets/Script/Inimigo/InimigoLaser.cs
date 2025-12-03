using UnityEngine;
using System.Collections;

public class InimigoLaser : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource warningaudioSource;
    public AudioSource shootaudioSource;
    [Header("Configuração do inimigo de laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Transform spawnLaser;
    [SerializeField] float intervaloTiro = 5f;

    [SerializeField] float tempoCrescimento = 0.5f;
    [SerializeField] float tempoAlerta = 1.0f;
    [SerializeField] float duracaoLaser = 2f;

    // Configurações do disparo em duas fases
    [SerializeField] float tamanhoCarregamento = 0.3f;
    [SerializeField] float comprimentoMaximo = 50f;

    [SerializeField] float tempoMorte = 2f;
    [SerializeField] float speed = 2f;

    [SerializeField] float tempoMovimento = 3f;
    [SerializeField] float timerMove = 0f;
    [SerializeField] bool movendo = true;

    [Header("Status")]
    [SerializeField] private float vidaMax = 6f;
    private float vidaAtual;

    private GameObject laserAtual;
    private bool atirando = false;
    private Rigidbody rb;
    public GameManager GameManager;

    [Header("Feedback visual de dano")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color[] originalColors;

    [Header("Feedback Visual de Alerta")]
    [SerializeField] private Color alertColor = Color.yellow;
    private bool emAlerta = false;

    [Header("Particulas")]

    [SerializeField] GameObject ExplosaoTiro;

    private TimeBody timeBody;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timeBody = GetComponent<TimeBody>();

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
        vidaAtual = vidaMax;
    }
    void Start()
    {
        InvokeRepeating("AtirarLaser", tempoCrescimento, intervaloTiro);
        vidaAtual = vidaMax;
    }

    void FixedUpdate()
    {    
          
       if (timeBody != null && timeBody.isRewinding)
        {
            RewindSolution();
        }

        if (atirando || emAlerta) movendo = false;

        if (movendo)
        {
            MovimentacaoInimigo();
            timerMove += Time.deltaTime;

            if (timerMove >= tempoMovimento)
                movendo = false;
        }
    }

    void AtirarLaser()
    {
        if (atirando) return;
        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        atirando = true;

      
        
        // --- FASE 1: CARREGAMENTO (QUADRADO) ---
     

        Vector3 posicaoSpawn = spawnLaser.position;
        Quaternion rotacaoInicial = spawnLaser.rotation;

        GameObject laser = Instantiate(laserPrefab, posicaoSpawn, rotacaoInicial);

        // Rastreia o laser atual para o Morrer()
        laserAtual = laser;

        laser.transform.SetParent(spawnLaser);

        laser.transform.localScale = Vector3.zero;

        Laser laserScript = laser.GetComponent<Laser>();
        if (laserScript != null)
        {
            laser.GetComponent<Collider>().enabled = false;
        }

        // Crescimento Uniforme (de 0 até tamanhoCarregamento)
        yield return StartCoroutine(CrescerLaserfase1(laser.transform, tempoCrescimento));

         

        // --- FASE 2: ALERTA (DURANTE O ESTICAMENTO) ---
        warningaudioSource.Play();
        emAlerta = true;
        StartCoroutine(MudarCorAlerta(true));

        yield return new WaitForSeconds(tempoAlerta); // Espera o tempo de alerta


        // --- FASE 3: ESTICAMENTO (DISPARO com DANO ATIVO) ---

       

        if (laserScript != null)
        {
            laser.GetComponent<Collider>().enabled = true;
        }
        shootaudioSource.Play();
        Instantiate(ExplosaoTiro, spawnLaser.transform.position, Quaternion.LookRotation(-spawnLaser.transform.right));
        StartCoroutine(MudarCorAlerta(false));
        emAlerta = false; // Alerta desliga assim que começa a esticar

        yield return StartCoroutine(CrescerLaserfase2(laser.transform, tempoCrescimento, comprimentoMaximo));

       
        // --- FASE 4: LASER ATIVO (DANO E ESPERA) ---
     
        if (laserScript != null)
        {
            // A duração do laser deve ser tratada aqui
            yield return new WaitForSeconds(laserScript.tempoVida);
        }
        else
        {
            yield return new WaitForSeconds(duracaoLaser);
        }

        if (laser != null)
            Destroy(laser);

        

        atirando = false;
        timerMove = 0f;

    }

   
    IEnumerator CrescerLaserfase1(Transform laserTransform, float duracao)
    {
        Vector3 escalaInicial = Vector3.zero;
        Vector3 escalaFinal = Vector3.one * tamanhoCarregamento;
        float tempoPassado = 0f;

        while (tempoPassado < duracao)
        {
            tempoPassado += Time.deltaTime;
            float t = tempoPassado / duracao;

            laserTransform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);

            yield return null;
        }

        laserTransform.localScale = escalaFinal;
    }


   
    IEnumerator CrescerLaserfase2(Transform laserTransform, float duracao, float distanciaAlvo)
    {
        Vector3 escalaInicial = laserTransform.localScale;

      
        Vector3 escalaFinal = new Vector3(distanciaAlvo * 2f, escalaInicial.y, escalaInicial.z);
        float tempoPassado = 0f;

        while (tempoPassado < duracao)
        {
            tempoPassado += Time.deltaTime;
            float t = tempoPassado / duracao;

           
            float newScaleX = Mathf.Lerp(escalaInicial.x, escalaFinal.x, t);

            laserTransform.localScale = new Vector3(newScaleX, escalaInicial.y, escalaInicial.z);

           
            float offset = (newScaleX - escalaInicial.x) * -0.5f;

           
          
            laserTransform.localPosition = new Vector3(offset, laserTransform.localPosition.y, laserTransform.localPosition.z);

            yield return null;
        }

        laserTransform.localScale = escalaFinal;
        
        float finalOffset = (escalaFinal.x - escalaInicial.x) * -0.5f;
        laserTransform.localPosition = new Vector3(finalOffset, laserTransform.localPosition.y, laserTransform.localPosition.z);
    }


    IEnumerator RewindSolution()
    {
        OnRewindStart();

    

        yield return new WaitUntil(() => timeBody.isRewinding == false);

        OnRewindStop();

    }

    public void OnRewindStart()
    {
    
        CancelInvoke("AtirarLaser");

        StopCoroutine("LaserRoutine");
        if (laserAtual != null)
        {
            Destroy(laserAtual);
         
        }
    }

    public void OnRewindStop()
    {

        InvokeRepeating("AtirarLaser", tempoCrescimento, intervaloTiro);


        timerMove = 0f;
        movendo = true;
    }
    IEnumerator MudarCorAlerta(bool alertar)
    {
        Color corAlvo = alertar ? alertColor : originalColors[0];
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_TintColor"))
                r.material.SetColor("_TintColor", corAlvo);
            else if (r.material.HasProperty("_Color"))
                r.material.color = corAlvo;
        }

        yield break;
    }


    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speed * Time.deltaTime;
        Vector3 novaPos = rb.position + movimento;

        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        novaPos.x = Mathf.Clamp(novaPos.x, -22f, 0f);

        rb.MovePosition(novaPos);
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
            if (r.material.HasProperty("_TintColor"))
                r.material.SetColor("_TintColor", damageColor);
            else if (r.material.HasProperty("_Color"))
                r.material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            Color corRestaurar = emAlerta ? alertColor : originalColors[i];

            if (renderers[i].material.HasProperty("_TintColor"))
                renderers[i].material.SetColor("_TintColor", corRestaurar);
            else if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = corRestaurar;
        }
    }

    public void Morrer()
    {
        if (GameManager != null && GameManager.Mestre != null)
            GameManager.Mestre.AlterarPontos(100);
            GameManager.Mestre.AlterarChronosPontos(10);
        InimigoSpawnSequence.AddWavePoints();
        if (laserAtual != null)
        {
            Destroy(laserAtual);
        }
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke(nameof(Destruir), tempoMorte);
    }

    void Destruir() => Destroy(gameObject);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Player>().Derrota();
    }
}