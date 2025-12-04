using UnityEngine;

public class Rotation3d : MonoBehaviour
{
    [Tooltip("Rota��o (Graus/Segundo")]
    public float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed,0f  * Time.deltaTime);
    }
}
