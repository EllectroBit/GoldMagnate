using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    private Core core;
    private System.Random random;
    private ClickObjectFly[] clickObjPool = new ClickObjectFly[30]; 

    private int counter = 0;
    private int ObjCounter = 0;
    public int UpgradeCounter { get; private set; }
    public long OrePerClick { get; private set; }

    public long[] UpgradeCost;
    [Space]
    public long[] UpgradeOre;
    [Space]
    public GameObject[] AdditionalOre;
    [Space]
    public GameObject clickObjPrefab;
    public GameObject clickPerent;

    void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();
        random = new System.Random();
        UpgradeCounter = 0;
        OrePerClick = 1;

        for (int i = 0; i < clickObjPool.Length; i++)
        {
            clickObjPool[i] = Instantiate(clickObjPrefab, clickPerent.transform).GetComponent<ClickObjectFly>();
        }

        _Load();
    }
    void Update()
    {
        if (counter >= 15)
            AdditionOreSetActive();
    }

    public void OnOreClicked()
    {
        core.Ore += OrePerClick;
        counter++;
        ObjCounter++;
        if (ObjCounter < clickObjPool.Length) clickObjPool[ObjCounter].StartMotion();
        else ObjCounter = 0;
    }

    private void AdditionOreSetActive()
    {        
        counter = 0;
        GameObject temp = AdditionalOre[random.Next(0, AdditionalOre.Length)];
        if (!temp.activeSelf)
            temp.SetActive(true);
        //else
        //    AdditionOreSetActive();
    }

    public void OnAdditionalOreClicker(int i)
    {
        core.Ore += OrePerClick * random.Next(2, 5);
        AdditionalOre[i].SetActive(false);
    }

    public void OnUpgradeClicked()
    {
        if(UpgradeCounter < UpgradeCost.Length - 1 && core.Money >= UpgradeCost[UpgradeCounter])
        {
            core.Money -= UpgradeCost[UpgradeCounter];
            OrePerClick = UpgradeOre[UpgradeCounter];
            UpgradeCounter++;
        }
        else
        {
            GameObject.FindGameObjectWithTag("MessagePanel").GetComponent<MessagePanelScript>().NotEnoughMoney(UpgradeCost[UpgradeCounter], core.Money);
        }
    }

    public Save _Save(Save sv)
    {
        Save save = sv;

        save.counter = counter;
        save.UpgradeCounter = UpgradeCounter;
        save.OrePerClick = OrePerClick;

        return save;
    }

    private void _Load()
    {
        if (!core.NewGame && PlayerPrefs.HasKey("SAVE"))
        {
            counter = core.load.counter;
            UpgradeCounter = core.load.UpgradeCounter;
            OrePerClick = core.load.OrePerClick;
        }
    }
}