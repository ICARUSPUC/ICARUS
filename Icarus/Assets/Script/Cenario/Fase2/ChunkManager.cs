using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
   
    public Transform spawnPoint;
    public List<GameObject> chunkPrefabs;
    public int maxChunks = 5;
    public float chunkLength = 200f;

    private List<GameObject> activeChunks = new List<GameObject>();

    void Start()
    {
        Vector3 nextPos = spawnPoint.position;

        for (int i = 0; i < maxChunks; i++)
        {
            SpawnChunk(nextPos);   // agora funciona
            nextPos.x -= chunkLength;
        }
    }

    void Update()
    {
       

        while (activeChunks.Count < maxChunks)
        {
            Vector3 lastChunkPos = activeChunks[activeChunks.Count - 1].transform.position;

            // posição correta atrás do último chunk
            Vector3 spawnPos = lastChunkPos;
            spawnPos.x -= chunkLength;

            SpawnChunk(spawnPos);
        }
    }

    // Spawn na posição específica
    void SpawnChunk(Vector3 pos)
    {
        int index = Random.Range(0, chunkPrefabs.Count);
        GameObject newChunk = Instantiate(chunkPrefabs[index], pos, Quaternion.identity);
        activeChunks.Add(newChunk);
    }

    // Spawn no spawnPoint
    void SpawnChunk()
    {
        SpawnChunk(spawnPoint.position);
    }
}
