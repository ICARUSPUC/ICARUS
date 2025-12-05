using UnityEngine;

public class FlutuarIdle : MonoBehaviour
{
    // Vari�vel para armazenar a posi��o inicial
    private Vector3 posicaoInicial;

    
    public float amplitude = 0.05f; // Amplitude do movimento (meio cent�metro para cima/baixo)

    
    public float frequencia = 2f; // Velocidade da flutua��o

    void Start()
    {
        // Armazena a posi��o Y inicial no in�cio
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // 1. Calcula o novo valor Y usando a fun��o seno
        // A fun��o Mathf.Sin(Time.time * frequencia) varia suavemente entre -1 e 1 ao longo do tempo.
        float novoY = Mathf.Sin(Time.time * frequencia) * amplitude;

        // 2. Define a nova posi��o
        // Adiciona o novoY (o deslocamento) � posi��o Y inicial.
        transform.position = new Vector3(
            posicaoInicial.x,
            posicaoInicial.y + novoY,
            posicaoInicial.z
        );
    }
}