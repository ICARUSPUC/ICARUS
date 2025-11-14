using UnityEngine;
using UnityEngine.SceneManagement;

public class Bala : MonoBehaviour
{
    [SerializeField] float dano = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float DeathTime = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint") && GetComponent<TimeBody>().isrewinding == true)
        {
            Invoke("DestruirBala", 0.05f);
            // Faz a bala ser deletada ao encostar no player ao voltar no tempo
        }    
        if (GetComponent<TimeBody>().isrewinding == true)
            return;
        if (other.CompareTag("Inimigo"))
        {
            other.GetComponent<Inimigo>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("InimigoMelee"))
        {
            other.GetComponent<InimigoMelee>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("InimigoLaser"))
        {
            other.GetComponent<InimigoLaser>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponentInParent<Boss>(); // Aqui � o dano no boss
            if (boss != null)
            {
                boss.TomarDano(dano);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Derrota();
        }
       
    }

    void DestruirBala()
    {
        Destroy(gameObject);
    }

    void Kill()
    {
        DeathTime += Time.deltaTime;
        if (DeathTime > 6f)
            Destroy(gameObject);
    }

    void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
    }

    void Update()
    {
        Move();
        Kill();
    }
}
