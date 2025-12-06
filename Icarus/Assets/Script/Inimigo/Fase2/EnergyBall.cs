using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    
    public float speed = 6f;                
    public float turnSpeed = 4f;            // quão rápido gira em direção ao player
    public float lifetime = 7f;

    private Transform alvo;

    void Start()
    {
        // procura o jogador na cena
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            alvo = player.transform;

            Invoke("Bust",7f);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (alvo == null)
        {
            // segue para frente caso não ache o player
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        // direção para o alvo
        Vector3 direcao = (alvo.position - transform.position).normalized;

        // rotaciona suavemente para o alvo
        Quaternion rotacaoDesejada = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacaoDesejada,
            turnSpeed * Time.deltaTime
        );

        // avança
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void Bust()
    {
        speed = 6f;
        turnSpeed = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player playerScript = other.GetComponent<Player>();

        if (other.CompareTag("Player") && other.GetComponent<Player>().Modo == true)
        {
            // mata o jogador
            playerScript.StartCoroutine(playerScript.Derrota());
            // O laser deve ser destruído ao atingir o jogador para evitar dano contínuo
            Destroy(gameObject, 1f);
        }
    }
}
