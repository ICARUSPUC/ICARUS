using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float tempoVida = 2f; // quanto tempo o laser fica ativo

    void Start()
    {
        Destruir();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // mata o jogador
            other.GetComponent<Player>().Derrota();
        }
    }

    public void Destruir()
    {
        Destroy(gameObject, tempoVida);
    }
}
