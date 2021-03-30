using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeScript : MonoBehaviour
{
    private const int XP_PER_CLICK = 10;

    private CityManager cityManager;
    private Core core;
    private Slider slider;
    public long hirePrice { get; private set; }
    public int LVL { get; private set; }
    public int maxEnergy { get; set; }
    public int currentEnergy { get; set; }
    private long maxXP = 100;
    public long currentXP { get; set; }
    public bool isBought = false;

    public int index;

    void Start()
    {
        LVL = 1;
        maxEnergy = 10;
        currentEnergy = maxEnergy;
     
        core = GameObject.FindGameObjectWithTag("MainScript").gameObject.GetComponent<Core>();
        cityManager = core.transform.parent.GetChild(4).gameObject.GetComponent<CityManager>();
        slider = transform.gameObject.transform.GetChild(1).gameObject.GetComponent<Slider>();
    }
    void Update()
    {
        slider.maxValue = maxEnergy;
        slider.value = currentEnergy;

        if (currentXP >= maxXP)
        {
            LVL++;
            currentXP = 0;
            maxXP = LVL * 100;
        }
    }

    public void SetHirePrice(long price)
    {
        hirePrice = price;
    }

    public void HireNew()
    {
        if(core.Money >= hirePrice)
        {
            core.Money -= hirePrice;
            cityManager.employees[index].transform.GetChild(0).gameObject.SetActive(false);
            cityManager.employees[index].GetComponent<Image>().color = cityManager.ObjON;
            cityManager.employees[index].transform.GetChild(1).gameObject.SetActive(true);
            cityManager.employees[index].transform.GetChild(2).gameObject.SetActive(true);
            if(index + 1 < cityManager.EmployeesCount) cityManager.employees[index + 1].transform.GetChild(0).gameObject.SetActive(true);
            isBought = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("MessagePanel").GetComponent<MessagePanelScript>().NotEnoughMoney(hirePrice, core.Money);
        }
    }

    public void OnEmployeeClick()
    {
        if(isBought == true && currentEnergy > 0)
        {
            currentEnergy--;
            XpAdd();
            currentXP += LVL * 10;
            core.Money += LVL;
        }
    }

    private void XpAdd()
    {
        if(LVL < 10)
            cityManager.currentXP += LVL * 10;
        else if (LVL < 50)
            cityManager.currentXP += 50 + ((LVL * 10) / 2);
        else if (LVL < 100)
            cityManager.currentXP += 160 + ((LVL * 10) / 4);
        else
            cityManager.currentXP += 250 + ((LVL * 10) / 4);
    }

    public Save _Save(Save sv)
    {
        Save save = sv;

        save.LVL[index] = LVL;
        save.currentEnergy[index] = currentEnergy;
        save.E_maxXP[index] = maxXP;
        save.E_currentXP[index] = currentXP;
        save.isBought[index] = isBought;

        return save;
    }

    public void _Load()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();

        if (!core.NewGame && PlayerPrefs.HasKey("SAVE"))
        {
            LVL = core.load.LVL[index];
            currentEnergy = core.load.currentEnergy[index];
            maxXP = core.load.E_maxXP[index];
            currentXP = core.load.E_currentXP[index];
            isBought = core.load.isBought[index];

            StateLoad();
        }
    }

    private void StateLoad()
    {
        cityManager = GameObject.FindGameObjectWithTag("MainScript").gameObject.GetComponent<Core>().transform.parent.GetChild(4).gameObject.GetComponent<CityManager>();

        if (isBought)
        {
            transform.GetComponent<Image>().color = cityManager.ObjON;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);

            if (index + 1 < cityManager.EmployeesCount)
                cityManager.employees[index + 1].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
