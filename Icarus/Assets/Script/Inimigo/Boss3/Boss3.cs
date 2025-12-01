using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Boss3 : MonoBehaviour
{

 [Header("Atributos do Boss")]
    [SerializeField] float vidaMax = 70f;
    float vidaAtual;

    [Header("Efeitos opcionais")]
    [SerializeField] GameObject efeitoMorte; // arraste um efeito se quiser

    [Header("Feedback visual de dano")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Rigidbody rb;
    private Color[] originalColors;

    public Boss3Controller controller;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(renderers[i].material);

                if (renderers[i].material.HasProperty("_BaseColor"))
                    originalColors[i] = renderers[i].material.GetColor("_BaseColor");
                else if (renderers[i].material.HasProperty("_Color"))
                    originalColors[i] = renderers[i].material.color;
            }
        }
    }
    void Start()
    {
        vidaAtual = vidaMax;
    }

    public float GetVidaPercentual()
{
    return vidaAtual / vidaMax;
}

     public void TomarDano(float dano)
    {
        vidaAtual -= dano;
        StartCoroutine(DanoVisual());

        if (vidaAtual <= 0)
        {
            Morrer();
        }
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

    void Morrer()
    {
        // 1. Lógica de Vitória
        // Usando o operador ?. (null-conditional) para garantir que não haverá erro
        // se o GameManager.Mestre não estiver inicializado.
        GameManager.Mestre?.SceneManger.Ganhar();

        // 2. Efeito Visual
        if (efeitoMorte != null)
        {
            Instantiate(efeitoMorte, transform.position, Quaternion.identity);
        }

        // 3. Destruição do Objeto
        // Garante que o objeto pai (raiz) do boss seja destruído.
        Destroy(transform.root.gameObject);
    }
}
