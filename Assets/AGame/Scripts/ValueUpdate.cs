using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueUpdate : MonoBehaviour
{
    public string UpdateKey;
    public int IndexKey_option;

    private LetterRounding lr;
    private Text text;

    private Core core;
    private MineManager mineScript;
    private ForgeManager forgeManager;
    private CityManager cityManager;
    private EmployeeScript employeeScript;
    private WorkersManager workersManager;
    private WorkerScript workerScript;
     
    void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();
        text = GetComponent<Text>();
        lr = new LetterRounding();
        mineScript = core.transform.parent.transform.GetChild(2).gameObject.GetComponent<MineManager>();
        forgeManager = core.transform.parent.transform.GetChild(3).gameObject.GetComponent<ForgeManager>();
        cityManager = core.transform.parent.transform.GetChild(4).gameObject.GetComponent<CityManager>();
        employeeScript = transform.gameObject.transform.parent.parent.GetComponent<EmployeeScript>();
        workerScript = transform.gameObject.transform.parent.parent.GetComponent<WorkerScript>();
        workersManager = core.transform.parent.transform.GetChild(5).gameObject.GetComponent<WorkersManager>();
    }
    

    void Update()
    {
        if (UpdateKey == "ore") text.text = lr.Rounding(core.Ore);
        else if (UpdateKey == "gold") text.text = lr.Rounding(core.Gold);
        else if (UpdateKey == "money") text.text = lr.Rounding(core.Money);
        else if (UpdateKey == "orePerClick") text.text = lr.Rounding(mineScript.OrePerClick);
        else if (UpdateKey == "mineUpgradeCost") text.text = lr.Rounding(mineScript.UpgradeCost[mineScript.UpgradeCounter]);
        else if (UpdateKey == "timer") text.text = lr.DateRounding(forgeManager.forgering[IndexKey_option]);
        else if (UpdateKey == "ingot") text.text = lr.Rounding(forgeManager.countOfIngots[IndexKey_option]);
        else if (UpdateKey == "forgeUpgradeLVL_MAX") text.text = $"{forgeManager.UpgradeLVL[IndexKey_option]} LVL\nMAX: {forgeManager.MaxIngots[IndexKey_option]}";
        else if (UpdateKey == "UpgradingTime")
        {
            int tempTime = forgeManager.UpgradeTime[IndexKey_option];
            string strTempTime = (tempTime == 0) ? "00.00.00" : lr.DateRounding(tempTime);
            text.text = strTempTime;
        }
        else if (UpdateKey == "ForgeUpCost") text.text = lr.Rounding(forgeManager.UpgradeCost[IndexKey_option]);
        else if (UpdateKey == "buyForge") text.text = lr.Rounding(forgeManager.forgePrice[IndexKey_option]);
        else if (UpdateKey == "goldPrice") text.text = cityManager.goldPrice.ToString();
        else if (UpdateKey == "saleLVL") text.text = cityManager.SaleLVL.ToString();
        else if (UpdateKey == "saleMaxIngots") text.text = lr.Rounding(cityManager.ingotsToSell);
        else if (UpdateKey == "employeeLVL") text.text = employeeScript.LVL.ToString();
        else if (UpdateKey == "employeeCost") text.text = lr.Rounding(employeeScript.hirePrice);
        else if (UpdateKey == "workerLVL_goldPerSec") text.text = $"LVL:{lr.Rounding(workerScript.LVL)}\n{lr.Rounding(workerScript.goldPerSec)}/sec";
        else if (UpdateKey == "workerUpgradeCost") text.text = lr.Rounding(workerScript.currentUpgradeCost);
        else if (UpdateKey == "workerBuyPrice") text.text = lr.Rounding(workerScript.buyCost);
        else if (UpdateKey == "specialFuel") text.text = ForgeManager.SpecialFuel.ToString();
        else if (UpdateKey == "(fTime") text.text = $"({forgeManager.FORGERING_TIME}";
    }
}
