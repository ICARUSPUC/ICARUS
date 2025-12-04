using UnityEngine;

public class PowerUpEscudo : MonoBehaviour
{
    public float Duracao = 5f;
    public float Queda = 3f;
    public float Limite = -10f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.AtivarEscudo(); // ativa o escudo no player
            Destroy(gameObject);   // destrói o power-up após pegar
        }
        
    }

    void Update()
    {
        transform.position += Vector3.left * Queda * Time.fixedDeltaTime;

        if (transform.position.x <= Limite)
            Destroy(gameObject);
    }

    private void gabriel()
    {

    }

}
