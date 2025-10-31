using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemy : MonoBehaviour
{
    [SerializeField] float speedEnemy = 10f;
    [SerializeField] float DeathTimeEnemy = 1f;

    // VARIÁVEIS ADICIONADAS PARA O SOM
    [Header("Áudio de Disparo")]
    [SerializeField] AudioClip somDeDisparo;
    [SerializeField] float volumeDoDisparo = 1f;


    // NOVO MÉTODO: Chamado uma vez quando a bala é criada
    void Start()
    {
        // Toca o som de disparo uma vez no local da bala
        if (somDeDisparo != null)
        {
            AudioSource.PlayClipAtPoint(somDeDisparo, transform.position, volumeDoDisparo);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inimigo") || GetComponent<TimeBody>().isrewinding == true) // Mata o inimigo
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Derrota();
            // SceneManager.LoadScene("Derrota");
        }

        // Adicionando a destruição da bala após colisão (se não for inimigo/rewind),
        // caso contrário ela continua a existir.
        if (!other.CompareTag("Inimigo"))
        {
            Destroy(gameObject);
        }
    }


    void KillBalaEnemy() //Mata a bala depois de certo tempo
    {
        DeathTimeEnemy += Time.deltaTime;
        if (DeathTimeEnemy > 6f)
            Destroy(gameObject);
    }


    void MoveBalaEnemy()
    {
        transform.Translate(Vector3.right * speedEnemy * Time.deltaTime, Space.Self); //Move a bala
    }


    void Update()
    {
        MoveBalaEnemy();
        KillBalaEnemy();
    }
}