using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenRed : MonoBehaviour
{
    public Color green;
    public Color red;

    public void SetColor(bool operation)
    {
        if (operation)
            transform.gameObject.GetComponent<Text>().color = green;
        else
            transform.gameObject.GetComponent<Text>().color = red;
    }
}
