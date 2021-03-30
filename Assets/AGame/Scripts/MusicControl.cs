using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    public AudioSource audioSource;
    [Space]
    public Sprite musicON;
    public Sprite musicOFF;
    [Space]
    public Slider slider;
    public Image image;

    public float current = 0.50f;
    public float temp = 0.50f;

    private void Start()
    {
        _Load();
    }

    public void OnSliderMove()
    {
        audioSource.volume = slider.value;

        if(slider.value == 0)
            image.sprite = musicOFF;
        else if(slider.value > 0)
            image.sprite = musicON;

        current = slider.value;
    }

    public void OnSliderMove(float value)
    {
        slider.value = value;
        audioSource.volume = slider.value;

        if (slider.value == 0)
            image.sprite = musicOFF;
        else if (slider.value > 0)
            image.sprite = musicON;

        current = slider.value;
    }

    public void OnSliderMove(float value, bool temp)
    {
        slider.value = value;
        audioSource.volume = slider.value;

        if (slider.value == 0)
            image.sprite = musicOFF;
        else if (slider.value > 0)
            image.sprite = musicON;
    }

    public void OnButtonClicked()
    {
        if(slider.value == 0)
        {
            OnSliderMove(temp);
        }
        else
        {
            temp = slider.value;
            OnSliderMove(0.0f, false);
        }
    }

    private void _Load()
    {
        if (!GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>().NewGame && PlayerPrefs.HasKey("SAVE"))
        {
            current = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>().load.music;
            OnSliderMove(current);
        }
    }
}
