using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneManger : MonoBehaviour
{

    [Header("Fade Config")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeedOut = 1.5f;
    [SerializeField] private float fadeSpeedIn = 1.5f;
    private bool isFading = false;
    
    private const string ULTIMA_CENA_KEY = "UltimaCena"; // chave usada no PlayerPrefs

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void Start()
    {
        GameManager.Mestre.SceneManger = this;
        if (fadeImage != null)
         {
            fadeImage.color = Color.black;
            StartCoroutine(FadeIn());
        }
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
            Debug.Log($"�ltima cena de jogo salva: {cenaNova.name}");
        }
        else
        {
            Debug.Log($"Cena '{cenaNova.name}' ignorada (n�o � cena de jogo).");
        }
    }

    private bool EhCenaDeJogo(string nomeCena)
    {
        // S� considera cenas de fases como cenas de jogo
        return nomeCena.StartsWith("Fase");
    }

    // Iniciar o jogo (usado no menu principal)
    public void StartGame()
    {
        SceneManager.LoadScene("Fase1");
    }

    // Bot�o da tela de derrota para voltar � �ltima fase jogada
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

    // Bot�o para voltar ao menu
    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Bot�o "Pr�xima Fase" na tela Victory
    public void ProximaFase()
    {
        if (!PlayerPrefs.HasKey(ULTIMA_CENA_KEY))
        {
            Debug.LogWarning("Nenhuma fase anterior encontrada. Carregando Fase1 por padr�o.");
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
                Debug.LogWarning($"A fase {ultimaFase} n�o possui pr�xima fase definida.");
                return;
        }

        if (!string.IsNullOrEmpty(proximaFase))
        {
            SceneManager.LoadScene(proximaFase);
        }
    }
 public void LoadScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }
   IEnumerator FadeAndLoad(string sceneName)
    {
        isFading = true;

        // FADE OUT (escurecendo)
        for (float i = 0; i <= 1; i += Time.deltaTime * fadeSpeedOut)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }

        // Espera um pouquinho pra suavizar
        yield return new WaitForSeconds(0.2f);

        // Carrega a nova cena (mantendo tudo preto)
        SceneManager.LoadScene(sceneName);
    }
  IEnumerator FadeIn()
    {
        isFading = true;

        // FADE IN (clareando)
        for (float i = 1; i >= 0; i -= Time.deltaTime * fadeSpeedIn)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        isFading = false;
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