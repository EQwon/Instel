using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private float delay;
    [SerializeField] private GameObject speechPanel;
    [SerializeField] private GameObject speechBox;
    [SerializeField] private GameObject nameBox;
    [SerializeField] private Text speakerName;
    [SerializeField] private Text speech;
    [SerializeField] private GameObject canGoNextObject;

    [Header("Character")]
    [SerializeField] private List<GameObject> characterImages;

    [Header("Choice")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private List<Text> choiceTexts;

    private TextHolder tHolder;
    private CharacterHolder cHolder;
    private List<List<string>> nowDialogue;
    private bool canGoNext = false;
    private int nowCnt;

    private void Awake()
    {
        tHolder = GetComponent<TextHolder>();
        cHolder = GetComponent<CharacterHolder>();
        choicePanel.SetActive(false);
    }

    private void Start()
    {
        nowDialogue = tHolder.GetDialogue(0);
        nowCnt = 0;
        ShowText();
    }

    private void Update()
    {
        if (canGoNext) canGoNextObject.SetActive(true);
        else canGoNextObject.SetActive(false);
    }

    private void ShowText()
    {
        List<string> dialogue = nowDialogue[nowCnt];

        switch (dialogue[0])
        {
            case "narration":
                canGoNext = false;
                nameBox.SetActive(false);
                speech.color = Color.white;
                StartCoroutine(PrintText(dialogue[1], dialogue[2]));
                break;
            case "talk":
                canGoNext = false;
                nameBox.SetActive(true);
                speech.color = Color.white;
                StartCoroutine(PrintText(dialogue[1], dialogue[2]));
                break;
            case "think":
                canGoNext = false;
                nameBox.SetActive(true);
                speech.color = Color.grey;
                StartCoroutine(PrintText(dialogue[1], dialogue[2]));
                break;
            case "image":
                if (dialogue[1] == "None")
                {
                    for (int i = 0; i < characterImages.Count; i++) characterImages[i].SetActive(false);
                }
                else
                {
                    characterImages[0].SetActive(true);
                    characterImages[0].GetComponent<Image>().sprite = cHolder.GetInfo(dialogue[1]).sprite;
                    if (dialogue.Count == 3)
                    {
                        characterImages[1].SetActive(true);
                        characterImages[1].GetComponent<Image>().sprite = cHolder.GetInfo(dialogue[2]).sprite;
                    }
                }
                NextText();
                break;
            case "choice":
                List<string> choices = new List<string>();

                for (int i = 0; i < choiceTexts.Count; i++) choices.Add(dialogue[i + 1]);

                ShowChoicePanel(choices);
                break;
            case "result":
                break;
        }        
    }

    public void NextText()
    {
        if (canGoNext == false) return;

        nowCnt += 1;
        if (nowCnt >= nowDialogue.Count) return;

        ShowText();
    }

    public void ShowChoicePanel(List<string> choices)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceTexts.Count; i++)
        {
            choiceTexts[i].text = choices[i];
        }
    }

    public void SelectChoice(int num)
    {
        choicePanel.SetActive(false);

        nowDialogue = tHolder.GetDialogue(int.Parse(nowDialogue[nowCnt + 1][num]));
        nowCnt = 0;
        ShowText();
    }

    private IEnumerator PrintText(string name, string text)
    {
        speechBox.GetComponent<Image>().color = cHolder.GetInfo(name).color;
        nameBox.GetComponent<Image>().color = cHolder.GetInfo(name).color;
        speakerName.text = name;
        speech.text = text;

        yield return new WaitForSeconds(delay);

        canGoNext = true;
    }
}
