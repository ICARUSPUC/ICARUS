using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class Inimigo : MonoBehaviour

{

    [SerializeField] GameObject EnemyShot; //Prefab do tiro do Inimigo
    [SerializeField] GameObject SpawnEnemy; //Spawn do Tiro do Inimigo
    [SerializeField] float ShotFrequency = 10f; // Quão rapido ele atira
    [SerializeField] float MoveTimer = 0;
    [SerializeField] float speedInimigo;
    [SerializeField] float timerMove = 0f;
    [SerializeField] float tempoMovimento = 0f;

        [Header("Status")]
    [SerializeField] private float vidaMax = 3f; // Vida máxima
    private float vidaAtual;

    bool movendo = true;
    float InimigoFireTimer = 1;
    public GameManager GameManager; //Fala quem é o GameManager Pra esse Script
    private Vector3 moveEnemy; //Variavel pra mover o inimigo
    private Rigidbody rbEnemy; //Variavel Pro rigidBody do Inimigo

    [SerializeField] private Renderer[] renderers; // arraste aqui os meshes do inimigo
[SerializeField] private Color damageColor = Color.red;
[SerializeField] private float flashDuration = 0.1f;

private Color[] originalColors;


void Awake()
{
    // Guarda as cores originais dos materiais
    if (renderers != null && renderers.Length > 0)
    {
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                originalColors[i] = renderers[i].material.color;
        }
    }
}


    void Start()
    {
        InvokeRepeating("Atirar", InimigoFireTimer, ShotFrequency); //Atira depois de tanto tempo depois repete
        rbEnemy = GetComponent<Rigidbody>();
         vidaAtual = vidaMax;

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

    public void LevarDano(float dano)
{
    vidaAtual -= dano;
    StartCoroutine(DanoVisual());
    Debug.Log($"{name} levou {dano} de dano. Vida atual: {vidaAtual}/{vidaMax}");
    if (vidaAtual <= 0f) Morrer();
}

IEnumerator DanoVisual()
{
    // muda a cor
    foreach (var r in renderers)
    {
        if (r.material.HasProperty("_Color"))
            r.material.color = damageColor;
    }

    yield return new WaitForSeconds(flashDuration);

    // volta à cor original
    for (int i = 0; i < renderers.Length; i++)
    {
        if (renderers[i].material.HasProperty("_Color"))
            renderers[i].material.color = originalColors[i];
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


