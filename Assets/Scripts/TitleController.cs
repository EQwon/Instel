using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject creditButton;

    public void MouseOver(RectTransform rect)
    {
        rect.localScale = new Vector2(1.5f, 1.5f);

        if (rect.GetComponent<Animator>())
        {
            rect.GetComponent<Animator>().enabled = false;
        }
    }

    public void MouseExit(RectTransform rect)
    {
        rect.localScale = new Vector2(1f, 1f);

        if (rect.GetComponent<Animator>())
        {
            rect.GetComponent<Animator>().enabled = true;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
