using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSizeFitter : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private RectTransform descriptionBox;

    private void FixedUpdate()
    {
        ResizeCard();
    }

    private void ResizeCard()
    {
        float textSizeY = descriptionText.GetComponent<RectTransform>().sizeDelta.y;
        descriptionBox.sizeDelta = new Vector2(descriptionBox.sizeDelta.x, textSizeY + 70);
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, textSizeY + 120);
    }

    public void SetText(string name, string description)
    {
        nameText.text = name;
        descriptionText.text = description;
    }
}
