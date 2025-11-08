using UnityEngine;
using TMPro;
public class YourScore : MonoBehaviour
{
    public TMP_Text Showpontos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void FixedUpdate()
    {
        Showpontos.text = GameManager.Mestre.Pontos.ToString();
    }
}
