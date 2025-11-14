using UnityEngine;
using System.Collections;

public class BossAtaqueController : MonoBehaviour
{
    // O alvo que será atacado (o Player)
    private Transform playerTarget;

    [Header("Componentes de Ataque")]
    // Referência direta aos Transforms das mãos
    public Transform maoEsquerdaTransform;
    public Transform maoDireitaTransform;

    [Header("Configurações de Ataque")]
    public float minTempoAtaque = 3f;
    public float maxTempoAtaque = 6f;
    public float duracaoAtaque = 1f; // Tempo que a mão leva para ir e voltar

    private float proximoAtaqueTime;

    void Start()
    {
        // Encontra o Player. IMPORTANTE: Garanta que há APENAS UM objeto com esta tag.
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player com a tag 'Player' não encontrado! O Boss não pode atacar.");
            enabled = false; // Desativa o script se o Player não for encontrado
            return;
        }

        proximoAtaqueTime = Time.time + Random.Range(minTempoAtaque, maxTempoAtaque);
    }

    void Update()
    {
        if (playerTarget == null) return; // Se o Player foi destruído, para a execução

        // Verifica o tempo para o ataque
        if (Time.time >= proximoAtaqueTime)
        {
            TentarAtaque();
            proximoAtaqueTime = Time.time + Random.Range(minTempoAtaque, maxTempoAtaque);
        }
    }

    void TentarAtaque()
    {
        // Escolhe a mão a ser usada
        Transform maoAlvo = Random.Range(0, 2) == 0 ? maoEsquerdaTransform : maoDireitaTransform;

        // Inicia o movimento de ataque
        StartCoroutine(ExecutarAtaque(maoAlvo));
    }

    IEnumerator ExecutarAtaque(Transform mao)
    {
        // 1. Armazena a posição de onde a mão está (ponto de partida)
        Vector3 posInicial = mao.position;

        // 2. Define o alvo: um pouco à frente do Player
        // Nota: Você pode ajustar o '0.5f' para o 'over-shoot'
        Vector3 posFinal = playerTarget.position + playerTarget.forward * 0.5f;

        float tempoPassado = 0f;

        // Movimento de ATAQUE (para frente)
        while (tempoPassado < duracaoAtaque)
        {
            mao.position = Vector3.Lerp(posInicial, posFinal, tempoPassado / duracaoAtaque);
            tempoPassado += Time.deltaTime;
            yield return null;
        }
        mao.position = posFinal;

        // Pausa no ponto de ataque (pode ser substituído pela sua animação de 'soco' completo)
        yield return new WaitForSeconds(0.2f);

        // Movimento de RETORNO
        tempoPassado = 0f;
        while (tempoPassado < duracaoAtaque)
        {
            mao.position = Vector3.Lerp(posFinal, posInicial, tempoPassado / duracaoAtaque);
            tempoPassado += Time.deltaTime;
            yield return null;
        }
        mao.position = posInicial;
    }


    public class MaoDanoPlayer : MonoBehaviour
    {
        // A Tag do Player que deve ser destruído ao ser acertado
        private const string PLAYER_TAG = "Player";

        // Esta função é chamada quando o Collider (marcado como Is Trigger) toca outro Collider (o Player)
        private void OnTriggerEnter(Collider other)
        {
            // Verifica se o objeto que colidiu tem a Tag "Player"
            if (other.CompareTag(PLAYER_TAG))
            {
                Debug.Log($"O Player foi esmagado pela {gameObject.name}! Destruindo Player.");

                // Destroi o objeto Player (o 'other' é o Collider do Player)
                Destroy(other.gameObject);
            }
        }
    }
}