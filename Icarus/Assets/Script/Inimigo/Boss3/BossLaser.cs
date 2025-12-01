using UnityEngine;
using System.Collections;

public class BossLaser : MonoBehaviour
{
   
    [Header("Audio")]
    public AudioSource warningaudioSource;
    public AudioSource shootaudioSource;

    [Header("Configuração do Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Transform spawnLaser;

    [SerializeField] float tempoCrescimento = 0.5f; 
    [SerializeField] float tempoAlerta = 1f;
    [SerializeField] float tempoLaserAtivo = 2f;

    [SerializeField] float tamanhoCarregamento = 0.3f;
    [SerializeField] float comprimentoMaximo = 50f;

    private GameObject laserAtual;

    public IEnumerator Atacar()
    {
        // --- FASE 1: INSTANCIA e CARREGAMENTO ---
        GameObject laser = Instantiate(laserPrefab, spawnLaser.position, spawnLaser.rotation);
        laserAtual = laser;
        laser.transform.SetParent(spawnLaser);

        laser.transform.localScale = Vector3.zero;
        laser.GetComponent<Collider>().enabled = false;

        yield return CrescerFase1(laser.transform, tempoCrescimento);

        // --- FASE 2: ALERTA ---
        if (warningaudioSource) warningaudioSource.Play();
        yield return new WaitForSeconds(tempoAlerta);

        // --- FASE 3: LASER CRESCE E ATIVA ---
        if (shootaudioSource) shootaudioSource.Play();
        laser.GetComponent<Collider>().enabled = true;

        yield return CrescerFase2(laser.transform, tempoCrescimento, comprimentoMaximo);

        // --- FASE 4: LASER ATIVO ---
        yield return new WaitForSeconds(tempoLaserAtivo);

        if (laser != null)
            Destroy(laser);
    }

    IEnumerator CrescerFase1(Transform t, float duracao)
    {
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.one * tamanhoCarregamento;

        float tempo = 0;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            t.localScale = Vector3.Lerp(start, end, tempo / duracao);
            yield return null;
        }
    }

    IEnumerator CrescerFase2(Transform t, float duracao, float max)
    {
        float tempo = 0;

        Vector3 start = t.localScale;
        Vector3 end = new Vector3(max * 2f, start.y, start.z);

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float x = Mathf.Lerp(start.x, end.x, tempo / duracao);

            t.localScale = new Vector3(x, start.y, start.z);

            float offset = (x - start.x) * -0.5f;
            t.localPosition = new Vector3(offset, t.localPosition.y, t.localPosition.z);

            yield return null;
        }
    }

    public void CancelarLaser()
    {
        if (laserAtual != null)
            Destroy(laserAtual);
    }
}
