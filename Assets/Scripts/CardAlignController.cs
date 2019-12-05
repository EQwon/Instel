using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAlignController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> cards = new List<GameObject>();

    private void Update()
    {
        CardAlign();
    }

    private void CardAlign()
    {
        float posY = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform rect = cards[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-10f, posY);
            posY -= rect.sizeDelta.y + 50f;
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, Mathf.Abs(posY));
    }

    public void AddCard(string name, string description)
    {
        GameObject card = Instantiate(cardPrefab, transform);
        card.GetComponent<CardSizeFitter>().SetText(name, description);

        cards.Add(card);
    }
}
