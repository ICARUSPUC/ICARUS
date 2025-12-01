using UnityEngine;
using System.Collections;

public class BossTentaculos : MonoBehaviour
{
    [Header("Referência do Modelo")]
    public Transform modelo;

    [Header("Animação")]
    public float tempoSubir = 0.4f;
    public float tempoAtivo = 0.6f;
    public float tempoDescer = 0.4f;
    public float anguloAtivo = 180f;

    public Animator animator;

    private bool animando = false;


    public IEnumerator Atacar()
    {
        if (animando) yield break;
        animando = true;


         yield return Rotacionar(0, anguloAtivo, tempoSubir);

       
    yield return new WaitForSeconds(tempoAtivo);

    

     animando = false;
    }

    IEnumerator Rotacionar(float inicio, float fim, float duracao)
    {
        float t = 0;
        while (t < duracao)
        {
            t += Time.deltaTime;
            float angulo = Mathf.Lerp(inicio, fim, t / duracao);
            modelo.localRotation = Quaternion.Euler(angulo, 0, 0);
            yield return null;
        }
    }
}