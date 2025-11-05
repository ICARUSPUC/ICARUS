using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement; // necessário para sceneLoaded

public class GameManager : MonoBehaviour
{
    public static GameManager Mestre;

    [SerializeField] float tempo = 0.25f;

    public int Pontos = 0;

    [SerializeField] GameObject inimigoPrefab;
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject[] Wave;
    [SerializeField] float SpawnInterval = 5;
    Vector3 SpawnPosition;

    public Player Player;

    float SpawnTimer = 0;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Atualiza SpawnPoint ao carregar cena
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EncontrarSpawnPoint();
    }

    private void Awake()
    {
        if (Mestre == null)
        {
            Mestre = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tenta pegar SpawnPoint da cena inicial (caso exista)
        EncontrarSpawnPoint();
    }
    private void Start()
    {
        //InvokeRepeating("SpawnWave", 1f, 8f); //invoca as waves repetidamente SiSTEMA ANTIGO DE WAVE
        Pontos = 0;
    }

  
    void Update()
    {
        Zawarudo();
      //  SpawnInimigos();
        if (Input.GetKeyDown(KeyCode.L)) // mostra seus pontos
        {
            Debug.Log($"Seus pontos sao: {Pontos}");
        }

        if (Player.PlayerVivo == false)
        {
            CancelInvoke();
        }
    }

    void EncontrarSpawnPoint() // Acha o SpawnPoint Do inimigo
    {
        if (SpawnPoint == null)
        {
            SpawnPoint = GameObject.FindWithTag("SpawnPoint");

            if (SpawnPoint != null)
            {
                DontDestroyOnLoad(SpawnPoint);
            }
        }
    }

    void Zawarudo() //pausa o tempo
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = tempo;
        }
    }

    public void AlterarPontos(int pontos)
    {
        Pontos += pontos;
    }

    void SpawnInimigos ()
    {
        EncontrarSpawnPoint(); // garante que o SpawnPoint está definido

        if (SpawnPoint == null) return;

        SpawnTimer += Time.deltaTime;
        if (SpawnTimer > SpawnInterval)
        {
            SpawnPosition = new Vector3(
                SpawnPoint.transform.position.x,
                SpawnPoint.transform.position.y,
                Random.Range(-12, 5)
                );
            Instantiate(inimigoPrefab, SpawnPosition, SpawnPoint.transform.rotation);
            SpawnTimer = 0f;
        }
    }

    /*public void SpawnWave(float raioSpawnSeguro) SISTEMA ANTIGO DE WAVE
    {
        EncontrarSpawnPoint(); // garante que o SpawnPoint está definido

        raioSpawnSeguro = 10f;

        if (SpawnPoint == null)
        {
            Debug.LogWarning("SpawnPoint não definido. Não é possível spawnar a wave.");
            return;
        }

        for (int i = 0; i < Wave.Length; i++)
        {
            Vector3 posicao;
            bool posValida;

            int tentativas = 0;
            do
            {
                posicao = new Vector3(
                    SpawnPoint.transform.position.x,
                    SpawnPoint.transform.position.y,
                    Random.Range(-12, 5)
                );

                // verifica se tem algum inimigo nessa área
                Collider[] colisores = Physics.OverlapSphere(posicao, raioSpawnSeguro);
                posValida = colisores.Length == 0;

                tentativas++;
                if (tentativas > 10) break; // evita loop infinito
            }
            while (!posValida);

            if (posValida)
                Instantiate(Wave[i], posicao, SpawnPoint.transform.rotation);

        }*/







}