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

        chronospontos = 25f;
    }
    private void Start()
    {
        Pontos = 0;
    }

  
    void Update()
    {
        Zawarudo();
        ChronosPontos();
      
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            Debug.Log($"Seus pontos sao: {Pontos}");
        }

        if (Player.PlayerVivo == false)
        {
            CancelInvoke();
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

 
    }





