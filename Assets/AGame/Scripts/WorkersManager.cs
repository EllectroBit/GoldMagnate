using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class WorkersManager : MonoBehaviour
{
    private const int OUT_TIME_COEF = 4;

    public GameObject MainObject;
    public Image Vision;
    public Color panelON;
    public Color panelOFF;
    public Color buttonUnPressed;
    public Color buttonPressed;
    public Sprite visionOn;
    public Sprite visionOff;
    [Space]
    public long[] workLinesPrice;
    public long totalGoldPerSec { get; set; }

    private Core core;
    public GameObject[] workLine { get; set; }
    public WorkerScript[] workerScript { get; private set; }
    public int countOfLines { get; private set; }

    public bool needToLoad = true;
    private bool vision = true;

    private void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();
        countOfLines = MainObject.transform.childCount;

        workLine = new GameObject[countOfLines];
        workerScript = new WorkerScript[countOfLines];

        for(int i = 0; i < countOfLines; i++)
        {
            workLine[i] = MainObject.transform.GetChild(i).gameObject;
            workerScript[i] = workLine[i].GetComponent<WorkerScript>();
        }

        StartCoroutine(GoldPerSec());
    }

    private void Update()
    {
        if (needToLoad)
        {
            for (int i = 0; i < countOfLines; i++)
            {
                workerScript[i]._Load(true);
            }

            _Load();

            needToLoad = false;
        }
    }

    IEnumerator GoldPerSec()
    {
        while (true)
        {
            for(int i = 0; i < countOfLines; i++)
            {
                //Debug.Log(workerScript[i].goldPerSec);
                if(workerScript[i].isBought) totalGoldPerSec += workerScript[i].goldPerSec;
            }
            core.Ore += totalGoldPerSec; //Debug.Log(totalGoldPerSec);
            totalGoldPerSec = 0; 
            yield return new WaitForSeconds(1);
        }
    }

    private void _Load()
    {
        long afkOre = GetafkOre();
        if (!core.NewGame && afkOre > 100)
            core.transform.parent.GetChild(7).gameObject.GetComponent<DonateManager>().WorkersAfkOre(afkOre);
        else if (!core.NewGame) 
            core.Ore += afkOre;
    }

    private long GetafkOre()
    {
        if (PlayerPrefs.HasKey("SAVE"))
        {
            DateTime loadDT = new DateTime(core.load.date[0], core.load.date[1], core.load.date[2], core.load.date[3], core.load.date[4], core.load.date[5]);
            TimeSpan timeSpan = DateTime.Now - loadDT;
            long totalsec = (long)timeSpan.TotalSeconds > 7200 ? 7200 : (long)timeSpan.TotalSeconds;

            for (int i = 0; i < countOfLines; i++)
            {
                if (workerScript[i].isBought) totalGoldPerSec += workerScript[i].goldPerSec;
            }
            long res = (totalGoldPerSec * totalsec) / OUT_TIME_COEF;
            totalGoldPerSec = 0;
            return res;
        }
        else return 0;
    }

    public void OnVisionButtonClicked()
    {
        if (vision)
        {
            for(int i = 0; i < countOfLines; i++)
            {
                if (workerScript[i].isBought)
                    workLine[i].transform.GetChild(1).gameObject.SetActive(false);
            }

            Vision.sprite = visionOff;
            vision = false;
        }
        else
        {
            for (int i = 0; i < countOfLines; i++)
            {
                if(workerScript[i].isBought)
                    workLine[i].transform.GetChild(1).gameObject.SetActive(true);
            }

            Vision.sprite = visionOn;
            vision = true;
        }
    }
}
