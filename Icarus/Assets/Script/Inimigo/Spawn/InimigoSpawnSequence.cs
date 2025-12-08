using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.VFX;

public class InimigoSpawnSequence : MonoBehaviour
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
    public static float WavePoints = 0f;

    [Header("Dialogo após completar a wave")]
    public DialogueSequence Dialogue;

    private int enemiesToSpawnCount = 0;
    private int currentWaveEnemyCount = 0;

    private bool spawning = false; // impede sobreposicao de spawn

    [SerializeField] InimigoSpawnSequenceTutorial tutorialSpawn;
    

    void Start()
    {
        if (gameManager == null)
            gameManager = GameManager.Mestre;
        WavePoints = 0;

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
            WavePoints = 0;
            enemiesToSpawnCount = 0;

            yield return new WaitForSeconds(0.01f);

            yield return new WaitUntil(() => tutorialSpawn.Tutorialspawning == false);
            
                StartCoroutine(SpawnEnemiesInWave(wave));
                Debug.Log($"Iniciando wave: {wave.waveName}");

                yield return new WaitUntil(() => WavePoints >= currentWaveEnemyCount);



                yield return new WaitForSeconds(wave.delayAfterWave);
            
        }


        spawning = false;

        if (Dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(Dialogue);
        }

        Debug.Log("Todas as waves foram conclu�das!");

        GameManager.Mestre?.SceneManger.Invoke("Ganhar", 3f);
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
    public static void AddWavePoints()
    {
        WavePoints++;
        Debug.Log($"Inimigo derrotado. Pontos da Wave: {WavePoints}");
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