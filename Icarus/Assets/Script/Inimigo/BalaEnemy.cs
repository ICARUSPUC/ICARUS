using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemy : MonoBehaviour

    
{
    [SerializeField] float speedEnemy = 10f;
    [SerializeField] float DeathTimeEnemy = 1f;



    private void OnTriggerEnter(Collider other)
    {
 

        if (other.CompareTag("Player") && GetComponent<TimeBody>().isrewinding == false)
        {
            other.GetComponent<Player>().Derrota();
            // SceneManager.LoadScene("Derrota");
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
