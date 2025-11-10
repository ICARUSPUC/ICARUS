using UnityEngine;
using UnityEngine.SceneManagement;

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
        

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        GameManager.Mestre.SceneManger.Ganhar();


        if (efeitoMorte != null)
            Instantiate(efeitoMorte, transform.position, Quaternion.identity);

        Destroy(transform.root.gameObject); // destrói o boss inteiro
    }
}
