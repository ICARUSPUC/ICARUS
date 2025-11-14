using UnityEngine;

public class Laser : MonoBehaviour
{
    // Tornamos tempoVida pública para que InimigoLaser possa ler
    public float tempoVida = 2f; // quanto tempo o laser fica ativo (após o crescimento)

    void Start()
    {
        // REMOVIDO: Destruir();
        // O InimigoLaser agora gerencia o tempo total de vida e a destruição.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Player>().Modo == true)
        {
            // mata o jogador
            other.GetComponent<Player>().Derrota();
            // O laser deve ser destruído ao atingir o jogador para evitar dano contínuo
            Destroy(gameObject);
        }
    }

    // O InimigoLaser agora chamará o Destroy(gameObject) após o tempo de vida terminar.
    // Este método Destruir() se torna redundante se não for mais chamado no Start.
    /*
    public void Destruir()
    {
        Destroy(gameObject, tempoVida);
    }
    */
}