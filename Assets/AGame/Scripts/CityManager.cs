using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityManager : MonoBehaviour
{
    private const int START_MAX_XP = 100;
    private const int START_INGOTS = 1;
    private const int MINIMUM_GOLD_PRICE = 15;
    private const int GOLD_PRICE_STEP = 60;

    private Core core;
    private LetterRounding lr = new LetterRounding();
    private DonateManager donateManager;

    public int SaleLVL { get; private set; }
    public int ingotsToSell { get; private set; }
    private long tempXP;
    public long currentXP { get;  set; }
    private long maxXP;
    private int possibilityToDrop = 25;
    private long XPcounter;
    public int EmployeesCount { get; private set; }

    public GameObject[] employees { get; private set; }
    public EmployeeScript[] employeeScript { get; private set; }

    public int goldPrice = MINIMUM_GOLD_PRICE;
    public Color ObjON;
    public Color ObjOFF;
    public GameObject MainEmployee;
    public Slider slider;
    public GameObject AddXP;
    public GameObject CityTAB;
    [Space]
    public int[] HirePrice;

    private bool needToLoad = true;

    void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();
        EmployeesCount = MainEmployee.transform.childCount;
        donateManager = core.transform.parent.GetChild(7).gameObject.GetComponent<DonateManager>();

        employeeScript = new EmployeeScript[EmployeesCount];
        employees = new GameObject[EmployeesCount];

        SaleLVL = 1;
        ingotsToSell = 1;

        for(int i = 0; i < EmployeesCount; i++)
        {
            employees[i] = MainEmployee.transform.GetChild(i).gameObject;
            employeeScript[i] = employees[i].GetComponent<EmployeeScript>();
            employeeScript[i].SetHirePrice(HirePrice[i]);
        }

        tempXP = currentXP;
        StartCoroutine(barchek());
        StartCoroutine(goldPriceEnumerator());
        StartCoroutine(employeesEnergy());

        _Load();
    }

    void Update()
    {
        SetMaxXp();
        slider.maxValue = maxXP;
        slider.value = currentXP;
        if(currentXP >= maxXP)
        {
            SaleLVL++;
            currentXP = 0;
            ingotsToSell += START_INGOTS * SaleLVL / 2;
        }

        if (tempXP != currentXP)
        {
            AddXP.SetActive(true);
            if ((currentXP - tempXP) > 0) AddXP.GetComponent<Text>().text = $"+{lr.Rounding(currentXP - tempXP)}";
            tempXP = currentXP;
        }

        if (needToLoad)
        {
            for (int i = 0; i < EmployeesCount; i++)
            {
                employeeScript[i]._Load();
            }
            needToLoad = false;
        }

        if (donateManager.IsGoldPriceBoosted)
        {
            goldPrice = 75;
            StopCoroutine(goldPriceEnumerator());
        }
    }

    private void SetMaxXp()
    {
        if(SaleLVL < 5)
            maxXP = START_MAX_XP * SaleLVL * 2;
        else if (SaleLVL < 10)
            maxXP = START_MAX_XP * SaleLVL * 4;
        else if (SaleLVL < 20)
            maxXP = START_MAX_XP * SaleLVL * 8;
        else
            maxXP = START_MAX_XP * SaleLVL * 12;
    }

    IEnumerator goldPriceEnumerator()
    {
        int temp;
        while (true)
        {
            goldPrice += Random.Range(1, 2);
            temp = Random.Range(1, 100);
            if (temp <= possibilityToDrop && goldPrice > MINIMUM_GOLD_PRICE + 5)
            {
                goldPrice -= Random.Range(1, 5);
            }
            else if (possibilityToDrop >= 75) possibilityToDrop = 25;
            else
            {
                possibilityToDrop++;
            }

            yield return new WaitForSeconds(GOLD_PRICE_STEP);
        }
    }

    IEnumerator barchek()
    {
        while (true)
        {
            AddXP.SetActive(false);
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator employeesEnergy()
    {
        while (true)
        {
            for (int i = 0; i < EmployeesCount; i++)
            {
                if (employeeScript[i].currentEnergy < employeeScript[i].maxEnergy) employeeScript[i].currentEnergy++;
            }
            yield return new WaitForSeconds(2);
        }
    }

    public void OnSellClick()
    {
        if(core.Gold < 1)
            TutorialScript.tutorialScript.ShowCityT();

        for (int i = ingotsToSell; i > 0; i--)
        {
            if(core.Gold >= i)
            {
                core.Gold -= i;
                core.Money += i * goldPrice;
                if (goldPrice - i / 5 < MINIMUM_GOLD_PRICE + i) goldPrice = MINIMUM_GOLD_PRICE;
                else goldPrice -= i / 5;
                currentXP += i * 10;
                break;
            }            
        }
    }

    public Save _Save(Save sv)
    {
        Save save = sv;

        save.SaleLVL = SaleLVL;
        save.ingotsToSell = ingotsToSell;
        save.tempXP = tempXP;
        save.currentXP = currentXP;
        save.maxXP = maxXP;
        save.possibilityToDrop = possibilityToDrop;

        return save;
    }

    private void _Load()
    {
        if (!core.NewGame && PlayerPrefs.HasKey("SAVE"))
        {
            SaleLVL = core.load.SaleLVL;
            ingotsToSell = core.load.ingotsToSell;
            tempXP = core.load.tempXP;
            currentXP = core.load.currentXP;
            maxXP = core.load.maxXP;
            possibilityToDrop = core.load.possibilityToDrop;
            XPcounter = core.load.XPcounter;
        }
    }
}