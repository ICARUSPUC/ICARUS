using Unity.Collections;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineEnemyFast : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject tiroPrefab;
    public Transform[] firePoints;
    public float fireRate = 0.15f; // <--- AUMENTADO A CADÊNCIA: Tempo entre tiros menor (0.15s)
    private float fireTimer = 0f;
    private int currentFirePointIndex = 0;
    private const string bulletTag = "EnemyFast"; 

    [Header("Animação")]
    public Animator anim;
    private bool girando = false;

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

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                originalColors[i] = renderers[i].material.color;
        }

        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogError("O inimigo precisa de pelo menos um ponto de disparo (FirePoint) configurado.");
            enabled = false;
        }
    }

    void Update()
    {
        // O tiro é acionado continuamente (sem depender da animação)
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Disparar();
            fireTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        MovimentacaoInimigo();
    }

    void Disparar()
    {
        Transform currentFirePoint = firePoints[currentFirePointIndex];

        // O tiro ainda usa o mesmo prefab, mas a velocidade é controlada pelo script da bala (abaixo).
        GameObject novoTiro = Instantiate(tiroPrefab, currentFirePoint.position, currentFirePoint.rotation);

        novoTiro.tag = bulletTag;

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
        foreach (Renderer r in renderers)
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

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;
        Vector3 novaPos = rbEnemy.position + movimento;

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