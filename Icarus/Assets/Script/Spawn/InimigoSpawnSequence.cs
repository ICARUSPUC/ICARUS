using UnityEngine;
using System.Collections;

public class InimigoSpawnSequence : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;   // Prefab do inimigo
        public Vector3 position;         // Posição onde vai nascer
        public float delay;              // Tempo de espera antes do próximo inimigo
    }

    [System.Serializable]
    public class WaveData
    {
        public string waveName = "Nova Wave";
        public EnemySpawnData[] enemies;   // Lista de inimigos dessa wave
        public float delayAfterWave = 5f;  // Espera antes da próxima wave
    }

    [Header("Sequência simples (sem waves)")]
    public EnemySpawnData[] spawnList;

    [Header("Sequência em waves")]
    public WaveData[] waves;

    [Header("Referência ao GameManager")]
    public GameManager gameManager;

    private bool spawning = false; // impede sobreposição de spawn

    void Start()
    {
        if (gameManager == null)
            gameManager = GameManager.Mestre;

        // Decide qual tipo de sequência usar
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
        Debug.Log("Sequência simples concluída!");
    }

    // Modo com waves organizadas
    IEnumerator SpawnWavesRoutine()
    {
        spawning = true;

        foreach (var wave in waves)
        {
            Debug.Log($"Iniciando wave: {wave.waveName}");

            foreach (var spawn in wave.enemies)
            {
                yield return new WaitForSeconds(spawn.delay);

                if (spawn.enemyPrefab != null)
                {
                    Instantiate(spawn.enemyPrefab, spawn.position, Quaternion.identity);
                }
            }

            Debug.Log($"Wave '{wave.waveName}' concluída. Aguardando próxima...");
            yield return new WaitForSeconds(wave.delayAfterWave);
        }

        spawning = false;
        Debug.Log("Todas as waves foram concluídas!");
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