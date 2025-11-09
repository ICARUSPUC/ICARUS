using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Atributos do Boss")]
    [SerializeField] float vidaMax = 70f;
    float vidaAtual;

    [Header("Efeitos opcionais")]
    [SerializeField] GameObject efeitoMorte; // arraste um efeito se quiser

    void Start()
    {
        vidaAtual = vidaMax;
    }

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;
        Debug.Log($"Boss tomou dano! Vida atual: {vidaAtual}");

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("💀 Boss derrotado!");

        if (efeitoMorte != null)
            Instantiate(efeitoMorte, transform.position, Quaternion.identity);

        Destroy(transform.root.gameObject); // destrói o boss inteiro
    }
}
