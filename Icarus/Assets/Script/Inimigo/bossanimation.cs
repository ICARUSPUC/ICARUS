using UnityEngine;

public class FlutuarIdle : MonoBehaviour
{
    // Variável para armazenar a posição inicial
    private Vector3 posicaoInicial;

    
    public float amplitude = 0.05f; // Amplitude do movimento (meio centímetro para cima/baixo)

    
    public float frequencia = 2f; // Velocidade da flutuação

    void Start()
    {
        // Armazena a posição Y inicial no início
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // 1. Calcula o novo valor Y usando a função seno
        // A função Mathf.Sin(Time.time * frequencia) varia suavemente entre -1 e 1 ao longo do tempo.
        float novoY = Mathf.Sin(Time.time * frequencia) * amplitude;

        // 2. Define a nova posição
        // Adiciona o novoY (o deslocamento) à posição Y inicial.
        transform.position = new Vector3(
            posicaoInicial.x,
            posicaoInicial.y + novoY,
            posicaoInicial.z
        );
    }
}