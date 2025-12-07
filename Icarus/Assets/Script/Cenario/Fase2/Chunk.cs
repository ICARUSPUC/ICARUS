using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    
    public float speed = 10f;
    public float destroyX = -1000f; // Quando passar dessa posi��o X, destr�i
    void Update()
    {
        // Move no eixo X positivo
        // Se passou do limite, destruir
        if (transform.position.x <= destroyX)
        {
            Destroy(this.gameObject);
        }

        if (ChunkBackground.Fundo.MoverCenario == false)
        {
            return;
        

        }

        transform.Translate(Vector3.left * speed * Time.deltaTime);


    }
}
