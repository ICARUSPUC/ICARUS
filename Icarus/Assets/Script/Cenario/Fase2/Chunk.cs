using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    
    public float speed = 10f;
    public float destroyX = -1000f; // Quando passar dessa posição X, destrói

    void Update()
    {
        // Move no eixo X positivo
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Se passou do limite, destruir
        if (transform.position.x <= destroyX)
        {
            Destroy(this.gameObject);
        }
    }
}
