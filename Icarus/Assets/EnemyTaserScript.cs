using Unity.Collections;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineEnemyTaser : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject tiroPrefab;
    // Alterado para um array de Transform para os múltiplos pontos de disparo
    public Transform[] firePoints;
    public float fireRate = 0.5f; // Cadência de tiro diminuída (tempo entre tiros aumentado)
    private float fireTimer = 0f;
    private int currentFirePointIndex = 0; // Para alternar entre os pontos de disparo
    private const string bulletTag = "EnemyTaser"; // Tag para o tiro

        
    [Header("Status")]
    [SerializeField] private float vidaMax = 5f;
    private float vidaAtual;

    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color[] originalColors;

    private Rigidbody rbEnemy;
    [SerializeField] private float speedInimigo = 5f;


    void Start()
    {
        rbEnemy = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;

        // Salva as cores originais dos materiais
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                originalColors[i] = renderers[i].material.color;
        }

        // Verifica se há pelo menos um ponto de disparo configurado
        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogError("O inimigo precisa de pelo menos um ponto de disparo (FirePoint) configurado.");
            enabled = false; // Desativa o script se não houver FirePoints
        }
    }

    void Update()
    {
        // *** LÓGICA DE DISPARO REVISADA ***
        // O tiro é agora ACIONADO CONTINUAMENTE, independentemente da animação.
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Disparar();
            fireTimer = 0f;
        }

        // As linhas abaixo foram removidas/ignoradas para garantir o tiro contínuo:
        // girando = anim.GetBool("Girando");
        // if (girando) { ... }
    }

    void FixedUpdate()
    {
        MovimentacaoInimigo();
    }

    void Disparar()
    {
        // Pega o ponto de disparo atual
        Transform currentFirePoint = firePoints[currentFirePointIndex];

        // Cria a instância do tiro
        GameObject novoTiro = Instantiate(tiroPrefab, currentFirePoint.position, currentFirePoint.rotation);

        // Define a tag do tiro
        novoTiro.tag = bulletTag;

        // Avança para o próximo ponto de disparo no array (circularmente)
        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
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
        // aplica a cor de dano
        foreach (Renderer r in renderers)
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

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;

        Vector3 novaPos = rbEnemy.position + movimento;

        // limites (mantido os seus limites originais)
        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        novaPos.x = Mathf.Clamp(novaPos.x, -22f, 22f);

        rbEnemy.MovePosition(novaPos);
    }

    public void Morrer()
    {
        gameObject.SetActive(false);
        Invoke(nameof(Destruir), 6f);
    }

    void Destruir()
    {
        Destroy(gameObject);
    }
}