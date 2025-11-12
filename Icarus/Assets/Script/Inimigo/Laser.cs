using UnityEngine;

public class Laser : MonoBehaviour
{
    // =========================================================================
    // ⚙️ Configuração (MANTIDO ORIGINAL)
    // =========================================================================
    [SerializeField] float tempoVida = 2f; // quanto tempo o laser fica ativo

    // =========================================================================
    // 🔄 Métodos Padrão do Unity
    // =========================================================================

    void Start()
    {
        // Otimização: Chamando Destruir() no Start, que usa o tempoVida
        // Isso garante que o laser se autodestrua após o tempo configurado.
        Destruir();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tentativa de obter o componente Player do objeto colidido
        Player player = other.GetComponent<Player>();

        // Checa se é o Player E se o Modo do Player é o Modo Principal (Modo == true)
        if (other.CompareTag("Player") && player != null && player.Modo == true)
        {
            // Otimização: Usando o operador ?. para chamar Derrota com segurança
            player.Derrota();
        }
    }

    // =========================================================================
    // 🗑️ Destruição
    // =========================================================================

    public void Destruir()
    {
        // Destroi o objeto do jogo após o tempo definido em tempoVida
        Destroy(gameObject, tempoVida);
    }
}