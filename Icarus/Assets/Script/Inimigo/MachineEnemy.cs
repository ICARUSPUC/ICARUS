using Unity.Collections;
using UnityEngine;
using System.Collections;

public class MachineEnemy : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject tiroPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer = 0;

    [Header("Animação")]
    public Animator anim;
    private bool girando = false;

    [Header("Status")]
    [SerializeField] private float vidaMax = 5f;
    private float vidaAtual;

    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color[] originalColors;

    private Rigidbody rbEnemy;
    [SerializeField] private float speedInimigo = 5f;


    void Start()
    {
        rbEnemy = GetComponent<Rigidbody>();
        vidaAtual = vidaMax;

        // Salva as cores originais dos materiais
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                originalColors[i] = renderers[i].material.color;
        }
    }

    void Update()
    {
        // Animação controla o tiro
        girando = anim.GetBool("Girando");

        if (girando)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Disparar();
                fireTimer = 0;
            }
        }
    }

    void FixedUpdate()
    {
        MovimentacaoInimigo();
    }

    void Disparar()
    {
        Instantiate(tiroPrefab, firePoint.position, firePoint.rotation);
    }

    public void LevarDano(float dano)
    {
        vidaAtual -= dano;

        StartCoroutine(DanoVisual());

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    IEnumerator DanoVisual()
    {
        // aplica a cor de dano
        foreach (Renderer r in renderers)
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

    void MovimentacaoInimigo()
    {
        Vector3 movimento = Vector3.left * speedInimigo * Time.fixedDeltaTime;

        Vector3 novaPos = rbEnemy.position + movimento;

        // limites
        novaPos.z = Mathf.Clamp(novaPos.z, -13.5f, 6f);
        novaPos.x = Mathf.Clamp(novaPos.x, -22f, 22f);

        rbEnemy.MovePosition(novaPos);
    }

    public void Morrer()
    {
        gameObject.SetActive(false);
        Invoke(nameof(Destruir), 6f);
    }

    void Destruir()
    {
        Destroy(gameObject);
    }
}
