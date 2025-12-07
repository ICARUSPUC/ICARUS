using UnityEngine;
using System.Collections;

public class BossDragon : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 500f;
    private float currentHealth;

    [Header("Skill Rate")]
    [Tooltip("Tempo entre habilidades (min e max para rotação aleatória)")]
    public float minSkillRate = 3f;
    public float maxSkillRate = 6f;
    private bool canUseSkills = true;

    [Header("Projectile / Fire Particles")]
    public GameObject fireProjectilePrefab;
    public Transform fireSpawnPoint; // posição relativa configurável

    [Header("Enemy Summon")]
    public GameObject[] enemiesToSummon;
    public Transform summonPoint; // posição relativa configurável
    public int summonAmount = 3;

    [Header("Entrance Settings")]
    public bool comesFromSky = true;
    public float descendDistance = 10f;
    public float descendSpeed = 4f;

    private Animator anim;

    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;

        if (comesFromSky)
            StartCoroutine(DescendFromSky());

        StartCoroutine(SkillRoutine());
    }

    // ------------------------------------------------------------
    // ENTRADA DO CÉU (FlyEnd)
    // ------------------------------------------------------------
    IEnumerator DescendFromSky()
    {
        Vector3 targetPosition = transform.position - new Vector3(0, descendDistance, 0);

        while (transform.position.y > targetPosition.y)
        {
            transform.position -= new Vector3(0, descendSpeed * Time.deltaTime, 0);
            yield return null;
        }

        anim.Play("Idle"); // estado padrão após descer
    }

    // ------------------------------------------------------------
    // ROTINA DE HABILIDADES
    // ------------------------------------------------------------
    IEnumerator SkillRoutine()
    {
        yield return new WaitForSeconds(2f); // espera antes de começar

        while (!isDead)
        {
            if (canUseSkills)
            {
                float delay = Random.Range(minSkillRate, maxSkillRate);

                int skill = Random.Range(0, 2);

                if (skill == 0)
                {
                    anim.Play("BackflipAttack");
                }
                else
                {
                    anim.Play("PraiseSummon");
                }

                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }

    // ------------------------------------------------------------
    // EVENTO: SPAWNAR PROJÉTEIS DURANTE O BACKFLIP
    // (chame via Animation Event)
    // ------------------------------------------------------------
    public void SpawnFireProjectiles()
    {
        if (fireProjectilePrefab == null || fireSpawnPoint == null) return;

        Instantiate(fireProjectilePrefab, fireSpawnPoint.position, fireSpawnPoint.rotation);
    }

    // ------------------------------------------------------------
    // EVENTO: SPAWNAR INIMIGOS DURANTE PRAISE
    // ------------------------------------------------------------
    public void SpawnSummonEnemies()
    {
        if (enemiesToSummon.Length == 0 || summonPoint == null) return;

        for (int i = 0; i < summonAmount; i++)
        {
            int r = Random.Range(0, enemiesToSummon.Length);
            Instantiate(enemiesToSummon[r], summonPoint.position, Quaternion.identity);
        }
    }

    // ------------------------------------------------------------
    // DANO
    // ------------------------------------------------------------
    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ------------------------------------------------------------
    // MORTE
    // ------------------------------------------------------------
    void Die()
    {
        isDead = true;
        anim.Play("Idle"); // sua animação de morte (renomeada)
        StopAllCoroutines();
        canUseSkills = false;
    }
}
