using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum CharacterName { 경찰, 나, 나레이션, 대리, 사수, 앵커, 여동생, 예진, 의사 }

[System.Serializable]
public class CharacterInfo
{
    public CharacterName name;
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
    [SerializeField] private List<CharacterInfo> characters;

    public CharacterInfo GetInfo(string name)
    {
        int targetNum = (int)(CharacterName)System.Enum.Parse(typeof(CharacterName), name);

        return characters[targetNum];
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CharacterInfo))]
public class CharacterDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width * 0.2f, position.height), property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + position.width * 0.2f, position.y, position.width * 0.4f, position.height), property.FindPropertyRelative("sprite"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + position.width * 0.6f, position.y, position.width * 0.4f, position.height), property.FindPropertyRelative("color"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif