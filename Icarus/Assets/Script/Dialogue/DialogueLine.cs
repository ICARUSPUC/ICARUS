using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public CharacterData speaker;
    [TextArea(2, 5)] public string line;
}
