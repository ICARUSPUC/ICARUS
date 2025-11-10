using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    private const string ULTIMA_CENA_KEY = "UltimaCena"; // chave usada no PlayerPrefs

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void Start()
    {
        GameManager.Mestre.SceneManger = this;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene cenaAntiga, Scene cenaNova)
    {
        if (EhCenaDeJogo(cenaNova.name))
        {
            PlayerPrefs.SetString(ULTIMA_CENA_KEY, cenaNova.name);
            PlayerPrefs.Save();
            Debug.Log($"Última cena de jogo salva: {cenaNova.name}");
        }
        else
        {
            Debug.Log($"Cena '{cenaNova.name}' ignorada (não é cena de jogo).");
        }
    }

    private bool EhCenaDeJogo(string nomeCena)
    {
        // Só considera cenas de fases como cenas de jogo
        return nomeCena.StartsWith("Fase");
    }

    // Iniciar o jogo (usado no menu principal)
    public void StartGame()
    {
        SceneManager.LoadScene("Fase1");
    }

    // Botão da tela de derrota para voltar à última fase jogada
    public void VoltarUltimaCena()
    {
        if (PlayerPrefs.HasKey(ULTIMA_CENA_KEY))
        {
            string ultimaCena = PlayerPrefs.GetString(ULTIMA_CENA_KEY);
            SceneManager.LoadScene(ultimaCena);
        }
        else
        {
            SceneManager.LoadScene("Fase1");
        }
    }

    // Botão para voltar ao menu
    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Botão "Próxima Fase" na tela Victory
    public void ProximaFase()
    {
        if (!PlayerPrefs.HasKey(ULTIMA_CENA_KEY))
        {
            Debug.LogWarning("Nenhuma fase anterior encontrada. Carregando Fase1 por padrão.");
            SceneManager.LoadScene("Fase1");
            return;
        }

        string ultimaFase = PlayerPrefs.GetString(ULTIMA_CENA_KEY);
        string proximaFase = "";

        switch (ultimaFase)
        {
            case "Fase1":
                proximaFase = "Fase2";
                break;

            case "Fase2":
                proximaFase = "Fase3";
                break;

            case "Fase3":
                proximaFase = "Victory"; // ou talvez volte ao menu aqui
                break;

            default:
                Debug.LogWarning($"A fase {ultimaFase} não possui próxima fase definida.");
                return;
        }

        if (!string.IsNullOrEmpty(proximaFase))
        {
            SceneManager.LoadScene(proximaFase);
        }
    }

    private void Update()
    {
       // Ganhar();
    }

    public void Ganhar()
    {
        if (GameManager.Mestre == null) return;
        
        
            SceneManager.LoadScene("Victory");
            GameManager.Mestre.Pontos = 0;
        
    }
}