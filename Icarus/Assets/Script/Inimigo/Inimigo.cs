using UnityEngine;
using System.Collections;

public class Inimigo : MonoBehaviour
{
    [Header("Tiro e Movimento")]
    [SerializeField] GameObject EnemyShot;
    [SerializeField] GameObject SpawnEnemy;
    [SerializeField] float ShotFrequency = 10f;
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
    [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;

    [Header("Status")]
    [SerializeField] private float vidaMax = 3f;
    private float vidaAtual;

    [Header("Dano Visual")]
    [SerializeField] private Renderer[] renderers; // arraste aqui os MeshRenderers
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color[] originalColors;

    bool movendo = true;
    float InimigoFireTimer = 1;
    public GameManager GameManager;
    private Rigidbody rbEnemy;

    void Awake()
    {
        rbEnemy = GetComponent<Rigidbody>();

        // Garante materiais independentes (pra não mudar cor de todos os inimigos)
        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(renderers[i].material); // material único
                if (renderers[i].material.HasProperty("_Color"))
                    originalColors[i] = renderers[i].material.color;
            }
        }
    }

    void Start()
    {
        vidaAtual = vidaMax;
        InvokeRepeating(nameof(Atirar), InimigoFireTimer, ShotFrequency);
    }

    void Update()
    {
        if (movendo)
        {
            MovimentacaoInimigo();
            timerMove += Time.deltaTime;

            if (timerMove >= tempoMovimento)
                movendo = false;
        }
    }

    public void LevarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());

        Debug.Log($"{name} levou {dano} de dano. Vida atual: {vidaAtual}/{vidaMax}");

        if (vidaAtual <= 0f)
            Morrer();
    }

    IEnumerator DanoVisual()
    {
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_Color"))
                r.material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }

    void Atirar()
    {
        if (GetComponent<TimeBody>().isrewinding)
            return;

        Instantiate(EnemyShot, SpawnEnemy.transform.position, SpawnEnemy.transform.rotation);
    }

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.deltaTime;
        Vector3 limite = rbEnemy.position + movimento;
        limite.z = Mathf.Clamp(limite.z, -13.5f, 6f);
        limite.x = Mathf.Clamp(limite.x, -22f, 22f);
        rbEnemy.MovePosition(limite);
    }

    public void Morrer()
    {
        GameManager.Mestre.AlterarPontos(50);
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke(nameof(Destruir), 6f);
    }

    void Destruir()
    {
        Destroy(gameObject);
    }
}
