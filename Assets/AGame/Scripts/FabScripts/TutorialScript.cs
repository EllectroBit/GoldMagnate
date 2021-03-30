using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public GameObject Tutorial;
    public Image image;
    public Text text;
    public Sprite ore;
    public Sprite ingot;

    public static TutorialScript tutorialScript;

    private void Awake()
    {
        tutorialScript = this;
    }

    public void OnCloseBTClicked()
    {
        Tutorial.SetActive(false);
    }

    public void ShowForgeT()
    {
        Tutorial.SetActive(true);
        text.text = "10";
        image.sprite = ore;
    }

    public void ShowCityT()
    {
        Tutorial.SetActive(true);
        text.text = "1";
        image.sprite = ingot;
    }
}
