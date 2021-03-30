using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WorkerScript : MonoBehaviour
{
    public int index;

    private const int D_GOLD_PER_SEC = 10;

    private Core core;
    private WorkersManager workersManager;

    public long goldPerSec { get; private set; }
    private long goldPerSecPerLVL;
    public int LVL { get; private set; }
    public long buyCost { get; private set; }
    public long upgradeCost { get; private set; }
    public long currentUpgradeCost { get; private set; }
    public int upgradeMult { get; private set; }
    public bool isBought { get; private set; }

    private enum upgradeMultPull { ONE = 1, TEN = 10, HUNDRED = 100}

    private void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();
        workersManager = core.transform.parent.transform.GetChild(5).gameObject.GetComponent<WorkersManager>();

        isBought = false;
        LVL = 1;
        buyCost = workersManager.workLinesPrice[index];
        goldPerSecPerLVL = buyCost / 100;
        upgradeMult = (int)upgradeMultPull.ONE;
        goldPerSec = goldPerSecPerLVL * LVL;
        upgradeCost = buyCost / 100 * 25;
        currentUpgradeCost = GetMultUpgradeCost();

        _Load(false);
    }

    public void OnBuyClick()
    {
        if(core.Money >= buyCost)
        {
            core.Money -= buyCost;
            workersManager.workLine[index].gameObject.GetComponent<Image>().color = workersManager.panelON;
            workersManager.workLine[index].transform.GetChild(0).gameObject.SetActive(false);
            workersManager.workLine[index].transform.GetChild(1).gameObject.SetActive(true);
            if (index + 1 < workersManager.countOfLines) workersManager.workLine[index + 1].transform.GetChild(0).gameObject.SetActive(true);
            isBought = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("MessagePanel").GetComponent<MessagePanelScript>().NotEnoughMoney(buyCost, core.Money);
        }
    }

    public void OnUpgradeMultClicked(int mult)
    {
        switch (mult)
        {
            case 1:
                upgradeMult = (int)upgradeMultPull.ONE;
                ChangeMultColor(1);
                break;
            case 10:
                upgradeMult = (int)upgradeMultPull.TEN;
                ChangeMultColor(2);
                break;
            case 100:
                upgradeMult = (int)upgradeMultPull.HUNDRED;
                ChangeMultColor(3);
                break;
            default: break;
        }
        currentUpgradeCost = GetMultUpgradeCost();
    }

    public void OnUpgradeClicked()
    {
        if(core.Money >= currentUpgradeCost)
        {
            core.Money -= currentUpgradeCost;
            LVL += upgradeMult;
            goldPerSec = goldPerSecPerLVL * LVL;
            upgradeCost += upgradeMult * LVL;
            currentUpgradeCost = GetMultUpgradeCost();
        }
        else
        {
            GameObject.FindGameObjectWithTag("MessagePanel").GetComponent<MessagePanelScript>().NotEnoughMoney(currentUpgradeCost, core.Money);
        }
    }

    private long GetUpgradeCost()
    {
        return upgradeCost;
    }

    public long GetMultUpgradeCost()
    {
        return GetUpgradeCost() * (upgradeMult + LVL);
    }

    private void ChangeMultColor(int on)
    {
        workersManager.workLine[index].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = workersManager.buttonUnPressed;
        workersManager.workLine[index].transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Image>().color = workersManager.buttonUnPressed;
        workersManager.workLine[index].transform.GetChild(1).gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = workersManager.buttonUnPressed;
        workersManager.workLine[index].transform.GetChild(1).gameObject.transform.GetChild(on).gameObject.GetComponent<Image>().color = workersManager.buttonPressed;
    }

    public Save _Save(Save sv)
    {
        Save save = sv;

        save.goldPerSec[index] = goldPerSec;
        save.goldPerSecPerLVL[index] = goldPerSecPerLVL;
        save.W_LVL[index] = LVL;
        save.upgradeCost[index] = upgradeCost;
        save.currentUpgradeCost[index] = currentUpgradeCost;
        save.upgradeMult[index] = upgradeMult;
        save.W_isBought[index] = isBought;

        return save;
    }

    public void _Load(bool sl)
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();

        if (!core.NewGame && PlayerPrefs.HasKey("SAVE"))
        {
            goldPerSec = core.load.goldPerSec[index];
            goldPerSecPerLVL = core.load.goldPerSecPerLVL[index];
            LVL = core.load.W_LVL[index];
            upgradeCost = core.load.upgradeCost[index];
            currentUpgradeCost = core.load.currentUpgradeCost[index];
            upgradeMult = core.load.upgradeMult[index];
            isBought = core.load.W_isBought[index];

            if(sl) 
                StateLoad();
        }
    }

    private void StateLoad()
    {
        workersManager = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>().transform.parent.transform.GetChild(5).gameObject.GetComponent<WorkersManager>();

        if (isBought)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            workersManager.workLine[index].gameObject.GetComponent<Image>().color = workersManager.panelON;

            if (index + 1 < workersManager.countOfLines)
                workersManager.workLine[index + 1].transform.GetChild(0).gameObject.SetActive(true);

            OnUpgradeMultClicked(upgradeMult);
        }
    }
}
