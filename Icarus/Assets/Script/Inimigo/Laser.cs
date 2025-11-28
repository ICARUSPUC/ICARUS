using UnityEngine;

public class Laser : MonoBehaviour
{
    
    public float tempoVida = 2f; // quanto tempo o laser fica ativo (após o crescimento)

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player playerScript = other.GetComponent<Player>();

        if (other.CompareTag("Player") && other.GetComponent<Player>().Modo == true)
        {
            // mata o jogador
            playerScript.StartCoroutine(playerScript.Derrota());
            // O laser deve ser destruído ao atingir o jogador para evitar dano contínuo
            Destroy(gameObject, 1f);
        }
    }
}