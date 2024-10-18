using UnityEngine;

public enum CharacterCategory
{
    Monsters,
    Aliens,
    Animals,
    Robots
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/New Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int baseCost;
    public bool isUnlocked;

    // Character visual representation
    public Sprite characterSprite;

    public CharacterCategory category;
    public int index;
}

