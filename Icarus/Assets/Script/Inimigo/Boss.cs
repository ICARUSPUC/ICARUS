using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Boss : MonoBehaviour
{
    // =========================================================================
    // ⚙️ Atributos do Boss 
    // =========================================================================
    [Header("Atributos do Boss")]
    [SerializeField] float vidaMax = 70f;

    [Header("Efeitos opcionais")]
    [SerializeField] GameObject efeitoMorte; // arraste um efeito se quiser

    [Header("Feedback visual de dano")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;


    [Header("Musica")]
    [SerializeField] private AudioSource MusicaBoss;
    [SerializeField] private GameObject MusicaMain;
    [SerializeField] private AudioSource EfeitoVhs;

    public bool BossinScene = false;
    private Rigidbody rb;
    private Color[] originalColors;

    // =========================================================================
    // 🔄 Métodos Padrão do Unity
    // =========================================================================
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(renderers[i].material);

                if (renderers[i].material.HasProperty("_TintColor"))
                    originalColors[i] = renderers[i].material.GetColor("_TintColor");
                else if (renderers[i].material.HasProperty("_Color"))
                    originalColors[i] = renderers[i].material.color;
            }
        }

        BossinScene = true;
    }
    void Start()
    {
        
        EfeitoVhs.Play();
        MusicaBoss.Play();
    }

    // =========================================================================
    // 💥 Dano e Morte
    // =========================================================================

    public void TomarDano(float dano)
    {
        vidaMax -= dano;
        StartCoroutine(DanoVisual());

        if (vidaMax <= 0)
        {
            Morrer();
        }
    }

    IEnumerator DanoVisual()
    {
        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_TintColor"))
                r.material.SetColor("_TintColor", damageColor);
            else if (r.material.HasProperty("_Color"))
                r.material.color = damageColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_TintColor"))
                renderers[i].material.SetColor("_TintColor", originalColors[i]);
            else if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }

    void Morrer()
    {
        InimigoSpawnSequence.AddWavePoints();
        

        // 2. Efeito Visual
        if (efeitoMorte != null)
        {
            Instantiate(efeitoMorte, transform.position, Quaternion.identity);
        }

        // 3. Destruição do Objeto
        // Garante que o objeto pai (raiz) do boss seja destruído.
        Destroy(transform.root.gameObject);

        GameManager.Mestre?.SceneManger.Ganhar();
    }
}
