using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHolder : MonoBehaviour
{
    [SerializeField] private TextAsset text;

    public List<List<string>> GetDialogue()
    {
        return Parser.DialogParse(text);
    }
}
