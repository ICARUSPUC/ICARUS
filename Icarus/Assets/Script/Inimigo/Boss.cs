using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    // =========================================================================
    // ⚙️ Atributos do Boss (MANTIDO ORIGINAL)
    // =========================================================================
    [Header("Atributos do Boss")]
    [SerializeField] float vidaMax = 70f;
    float vidaAtual;

    [Header("Efeitos opcionais")]
    [SerializeField] GameObject efeitoMorte; // arraste um efeito se quiser

    // =========================================================================
    // 🔄 Métodos Padrão do Unity
    // =========================================================================

    void Start()
    {
        vidaAtual = vidaMax;
    }

    // =========================================================================
    // 💥 Dano e Morte
    // =========================================================================

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        // 1. Lógica de Vitória
        // Usando o operador ?. (null-conditional) para garantir que não haverá erro
        // se o GameManager.Mestre não estiver inicializado.
        GameManager.Mestre?.SceneManger.Ganhar();

        // 2. Efeito Visual
        if (efeitoMorte != null)
        {
            Instantiate(efeitoMorte, transform.position, Quaternion.identity);
        }

        // 3. Destruição do Objeto
        // Garante que o objeto pai (raiz) do boss seja destruído.
        Destroy(transform.root.gameObject);
    }
}
