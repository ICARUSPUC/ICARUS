using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Sequence")]
    public DialogueSequence dialogue;

    [Header("Debug / Test Settings")]
    public bool triggerOnStart = false; // se quiser começar assim que o jogo iniciar
    public bool triggerWithKey = true;  // ativa o teste com tecla
    public KeyCode triggerKey = KeyCode.T; // tecla para teste

    private bool playerNearby = false;

    void Start()
    {
        // inicia automaticamente se quiser
        Invoke("DialogoStart", 1f);
    }


    void DialogoStart ()
    {
         if (triggerOnStart && dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
    void Update()
    {
        // modo de teste: apertar tecla a qualquer momento
        if (triggerWithKey && Input.GetKeyDown(triggerKey))
        {
            TriggerDialogue();
        }

        // se quiser manter o modo "pressione E perto do NPC"
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        if (dialogue == null)
        {
            Debug.LogWarning("Nenhum DialogueSequence atribuído ao DialogueTrigger!");
            return;
        }

        if (!DialogueManager.Instance.IsDialogueActive())
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }

    // opcional: modo "entrar na área"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}