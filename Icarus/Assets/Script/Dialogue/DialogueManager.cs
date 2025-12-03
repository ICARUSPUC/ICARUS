using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private DialogueSequence currentSequence;
    private int currentIndex = 0;

    private DialogueUIController uiController;
    private bool isActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);


        uiController = Object.FindFirstObjectByType<DialogueUIController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0)))
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueSequence sequence)
    {
        Time.timeScale = 0.01f;
        currentSequence = sequence;
        currentIndex = 0;
        isActive = true;
        uiController.ShowDialoguePanel(true);

        ShowCurrentLine();
    }

    public void NextLine()
    {
      //  Debug.Log("Avan�ando linha...");

        if (!isActive) return;

        currentIndex++;

        if (currentIndex >= currentSequence.lines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentSequence.lines[currentIndex];
        uiController.DisplayLine(line);
    }

    private void EndDialogue()
    {
        Time.timeScale = 1f;
        isActive = false;
        uiController.ShowDialoguePanel(false);
        currentSequence = null;

      //  Debug.Log("Di�logo finalizado e painel desativado!");

    }

    public bool IsDialogueActive()
    {
        return isActive;
    }
}
