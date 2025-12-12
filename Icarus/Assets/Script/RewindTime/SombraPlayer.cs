using UnityEngine;

public class SombraPlayer : MonoBehaviour
{
    public float tempovida = 4f;
    private void Start()
    {
        Destroy(this.gameObject, tempovida);
    }
    public void Destruir()
    {
        Destroy(this.gameObject);
    }

   
}
