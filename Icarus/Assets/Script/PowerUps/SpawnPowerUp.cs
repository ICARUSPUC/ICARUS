using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject powerUpPrefab; // O prefab do power-up 
    [SerializeField] Vector3 areaMin = new Vector3(-20f, 0f, -10f); // Limite m�nimo do spawn
    [SerializeField] Vector3 areaMax = new Vector3(20f, 0f, 10f);  // Limite m�ximo do spawn
    [SerializeField] float tempoSpawn = 40f; // Tempo fixo entre cada spawn

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempoSpawn);

            Vector3 pos = new Vector3(
                Random.Range(areaMin.x, areaMax.x),
                Random.Range(areaMin.y, areaMax.y),
                Random.Range(areaMin.z, areaMax.z)
            );

            Instantiate(powerUpPrefab, pos, Quaternion.identity);
        }
    }
}
