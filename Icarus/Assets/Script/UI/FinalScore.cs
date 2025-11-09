using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    [SerializeField] TMP_Text scoreNumberText;

    private void OnEnable()
    {
        if (scoreNumberText != null)
            scoreNumberText.text = GameManager.Mestre.Pontos.ToString();
    }
}
