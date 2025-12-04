using UnityEngine;

public class EventObjectsMovement : MonoBehaviour
{
    public float speed = 1f;
    public float destroyX = -15f;

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}