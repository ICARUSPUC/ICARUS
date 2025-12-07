using UnityEngine;
using System.Collections.Generic;

public class ChunkBackground : MonoBehaviour
{
    public static ChunkBackground Fundo;
    public float speed = 10f;
    public float stopX = -1000f; // Quando passar dessa posi��o X, destr�i
    public bool MoverCenario = true;

    void Awake()
    {
       
        if (Fundo == null)
        {
            Fundo = this; 
            
        }
        else if (Fundo != this)
        {
            Destroy(gameObject); 
        }
       
    }

    void Start()
    {
        MoverCenario = true;
    }

    void Update()
    {
        // Se passou do limite, destruir
        if (transform.position.x >= stopX)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            MoverCenario = true;
        }

        else
        {
            MoverCenario = false;
        }
    }
}
