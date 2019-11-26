using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterName { 나, 대리, 여동생, 경찰, 예진 }

public class CharacterInfo
{
    public Sprite sprite;
    public Color color;

    public CharacterInfo(Sprite sprite, Color color)
    {
        this.sprite = sprite;
        this.color = color;
    }
}

public class CharacterHolder : MonoBehaviour
{
    [SerializeField] private List<Sprite> characterSprites;
    [SerializeField] private List<Color> characterColors;

    public CharacterInfo GetInfo(string name)
    {
        int targetNum = (int)(CharacterName)System.Enum.Parse(typeof(CharacterName), name);

        return new CharacterInfo(characterSprites[targetNum], characterColors[targetNum]);
    }
}