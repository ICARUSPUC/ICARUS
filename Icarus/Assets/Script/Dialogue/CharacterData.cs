using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Dialogue/Character")]

public class CharacterData : ScriptableObject
{
    public string characterName;
    public Color nameColor = Color.white;
    public Sprite portrait;
}
