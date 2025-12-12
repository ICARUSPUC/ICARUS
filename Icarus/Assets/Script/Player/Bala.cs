using UnityEngine;
using UnityEngine.SceneManagement;

public class Bala : MonoBehaviour
{
    [SerializeField] float dano = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float TimerDeath = 1f;
    private float DeathTime = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<TimeBody>().isRewinding == true)
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
            Boss boss = other.GetComponent<Boss>(); // Aqui � o dano no boss
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
        else if (other.CompareTag("SpawnPoint") && GetComponent<TimeBody>().isRewinding == true)
        {
            Invoke("DestruirBala", 0.05f);
        }
        else if (other.CompareTag("InimigoEspada"))
        {
            other.GetComponent<InimigoMelee>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("NaveDupla"))
        {
            other.GetComponent<InimigoDuplo>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("InimigoMachine"))
        {
            other.GetComponent<MachineEnemy>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("EnemyTaser"))
        {
            other.GetComponent<MachineEnemyTaser>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("EnemyFast"))
        {
            other.GetComponent<MachineEnemyFast>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("AngelProbe"))
        {
            other.GetComponent<AngelProbe>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("ChaserBall"))
        {
            other.GetComponent<ChaserBall>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("EnergyOrb"))
        {
            other.GetComponent<EnergyOrb>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("SimpleEnemy"))
        {
            other.GetComponent<SimpleEnemy>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Whorm"))
        {
            other.GetComponent<Worm>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("InimigoLaser2"))
        {
            other.GetComponent<InimigoLaser2>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Insetoide"))
        {
            other.GetComponent<SimpleEnemy>().LevarDano(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss3"))
        {
            Boss3 boss3 = other.GetComponentInParent<Boss3>(); // Aqui � o dano no boss
            if (boss3 != null)
            {
                boss3.TomarDano(dano);
            }
            Destroy(gameObject);
        }


    }

    void DestruirBala()
    {
        Destroy(gameObject);
    }

    void Kill()
    {
        DeathTime += Time.deltaTime;
        if (DeathTime > TimerDeath)
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
