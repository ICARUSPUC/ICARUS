using UnityEngine;

public class Rotation3dZ : MonoBehaviour
{
    [Tooltip("Rota��o (Graus/Segundo")]
    public float rotationSpeed = 30f;

    private void Start()
    {
   
    }
    void Update()
    {
        transform.Rotate(0f,0f ,rotationSpeed  * Time.deltaTime);
    }
}
