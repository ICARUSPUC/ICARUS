    using UnityEngine;
    using System.Collections;

    public class BossAtaqueController : MonoBehaviour
    {
        // O alvo que ser atacado (o Player)
        private Transform playerTarget;

        [Header("Componentes de Ataque")]
        // Referncia direta aos Transforms das mos
        public Transform maoEsquerdaTransform;
        public Transform maoDireitaTransform;
        [SerializeField] private AudioSource audioSource; // Componente para tocar sons
        public AudioClip somDeAtaque; // O som que toca no incio do ataque/swoosh
        public AudioClip somDeAcerto; // O som que toca quando o alvo  atingido/alcanado

        [Header("Configuraes de Ataque")]
        public float minTempoAtaque = 3f;
        public float maxTempoAtaque = 6f;
        public float duracaoAtaque = 1f; // Tempo que a mo leva para ir e voltar

        [Header("Feedback visual de dano")]
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private float flashDuration = 0.1f;
        private Animator animator;

        private Rigidbody rb;
        private float proximoAtaqueTime;
        private Color[] originalColors;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();

            // Garante que o AudioSource existe
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
            }

            if (renderers != null && renderers.Length > 0)
            {
                originalColors = new Color[renderers.Length];
                for (int i = 0; i < renderers.Length; i++)
                {
                    // Cria uma nova instncia de material para evitar alterar o Asset original
                    renderers[i].material = new Material(renderers[i].material);

                    if (renderers[i].material.HasProperty("_BaseColor"))
                        originalColors[i] = renderers[i].material.GetColor("_BaseColor");
                    else if (renderers[i].material.HasProperty("_Color"))
                        originalColors[i] = renderers[i].material.color;
                }
            }
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            Invoke("AcharPlayer", 0.5f);
        }

        void AcharPlayer()
        {

            // Encontra o Player. IMPORTANTE: Garanta que h APENAS UM objeto com esta tag.
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTarget = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player com a tag 'Player' no encontrado! O Boss no pode atacar.");
                enabled = false; // Desativa o script se o Player no for encontrado
                return;
            }

            proximoAtaqueTime = Time.time + Random.Range(minTempoAtaque, maxTempoAtaque);
        }

        void Update()
        {
            if (playerTarget == null) return; // Se o Player foi destrudo, para a execuo

            // Verifica o tempo para o ataque
            if (Time.time >= proximoAtaqueTime)
            {
                TentarAtaque();
                proximoAtaqueTime = Time.time + Random.Range(minTempoAtaque, maxTempoAtaque);
            }
        }

        void TentarAtaque()
        {
            // Escolhe a mo a ser usada
            Transform maoAlvo = Random.Range(0, 2) == 0 ? maoEsquerdaTransform : maoDireitaTransform;

            // Inicia o movimento de ataque
            StartCoroutine(ExecutarAtaque(maoAlvo));
        }

        IEnumerator DanoVisual()
        {
            foreach (var r in renderers)
            {
                if (r.material.HasProperty("_BaseColor"))
                    r.material.SetColor("_BaseColor", damageColor);
                else if (r.material.HasProperty("_Color"))
                    r.material.color = damageColor;
            }

            yield return new WaitForSeconds(flashDuration);

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_BaseColor"))
                    renderers[i].material.SetColor("_BaseColor", originalColors[i]);
                else if (renderers[i].material.HasProperty("_Color"))
                    renderers[i].material.color = originalColors[i];
            }
        }

    IEnumerator ExecutarAtaque(Transform mao)
    {
        yield return new WaitForSecondsRealtime(0.01f);
        // 1. Armazena a posio de onde a mo est (ponto de partida)
        Vector3 posInicial = mao.position;

        // 2. Define o alvo: um pouco  frente do Player
        Vector3 posFinal = playerTarget.position + playerTarget.forward * 0.5f;

        float tempoPassado = 0f;

        // Toca o som de ataque/swoosh (incio do movimento)
        if (audioSource != null && somDeAtaque != null)
        {
            audioSource.PlayOneShot(somDeAtaque);
        }

        // Movimento de ATAQUE (para frente)
        while (tempoPassado < duracaoAtaque)
        {
            mao.position = Vector3.Lerp(posInicial, posFinal, tempoPassado / duracaoAtaque);
            tempoPassado += Time.deltaTime;
            yield return null;
        }
        mao.position = posFinal;

        // Toca o som de acerto/impacto quando a mo chega ao destino
        if (audioSource != null && somDeAcerto != null)
        {
            audioSource.PlayOneShot(somDeAcerto);
        }

        // Pausa no ponto de ataque
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
            // A Tag do Player que deve ser destrudo ao ser acertado
            private const string PLAYER_TAG = "Player";

            // Esta funo  chamada quando o Collider (marcado como Is Trigger) toca outro Collider (o Player)
            private void OnTriggerEnter(Collider other)
            {
                // Verifica se o objeto que colidiu tem a Tag "Player"
                if (other.CompareTag(PLAYER_TAG))
                {
                    Debug.Log($"O Player foi esmagado pela {gameObject.name}! Destruindo Player.");

                    // Destroi o objeto Player (o 'other'  o Collider do Player)
                    Destroy(other.gameObject);
                }
            }
        }
    }