using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public Animation animation;
    public GameObject Obj;

    private bool isOpen;

    void Start()
    {

    }
    void Update()
    {
        
    }

    public void AnimationPlay()
    {
        animation.Play();
    }

    public void AnimationPlay(EmployeeScript employeeScript)
    {
        if (employeeScript.isBought)
            animation.Play();
    }

    public void QWE()
    {

    }

    public void AnimationOpen()
    {
        if (!isOpen)
        {
            animation.Play("AdditionalPanelAnim");
            isOpen = true;
        }
        else
        {
            animation.Play("CloseAnim");
            isOpen = false;
        }
    }

    public void ColorOn()
    {
        Obj.transform.GetChild(0).gameObject.SetActive(true);
        Obj.transform.GetChild(1).gameObject.SetActive(true);
        Obj.transform.GetChild(2).gameObject.SetActive(true);

        Obj.transform.GetChild(0).GetComponent<Animation>().Play();
        Obj.transform.GetChild(1).GetComponent<Animation>().Play();
        Obj.transform.GetChild(2).GetComponent<Animation>().Play();
    }

    public void ColorOff()
    {
        Obj.transform.GetChild(0).gameObject.SetActive(false);
        Obj.transform.GetChild(1).gameObject.SetActive(false);
        Obj.transform.GetChild(2).gameObject.SetActive(false);
    }
}
