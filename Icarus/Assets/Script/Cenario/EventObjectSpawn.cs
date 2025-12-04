using UnityEngine;
using System.Collections;

public class EventObjectSpawn : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEvent
    {
        public GameObject prefab; // objeto (buraco negro, estrela)
        public float spawnTime; // quando o objeto aparece (em segundos)
        public Vector3 spawnPosition; // onde o objeto aparece na tela
    }

    public SpawnEvent[] spawnEvents; // lista de eventos de spawn configur�vel

    void Start()
    {
        // acontecimentos pra cada evento/spawn de objeto
        foreach (SpawnEvent e in spawnEvents)
        {

            StartCoroutine(SpawnAtTime(e));
        }
    }
    IEnumerator SpawnAtTime(SpawnEvent e)
    {
        yield return new WaitForSeconds(e.spawnTime);
        SpawnObject(e);
    }

    void SpawnObject(SpawnEvent e)
    {
        //Vector3 worldPos = new Vector3(e.spawnPosition.x, e.spawnPosition.y, 0);
        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        Instantiate(e.prefab, e.spawnPosition, rotation);
    }
}