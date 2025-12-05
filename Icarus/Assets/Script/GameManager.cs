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
    public float chronospontos = 25f;

    [SerializeField] GameObject inimigoPrefab;
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject[] Wave;
    [SerializeField] float SpawnInterval = 5;
    Vector3 SpawnPosition;
    [SerializeField] TimeManager TimeManager;

    //public SceneManager SceneManager;
    public SceneManger SceneManger;


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
    void ChronosPontos()
    {
        if (chronospontos >= 100)
        {
            Player.Chronos = true;
        }
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
        chronospontos = 25f;
    }
    private void Start()
    {
        //InvokeRepeating("SpawnWave", 1f, 8f); //invoca as waves repetidamente SiSTEMA ANTIGO DE WAVE
        Pontos = 0;
    }

  
    void Update()
    {
        Zawarudo();
        ChronosPontos();
      
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
            TimeManager.BulletTime();
        }
    }


    public void AlterarPontos(int pontos)
    {
        Pontos += pontos;
    }

    public void AlterarChronosPontos(float pontos)
    {
        chronospontos += pontos;

        chronospontos = Mathf.Clamp(chronospontos, 0f, 100f);
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





}