using UnityEngine;
using UnityEngine.SceneManagement;

public class Bala : MonoBehaviour

    
{
    [SerializeField] float dano = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float DeathTime = 1f;


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

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Derrota();
            // SceneManager.LoadScene("Derrota");
        }
            
    }
    void Kill() //Mata a bala depois de certo tempo
    {
        DeathTime += Time.deltaTime;
        if (DeathTime > 6f)
            Destroy(gameObject);
    }
    void Move()
    {
        transform.Translate( Vector3.right * speed * Time.deltaTime, Space.Self); //Move a bala
    }
  

    void Update()
    {
        Move();
        Kill();
    }
}
