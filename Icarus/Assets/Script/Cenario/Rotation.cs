using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Tooltip("Rota��o (Graus/Segundo")]
    public float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
