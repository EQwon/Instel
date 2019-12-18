using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Image backgroundImage;

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

    [Header("Object")]
    [SerializeField] private GameObject objectPanel;
    [SerializeField] private Image objectImage;

    [Header("Choice")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choicePrefab;

    [Header("Change")]
    [SerializeField] private GameObject dayStartPanel;
    [SerializeField] private Text dayText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private GameObject sceneChangePanel;

    [Header("Info")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject alram;
    private bool isOn = false;
    [SerializeField] private GameObject contents;

    private TextHolder tHolder;
    private CharacterHolder cHolder;
    private List<List<string>> nowDialogue;
    int nowDialogueNum = 33;
    private bool canGoNext = true;
    private int nowCnt;

    private void Awake()
    {
        tHolder = GetComponent<TextHolder>();
        cHolder = GetComponent<CharacterHolder>();
        choicePanel.SetActive(false);
        objectPanel.SetActive(false);
        dayStartPanel.SetActive(false);
        sceneChangePanel.SetActive(false);

        alram.SetActive(false);
    }

    private void Start()
    {
        nowDialogue = tHolder.GetDialogue(nowDialogueNum);
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
            case "background":
                string path = "Background/" + dialogue[2];
                Sprite bg = Resources.Load<Sprite>(path);
                backgroundImage.sprite = bg;
                NextText();
                break;
            case "narration":
                canGoNext = false;
                nameBox.SetActive(false);
                speech.color = Color.white;
                speech.fontStyle = FontStyle.Bold;
                StartCoroutine(PrintText(dialogue[1], dialogue[2]));
                break;
            case "talk":
                canGoNext = false;
                nameBox.SetActive(true);
                speech.color = Color.white;
                speech.fontStyle = FontStyle.Normal;
                StartCoroutine(PrintText(dialogue[1], dialogue[2]));
                break;
            case "think":
                canGoNext = false;
                nameBox.SetActive(true);
                speech.color = Color.grey;
                speech.fontStyle = FontStyle.Italic;
                StartCoroutine(PrintText(dialogue[1], dialogue[2]));
                break;
            case "image":
                for (int i = 0; i < characterImages.Count; i++) characterImages[i].SetActive(false);

                if (dialogue[1] != "")
                {
                    characterImages[0].SetActive(true);
                    characterImages[0].GetComponent<Image>().sprite = cHolder.GetInfo(dialogue[1]).sprite;
                    ImageSizeFitter(characterImages[0].GetComponent<Image>());
                    if (dialogue[2] != "")
                    {
                        characterImages[1].SetActive(true);
                        characterImages[1].GetComponent<Image>().sprite = cHolder.GetInfo(dialogue[2]).sprite;
                        ImageSizeFitter(characterImages[1].GetComponent<Image>());
                    }
                }
                NextText();
                break;
            case "info":
                // dialogue[1]에 해당하는 정보를 생성에서 등록
                string source = dialogue[2];
                string[] values = source.Split('/');

                contents.GetComponent<CardAlignController>().AddCard(values[0], values[1]);
                alram.SetActive(true);
                NextText();
                break;
            case "object":
                string objectName = dialogue[2];

                if (objectName == "None")
                    objectPanel.SetActive(false);
                else
                {
                    string objectPath = "Object/" + objectName;
                    Sprite sp = Resources.Load<Sprite>(objectPath);
                    objectImage.sprite = sp;

                    objectPanel.SetActive(true);
                    objectImage.SetNativeSize();
                }

                NextText();
                break;
            case "choice":
                List<string> choices = new List<string>();

                for (int i = 2; i < dialogue.Count; i++) choices.Add(dialogue[i]);

                ShowChoicePanel(choices);
                break;
            case "result":
                int targetDialogueNum = int.Parse(nowDialogue[nowCnt][2]);
                nowDialogueNum += targetDialogueNum;
                nowDialogue = tHolder.GetDialogue(nowDialogueNum);
                nowCnt = 0;
                ShowText();
                break;
            case "endScene":
                StartCoroutine(ChangeScene());
                break;
            case "dayStart":
                StartCoroutine(DayStart(dialogue[2], dialogue[3]));
                break;
        }        
    }

    public void NextText()
    {
        OffInfoPanel();

        if (canGoNext == false) return;
        if (choicePanel.activeInHierarchy) return;

        nowCnt += 1;
        if (nowCnt >= nowDialogue.Count) return;

        ShowText();
    }

    public void ShowChoicePanel(List<string> choices)
    {
        // 선택지로 보여줄 선택글들의 리스트
        // 리스트의 갯수는 변동될 수 있다.
        // 선택지 object를 생성해서 적절하게 align해야한다.
        // 선택지들에게 버튼 액션을 할당한다.

        float delta = (float)500 / choices.Count;

        for (int i = 0; i < choices.Count; i++)
        {
            GameObject choice = Instantiate(choicePrefab, choicePanel.transform);
            choice.GetComponent<RectTransform>().localPosition = new Vector3(0, (choices.Count - 1 - 2 * i) * 0.5f * delta, 0);
            choice.transform.GetChild(0).GetComponent<Text>().text = choices[i];

            int actionNum = new int();
            actionNum = i + 2;
            Debug.Log(actionNum + "를 할당했습니다.");
            choice.GetComponent<Button>().onClick.AddListener(delegate { SelectChoice(actionNum); });
        }

        choicePanel.SetActive(true);
    }

    public void SelectChoice(int num)
    {
        // 선택지 패널을 끈다.
        // 다음 dialogue를 읽어서 선택지에 해당하는 번호의 대화를 불러온다.

        for (int i = choicePanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(choicePanel.transform.GetChild(i).gameObject);
        }
        choicePanel.SetActive(false);

        Debug.Log((nowCnt + 1) + " 줄에 있는 " + num + "번째 숫자를 가지고 옵니다.");
        int targetDialogueNum = int.Parse(nowDialogue[nowCnt + 1][num]);
        nowDialogueNum += targetDialogueNum;
        nowDialogue = tHolder.GetDialogue(nowDialogueNum);
        nowCnt = 0;
        ShowText();
    }

    private IEnumerator PrintText(string name, string text)
    {
        speechBox.GetComponent<Image>().color = cHolder.GetInfo(name).color;
        nameBox.GetComponent<Image>().color = cHolder.GetInfo(name).color;
        speakerName.text = name;
        speech.text = text.Replace('/', '\n');

        yield return new WaitForSeconds(delay);

        canGoNext = true;
    }

    public void SwitchInfoPanel()
    {
        if (isOn) OffInfoPanel();
        else OnInfoPanel();
    }

    private void OffInfoPanel()
    {
        RectTransform rect = infoPanel.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector3.zero;
        isOn = false;
    }

    private void OnInfoPanel()
    {
        RectTransform rect = infoPanel.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(-680, 0, 0);
        isOn = true;
    }

    private void ImageSizeFitter(Image target)
    {
        if (target.sprite == null) return;

        float originWidth = target.sprite.rect.width;
        float originHeight = target.sprite.rect.height;

        float ratio = 960f / originHeight;

        target.GetComponent<RectTransform>().sizeDelta = new Vector2(originWidth * ratio, 960f);
    }

    private IEnumerator ChangeScene()
    {
        sceneChangePanel.SetActive(true);
        speechBox.GetComponent<Button>().interactable = false;

        float speed = 4500f;
        RectTransform rect = sceneChangePanel.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(3000, 0);

        while (rect.anchoredPosition.x > 0)
        {
            rect.anchoredPosition -= new Vector2(speed * Time.fixedDeltaTime, 0);
            yield return new WaitForFixedUpdate();
        }

        NextText();
        yield return new WaitForSeconds(1f);

        while (rect.anchoredPosition.x > -3000f)
        {
            rect.anchoredPosition -= new Vector2(speed * Time.fixedDeltaTime, 0);
            yield return new WaitForFixedUpdate();
        }

        sceneChangePanel.SetActive(false);
        speechBox.GetComponent<Button>().interactable = true;
    }

    private IEnumerator DayStart(string dayText, string descriptionText)
    {
        float delta = 0.8f;

        this.dayText.text = "";
        this.descriptionText.text = "";

        Image image = dayStartPanel.GetComponent<Image>();
        image.color = Color.clear;
        dayStartPanel.SetActive(true);
        speechBox.GetComponent<Button>().interactable = false;

        while (image.color.a < 1f)
        {
            image.color += new Color(0, 0, 0, delta * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        this.dayText.text = dayText;
        this.descriptionText.text = descriptionText;

        yield return new WaitForSeconds(2f);
        NextText();

        this.dayText.text = "";
        this.descriptionText.text = "";

        while (image.color.a > 0)
        {
            image.color -= new Color(0, 0, 0, delta * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        dayStartPanel.SetActive(false);
        speechBox.GetComponent<Button>().interactable = true;
    }
}
