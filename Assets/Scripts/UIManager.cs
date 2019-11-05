using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text speakerName;
    [SerializeField] private Text speech;

    private TextHolder holder;
    private List<List<string>> nowDialogue;
    private int nowCnt = 0;

    private void Awake()
    {
        holder = GetComponent<TextHolder>();
    }

    private void Start()
    {
        nowDialogue = holder.GetDialogue();
        for (int i = 0; i < nowDialogue.Count; i++)
        {
            for (int j = 0; j < nowDialogue[i].Count; j++)
            {
                Debug.Log(i + ", " + j + " = " + nowDialogue[i][j]);
            }
        }
        
        ShowText();
    }

    private void ShowText()
    {
        speakerName.text = nowDialogue[nowCnt][0];
        speech.text = nowDialogue[nowCnt][1];
    }

    public void NextText()
    {
        nowCnt += 1;
        if (nowCnt >= nowDialogue.Count) return;

        ShowText();
    }
}
