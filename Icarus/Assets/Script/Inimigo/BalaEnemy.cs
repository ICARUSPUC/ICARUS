using UnityEngine;
using UnityEngine.SceneManagement;

public class BalaEnemy : MonoBehaviour

    
{
    [SerializeField] float speedEnemy = 10f;
    [SerializeField] float DeathTimeEnemy = 1f;



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
