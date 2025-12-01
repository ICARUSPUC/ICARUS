using UnityEngine;
using System.Collections;

public class Boss3Controller : MonoBehaviour
{
    public Boss3 boss;

    [Header("Tentáculos")]
    public BossTentaculos[] tentaculosChao;
    public BossTentaculos[] tentaculosParede;
    public BossTentaculos[] tentaculosTeto;

    [Header("Laser")]
    public BossLaser laser;

    [Header("Config")]
    public float tempoEntreAtaques = 2f;

    bool atacando = false;

    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempoEntreAtaques);

            if (!atacando)
                yield return EscolherAtaque();
        }
    }

    IEnumerator EscolherAtaque()
    {
        atacando = true;

        bool fase2 = boss.GetVidaPercentual() < 0.5f;

        if (!fase2)
        {
            // fase 1
            int atk = Random.Range(0, 2);

            if (atk == 0) yield return AtaqueChao();
            else yield return AtaqueParede();
        }
        else
        {
            // fase 2
            int atk = Random.Range(0, 3);

            if (atk == 0) yield return AtaqueChao();
            else if (atk == 1) yield return AtaqueParede();
            else yield return AtaqueLaser();
        }

        atacando = false;
    }

    IEnumerator AtaqueChao()
    {
        var t = tentaculosChao[Random.Range(0, tentaculosChao.Length)];
        yield return t.Atacar();
    }

    IEnumerator AtaqueParede()
    {
        var t = tentaculosParede[Random.Range(0, tentaculosParede.Length)];
        yield return t.Atacar();
    }

    IEnumerator AtaqueLaser()
    {
        yield return laser.Atacar();
    }
}
