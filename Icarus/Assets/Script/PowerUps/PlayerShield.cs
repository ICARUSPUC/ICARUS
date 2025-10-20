using UnityEngine;

public class PowerUpEscudo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.AtivarEscudo(); // ativa o escudo no player
            Destroy(gameObject);   // destr�i o power-up ap�s pegar
        }
    }
}
