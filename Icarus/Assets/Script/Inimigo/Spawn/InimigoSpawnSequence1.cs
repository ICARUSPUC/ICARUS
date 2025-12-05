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

    [Header("Dialogo após completar a wave")]
    public DialogueSequence Dialogue;


    private int enemiesToSpawnCount = 0;
    private int currentWaveEnemyCount = 0;

    public bool Tutorialspawning = false; // impede sobreposicao de spawn
    
   

    void Start()
    {
        if (gameManager == null)
            gameManager = GameManager.Mestre;
        InimigoSpawnSequence.WavePoints = 0;

        // Decide qual tipo de sequencia usar
        if (waves != null && waves.Length > 0)
            StartCoroutine(SpawnWavesRoutine());
        else
            StartCoroutine(SpawnSequenceRoutine());
    }

    // Modo simples: apenas spawnList
    IEnumerator SpawnSequenceRoutine()
    {
        Tutorialspawning = true;

        foreach (var spawn in spawnList)
        {
            yield return new WaitForSeconds(spawn.delay);

            if (spawn.enemyPrefab != null)
            {
                Instantiate(spawn.enemyPrefab, spawn.position, Quaternion.identity);
            }
        }

        Tutorialspawning = false;
        Debug.Log("Sequ�ncia simples conclu�da!");
    }

    // Modo com waves organizadas
    IEnumerator SpawnWavesRoutine()
    {
        Tutorialspawning = true;

        foreach (var wave in waves)
        {
            currentWaveEnemyCount = wave.enemies.Length;
            InimigoSpawnSequence.WavePoints = 0;
            enemiesToSpawnCount = 0;
            yield return new WaitForSeconds(0.01f);    
            StartCoroutine(SpawnEnemiesInWave(wave));
            Debug.Log($"Iniciando wave: {wave.waveName}");
            Debug.Log($"Wave {wave.waveName} - Esperando {currentWaveEnemyCount} kills."); // LOG 1
            yield return new WaitUntil(() => InimigoSpawnSequence.WavePoints >= currentWaveEnemyCount);
            Debug.Log($"Wave {wave.waveName} CONCLUÍDA! Avançando."); // LOG 2


            yield return new WaitForSeconds(wave.delayAfterWave);
        }


        
        
        if (Dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(Dialogue);
        }
        Tutorialspawning = false;
        InimigoSpawnSequence.WavePoints = 0;
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

    public void IniciarSequencia()
    {
        if (!Tutorialspawning)
        {
            if (waves != null && waves.Length > 0)
                StartCoroutine(SpawnWavesRoutine());
            else
                StartCoroutine(SpawnSequenceRoutine());
        }
    }
}