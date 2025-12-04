using UnityEngine;

public class LoopObjects : MonoBehaviour
{
    public float speed = 1f;
    public float resetX = -12f; // posi��o quando o objeto some
    public float startX = 12f; // posi��o para reaparecer

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < resetX)
        {
            Vector3 pos = transform.position;
            pos.x = startX;
            transform.position = pos;
        }
    }
}
