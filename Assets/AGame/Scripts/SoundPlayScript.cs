using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayScript : MonoBehaviour
{
    public AudioSource SoundManager;
    [Space]
    public AudioClip BT_Sound;
    public AudioClip UP_Sound;
    public AudioClip Switch_Sound;
    public AudioClip[] Employee_Sound;

    private AudioClip temp;

    private AudioSource default_audioSource;
    private ForgeManager forgeManager;
    private int EmployeeSoundCounter = 0;
    
    void Start()
    {
        default_audioSource = transform.GetComponent<AudioSource>();
        if (default_audioSource != null && default_audioSource.clip != null)
            temp = default_audioSource.clip;
        forgeManager = GameObject.FindGameObjectWithTag("MainScript").transform.parent.GetChild(3).gameObject.GetComponent<ForgeManager>();
    }

    public void PlaySound()
    {
        default_audioSource.Play();
    }

    public void PlaySound(int forgeIndex)
    {
        if (forgeIndex == 1488)
        {
            default_audioSource.clip = UP_Sound;
            default_audioSource.Play();
        }
        else if(forgeManager.isBought[forgeIndex] && GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>().Ore >= 10 && !forgeManager.isUpgrading[forgeIndex] && forgeManager.isReady[forgeIndex])
        {
            default_audioSource.clip = temp;
            default_audioSource.Play();
        }
        else if (forgeManager.isBought[forgeIndex] && forgeManager.forgering[forgeIndex] > 0)
        {
            default_audioSource.clip = BT_Sound;
            default_audioSource.Play();
        }
    }

    public void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }

    public void PlayBtSound()
    {
        SoundManager.clip = BT_Sound;
        SoundManager.Play();
    }

    public void PlayUpgradeSound()
    {
        SoundManager.clip = UP_Sound;
        SoundManager.Play();
    }

    public void PlaySwitchSound()
    {
        SoundManager.clip = Switch_Sound;
        SoundManager.Play();
    }

    public void PlayEmployeeSound(EmployeeScript employeeScript)
    {
        if (employeeScript.isBought)
        {
            if (EmployeeSoundCounter >= Employee_Sound.Length)
                EmployeeSoundCounter = 0;
            SoundManager.clip = Employee_Sound[EmployeeSoundCounter];
            SoundManager.Play();
            EmployeeSoundCounter++;
        }
    }
}
