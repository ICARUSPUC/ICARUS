using UnityEngine;
using UnityEngine.SceneManagement;

public class win : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           SceneManager.LoadScene("Victory");
        }
    }
}
