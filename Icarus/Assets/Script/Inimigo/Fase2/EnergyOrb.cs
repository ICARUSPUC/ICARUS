using UnityEngine;
using System.Collections;

public class EnergyOrb : MonoBehaviour
{

    [Header("Tiro")]
    [SerializeField] GameObject energyBallPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float shotCooldown = 2.5f;  
    private float shotTimer = 0f;

    [Header("Movimento")]
    public float speed = 2f;                     
    public float limiteEsquerda = -20f;
    public float limiteDireita = 20f;
    private int direcao = -1;                    

   public GameManager GameManager;


    private Rigidbody rb;

    [Header("Vida")]
    [SerializeField] private float vidaMax = 6f;
    private float vidaAtual;

    [Header("Feedback visual de dano")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color[] originalColors;


    [Header("Particulas")]

    [SerializeField] GameObject Explosao;





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
        rb = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;
        Invoke("Atirar",2);
    }

    

    void Update()
    {
        Movimentacao();
        Disparo();
    }

   
    void Movimentacao()
    {
        Vector3 movimento = new Vector3(0, 0, direcao * speed * Time.deltaTime);
        Vector3 novaPos = rb.position + movimento;

        
        if (novaPos.z <= limiteEsquerda)
        {
            novaPos.z = limiteEsquerda;
            direcao = 1;
        }
        else if (novaPos.z >= limiteDireita)
        {
            novaPos.z = limiteDireita;
            direcao = -1;
        }

        rb.MovePosition(novaPos);
    }

    
   
    void Disparo()
    {
        shotTimer += Time.deltaTime;

        if (shotTimer >= shotCooldown)
        {
            shotTimer = 0;

            GameObject orb = Instantiate(
                energyBallPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            
        }
    }

    void Atirar()
    {
        GameObject orb = Instantiate(
                energyBallPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
    }

   
    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());

        if (vidaAtual <= 0) Morrer();
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

   
    void Destruir()
    {
        Destroy(gameObject);
    }

    void Morrer()
    {
        InimigoSpawnSequence.AddWavePoints();
        GameManager.Mestre.AlterarPontos(80);      
        GameManager.Mestre.AlterarChronosPontos(8);
        Instantiate(Explosao, transform.position, transform.rotation);
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke(nameof(Destruir), 5f);
    }
}
