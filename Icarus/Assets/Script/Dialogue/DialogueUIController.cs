using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text lineText;

    [Header("Typing Settings")]
    public float typingSpeed = 0.02f;

    private Coroutine typingCoroutine;

    private void Awake()
    {
        ShowDialoguePanel(false);
    }


   
    public void ShowDialoguePanel(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void DisplayLine(DialogueLine line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        portraitImage.sprite = line.speaker.portrait;
        nameText.text = line.speaker.characterName;
        nameText.color = line.speaker.nameColor;

        typingCoroutine = StartCoroutine(TypeText(line.line));
    }

    private IEnumerator TypeText(string text)
    {
        lineText.text = "";
        foreach (char c in text)
        {
            lineText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    public void OnClickNext()
    {
        DialogueManager.Instance.NextLine();
    }
}