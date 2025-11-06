using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

public class Inimigo : MonoBehaviour
{

    [SerializeField] GameObject EnemyShot; //Prefab do tiro do Inimigo
    [SerializeField] GameObject SpawnEnemy; //Spawn do Tiro do Inimigo
    [SerializeField] float ShotFrequency = 10f; // Quão rapido ele atira
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
    [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;
    bool movendo = true;
    float InimigoFireTimer = 1;
    public GameManager GameManager; //Fala quem é o GameManager Pra esse Script
    private Vector3 moveEnemy; //Variavel pra mover o inimigo
    private Rigidbody rbEnemy; //Variavel Pro rigidBody do Inimigo


    void Start()
    {
        InvokeRepeating("Atirar", InimigoFireTimer, ShotFrequency); //Atira depois de tanto tempo depois repete
        rbEnemy = GetComponent<Rigidbody>();

    }

    private void Update()
    {


        {
            if (movendo)
            {
                MovimentacaoInimigo();
                timerMove += Time.fixedDeltaTime;

                if (timerMove >= tempoMovimento)
                {
                    movendo = false; // para o inimigo
                }
            }
        }

    }



    void Atirar()
    {
        if (GetComponent<TimeBody>().isrewinding == true)
        {
            return;
        }
        else
        {
            GameObject tiro = Instantiate(EnemyShot, SpawnEnemy.transform.position, SpawnEnemy.transform.rotation);
        }
    }

    void Destruir() //Apaga o inimigo da cena
    {
        Destroy(gameObject);
    }
    public void Morrer() // Desativa e depois de um tempo deleta o inimigo
    {

        GameManager.Mestre.AlterarPontos(50);
        CancelInvoke();
        gameObject.SetActive(false);
        Invoke("Destruir", 6f);
    }

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;
        Vector3 Limite = rbEnemy.position + movimento;
        Limite.z = Mathf.Clamp(Limite.z, -13.5f, 6f);
        Limite.x = Mathf.Clamp(Limite.x, -22f, 22f);
        rbEnemy.MovePosition(Limite);
    }
}    


