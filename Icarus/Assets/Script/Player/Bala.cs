using UnityEngine;
using UnityEngine.SceneManagement;

public class Bala : MonoBehaviour
{
    // Vari�veis de Movimento e Morte
    [SerializeField] float speed = 10f;
    [SerializeField] float DeathTime = 1f;

    // Vari�vel para o Clip de �udio (Arraste o arquivo de som aqui no Inspector)
    [SerializeField] AudioClip shootSound;
    // Vari�vel para o volume do som
    [SerializeField] float soundVolume = 0.5f;

    // Toca o som assim que a bala � criada
    void Awake()
    {
        // Verifica se um clipe de som foi atribu�do no Inspector
        if (shootSound != null)
        {
            // Toca o clipe de �udio uma �nica vez na posi��o da bala.
            // Isso cria e destr�i um AudioSource tempor�rio, ideal para sons de tiro.
            AudioSource.PlayClipAtPoint(shootSound, transform.position, soundVolume);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<TimeBody>().isrewinding == true)
        {
            return;
        }

        // Verifica se o objeto "other" tem o componente Inimigo antes de tentar cham�-lo
        if (other.CompareTag("Inimigo")) // Mata o inimigo
        {
            if (other.GetComponent<Inimigo>() != null)
            {
                other.GetComponent<Inimigo>().Morrer();
            }
            Destroy(gameObject);
        }

        // Verifica se o objeto "other" tem o componente Player antes de tentar cham�-lo
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>() != null)
            {
                other.GetComponent<Player>().Derrota();
            }
            // SceneManager.LoadScene("Derrota");
        }

    }

    void Kill() // Mata a bala depois de certo tempo
    {
        DeathTime += Time.deltaTime;
        if (DeathTime > 6f)
            Destroy(gameObject);
    }

    void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self); // Move a bala
    }


    void Update()
    {
        Move();
        Kill();
    }
}