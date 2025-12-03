using UnityEngine;
using System.Collections;

public class InimigoSpawnSequenceTutorial : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;   // Prefab do inimigo
        public Vector3 position;         // Posicao onde vai nascer
        public float delay;              // Tempo de espera antes do pr�ximo inimigo
    }

    [System.Serializable]
    public class WaveData
    {
        public string waveName = "Nova Wave";
        public EnemySpawnData[] enemies;   // Lista de inimigos dessa wave
        public float delayAfterWave = 5f;  // Espera antes da pr�xima wave
    }

    [Header("Sequ�ncia simples (sem waves)")]
    public EnemySpawnData[] spawnList;

    [Header("Sequ�ncia em waves")]
    public WaveData[] waves;

    [Header("Refer�ncia ao GameManager")]
    public GameManager gameManager;
    public TimeManager Zawarudo;
    public TimeBody Dio;
    public static float WavePointsTutorial = 0f;


    private int enemiesToSpawnCount = 0;
    private int currentWaveEnemyCount = 0;

    private bool spawning = false; // impede sobreposicao de spawn
    

    void Start()
    {
        if (gameManager == null)
            gameManager = GameManager.Mestre;
        WavePointsTutorial = 0;

        // Decide qual tipo de sequencia usar
        if (waves != null && waves.Length > 0)
            StartCoroutine(SpawnWavesRoutine());
        else
            StartCoroutine(SpawnSequenceRoutine());
    }

    // Modo simples: apenas spawnList
    IEnumerator SpawnSequenceRoutine()
    {
        spawning = true;

        foreach (var spawn in spawnList)
        {
            yield return new WaitForSeconds(spawn.delay);

            if (spawn.enemyPrefab != null)
            {
                Instantiate(spawn.enemyPrefab, spawn.position, Quaternion.identity);
            }
        }

        spawning = false;
        Debug.Log("Sequ�ncia simples conclu�da!");
    }

    // Modo com waves organizadas
    IEnumerator SpawnWavesRoutine()
    {
        spawning = true;

        foreach (var wave in waves)
        {
            currentWaveEnemyCount = wave.enemies.Length;
            WavePointsTutorial = 0;
            enemiesToSpawnCount = 0;

            StartCoroutine(SpawnEnemiesInWave(wave));
            Debug.Log($"Iniciando wave: {wave.waveName}");

            yield return new WaitUntil(() => WavePointsTutorial >= currentWaveEnemyCount);

           

            yield return new WaitForSeconds(wave.delayAfterWave);
        }

        spawning = false;
        Debug.Log("Todas as waves foram conclu�das!");
    }
    IEnumerator SpawnEnemiesInWave(WaveData wave)
    {
        foreach (var spawn in wave.enemies)
        {
            yield return new WaitForSeconds(spawn.delay);

            if (spawn.enemyPrefab != null)
            {
                Instantiate(spawn.enemyPrefab, spawn.position, Quaternion.identity);
                enemiesToSpawnCount++; 
            }
        }
    }

    // Chamar em todos os inimigos derrotados
    public static void AddWavePointsTutorial()
    {
        WavePointsTutorial++;
        Debug.Log($"Inimigo derrotado. Pontos da Wave: {WavePointsTutorial}");
    }

    public void IniciarSequencia()
    {
        if (!spawning)
        {
            if (waves != null && waves.Length > 0)
                StartCoroutine(SpawnWavesRoutine());
            else
                StartCoroutine(SpawnSequenceRoutine());
        }
    }
}