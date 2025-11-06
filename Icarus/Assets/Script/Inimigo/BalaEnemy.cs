using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemy : MonoBehaviour

    
{
    [SerializeField] float speedEnemy = 10f;
    [SerializeField] float DeathTimeEnemy = 1f;


 private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<TimeBody>().isrewinding == true)
        {
            return;
        }
        if (other.CompareTag("Inimigo")) // Mata o inimigo
        {
            other.GetComponent<Inimigo>().LevarDano(1);
            Destroy(gameObject);
        }

        if (other.CompareTag("InimigoMelee")) // Mata o inimigo
        {
            other.GetComponent<InimigoMelee>().LevarDano(1);
            Destroy(gameObject);
        }

        if (other.CompareTag("InimigoLaser")) // Mata o inimigo
        {
            other.GetComponent<InimigoLaser>().LevarDano(1);
            Destroy(gameObject);
        }

        if (other.CompareTag("SpawnPoint") && GetComponent<TimeBody>().isrewinding == true)
        {
            Invoke("DestruirBala",0.05f);
            // Faz a bala ser deletada ao encostar no enemy ao voltar no tempo
        }


    }

    void DestruirBala()
    {
        Destroy(gameObject);
    }
    void KillBalaEnemy() //Mata a bala depois de certo tempo
    {
        DeathTimeEnemy += Time.deltaTime;
        if (DeathTimeEnemy > 6f)
            Destroy(gameObject);
    }
    void MoveBalaEnemy()
    {
        transform.Translate( Vector3.right * speedEnemy * Time.deltaTime, Space.Self); //Move a bala
    }

    void Update()
    {
        MoveBalaEnemy();
        KillBalaEnemy();
    }
}
