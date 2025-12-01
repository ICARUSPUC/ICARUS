using UnityEngine;

public class GarraAnimacao : MonoBehaviour
{
      public Transform garra;
    public float duracao = 3f;
    public float giro = 180f;
    public float descida = 2f;

    private float tempo = 0f;
    private bool animando = false;

    void Update()
    {
     
        if (animando)
        {
            tempo += Time.deltaTime;
            float t = Mathf.Clamp01(tempo / duracao);

     
            float y = Mathf.Lerp(0, giro, t);
            garra.localEulerAngles = new Vector3(garra.localEulerAngles.x, y, garra.localEulerAngles.z);

            
            if (t > 0.5f)
            {
                float tDescer = (t - 0.5f) * 2f; 
                float yPos = Mathf.Lerp(0, -descida, tDescer);
                garra.localPosition = new Vector3(garra.localPosition.x, yPos, garra.localPosition.z);
            }

            if (t >= 1f)
            {
                animando = false; 
            }
        }
    }

    public void AnimarGarra()
    {
        tempo = 0f;
        animando = true;
    }
}
