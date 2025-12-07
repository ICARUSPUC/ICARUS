using UnityEngine;
using System.Collections;

public class BossDragon : MonoBehaviour
{
    // =========================================================================
    // 🐉 Status do Boss
    // =========================================================================
    [Header("Boss Stats")]
    public float vidaMax = 500f;
    private float vidaAtual;

    [Header("Dano / Feedback Visual")]
    public Renderer[] renderers;
    public Color damageColor = Color.red;
    public float flashDuration = 0.1f;
    private Color[] originalColors;

    // =========================================================================
    // 🔥 Habilidades / Spawn
    // =========================================================================
    [Header("Skill Rate")]
    public float minSkillRate = 3f;
    public float maxSkillRate = 6f;
    private bool canUseSkills = true;

    [Header("Projectile / Fire Particles")]
    public GameObject fireProjectilePrefab;
    public Transform fireSpawnPoint;

    [Header("Enemy Summon")]
    public GameObject[] enemiesToSummon;
    public Transform summonPoint;
    public int summonAmount = 3;

    // =========================================================================
    // 🎥 Entrada
    // =========================================================================
    [Header("Entrance Settings")]
    public bool comesFromSky = true;
    public float descendDistance = 10f;
    public float descendSpeed = 4f;

    private Animator anim;
    private bool isDead = false;

    // =========================================================================
    // 🧠 Inicialização
    // =========================================================================
    void Awake()
    {
        anim = GetComponent<Animator>();
        vidaAtual = vidaMax;

        // Guarda cores originais
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
        if (comesFromSky)
            StartCoroutine(DescendFromSky());

        StartCoroutine(SkillRoutine());
    }

    // =========================================================================
    // 🚁 Entrada do Céu
    // =========================================================================
    IEnumerator DescendFromSky()
    {
        Vector3 targetPosition = transform.position - new Vector3(0, descendDistance, 0);

        while (transform.position.y > targetPosition.y)
        {
            transform.position -= new Vector3(0, descendSpeed * Time.deltaTime, 0);
            yield return null;
        }

        anim.Play("Idle");
    }

    // =========================================================================
    // 🔄 Rotação de Skills
    // =========================================================================
    IEnumerator SkillRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (!isDead)
        {
            if (canUseSkills)
            {
                float delay = Random.Range(minSkillRate, maxSkillRate);
                int skill = Random.Range(0, 2);

                if (skill == 0)
                    anim.Play("BackflipAttack");
                else
                    anim.Play("PraiseSummonStart");

                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }

    // =========================================================================
    // 🔥 Spawn de Projéteis (chamado pelo StateMachineBehaviour)
    // =========================================================================
    public void SpawnFireProjectiles()
    {
        if (fireProjectilePrefab == null || fireSpawnPoint == null) return;
        Instantiate(fireProjectilePrefab, fireSpawnPoint.position, fireSpawnPoint.rotation);
    }

    // =========================================================================
    // 👹 Spawn de Inimigos (chamado pelo StateMachineBehaviour)
    // =========================================================================
    public void SpawnSummonEnemies()
    {
        if (enemiesToSummon.Length == 0 || summonPoint == null) return;

        for (int i = 0; i < summonAmount; i++)
        {
            int r = Random.Range(0, enemiesToSummon.Length);
            Instantiate(enemiesToSummon[r], summonPoint.position, Quaternion.identity);
        }
    }

    // =========================================================================
    // 💥 Dano Padronizado (igual ao InimigoMelee)
    // =========================================================================
    public void LevarDano(float dano)
    {
        if (isDead) return;

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
            if (renderers[i].material.HasProperty("_TintColor"))
                renderers[i].material.SetColor("_TintColor", originalColors[i]);
            else if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }

    // =========================================================================
    // ☠️ Morte do Boss
    // =========================================================================
    void Morrer()
    {
        if (isDead) return;

        isDead = true;
        canUseSkills = false;

        StopAllCoroutines();
        anim.Play("Death"); // coloque o nome da animação certa

        // Se quiser: drop, explosão, partículas etc.
    }
}
