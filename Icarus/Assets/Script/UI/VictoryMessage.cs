using UnityEngine;
using TMPro;

public class VictoryMessage : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;

    [Tooltip("Mensagens possíveis desta fase")]
    [SerializeField] string[] victoryMessages;

    void OnEnable()
    {
        ShowRandomMessage();
    }

    public void ShowRandomMessage()
    {
        if (victoryMessages == null || victoryMessages.Length == 0)
        {
            messageText.text = "";
            return;
        }

        int index = Random.Range(0, victoryMessages.Length);
        messageText.text = victoryMessages[index];
    }
}