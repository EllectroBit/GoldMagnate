using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanelScript : MonoBehaviour
{
    private Text number;

    private void Start()
    {
        number = transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>();
    }
    
    public void NotEnoughMoney(long need, long current)
    {
        number.text = (need - current).ToString();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnButtonClicked()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
