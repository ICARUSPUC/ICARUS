using UnityEngine;

public class PowerUpEscudo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.AtivarEscudo(); // ativa o escudo no player
            Destroy(gameObject);   // destrói o power-up após pegar
        }
    }
}
