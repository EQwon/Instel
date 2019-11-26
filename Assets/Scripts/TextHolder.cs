using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHolder : MonoBehaviour
{
    [SerializeField] private List<TextAsset> texts;

    public List<List<string>> GetDialogue(int num)
    {
        return Parser.DialogParse(texts[num]);
    }
}
