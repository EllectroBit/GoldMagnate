using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Playables;

public class ForgeManager : MonoBehaviour
{
    public int FORGERING_TIME = 5;
    public static int SpecialFuel = 3;
    
    //private const int UPGRADING_TIME = 60;
    //private const int START_MAX_INGOTS = 10;
    //private const int UPGRADE_COST = 100;

    private Core core;
    private LetterRounding lr = new LetterRounding();
    private DonateManager donateManager;
    private GameObject[] forge;

    public int[] MaxIngots { get; private set; }
    public long[] forgering { get; private set; }
    public bool[] isReady { get; private set; }
    public bool[] isUpgrading { get; private set; }
    public bool[] isBought { get; private set; }
    public int[] countOfIngots { get; private set; }

    public long[] UpgradeCost { get; private set; }
    public int[] UpgradeLVL { get; private set; }
    public int[] UpgradeTime { get; private set; }
    public int size { get; private set; }

    public Color colorToVisibel;
    public Color colorToInVisibel;
    public Color colorToUpgradeVisibel;
    [Space]
    public Sprite forgeOff;
    public Sprite forgeOn;
    public GameObject forges;
    public GameObject SFPanel;
    public Text _SFcost;
    [Space]
    public long[] forgePrice;
    [SerializeField]
    private ForgeUpgrade[] forgeUpgrade;

    private int indexTemp;
    private int SFcost;

    void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").GetComponent<Core>();
        donateManager = core.transform.parent.GetChild(7).gameObject.GetComponent<DonateManager>();
        size = forges.transform.childCount;

        forge = new GameObject[size];
        forgering = new long[size];
        MaxIngots = new int[size];
        isReady = new bool[size];
        isUpgrading = new bool[size];
        isBought = new bool[size];
        countOfIngots = new int[size];
        UpgradeCost = new long[size];
        UpgradeLVL = new int[size];
        UpgradeTime = new int[size];

        for (int i = 0; i < forges.transform.childCount; i++)
        {
            UpgradeLVL[i] = 1;
            forge[i] = forges.transform.GetChild(i).gameObject;
            forge[i].transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = lr.Rounding(forgePrice[i]);
            MaxIngots[i] = forgeUpgrade[UpgradeLVL[i] - 1].MaxIngots;
            isReady[i] = false;
            isUpgrading[i] = false;
            isBought[i] = false;
            UpgradeCost[i] = forgeUpgrade[UpgradeLVL[i] - 1].UpgradeCost;
            UpgradeTime[i] = forgeUpgrade[UpgradeLVL[i] - 1].UpgradingTime;
        }
        isReady[0] = true;
        isBought[0] = true;

        StartCoroutine(enumerator());

        _Load();
    }

    private void Update()
    {
        if (donateManager.IsForgeBoosted)
        {
            FORGERING_TIME = 1;
        }
    }

    public void OnBuyClicked(int index)
    {
        if (core.Money >= forgePrice[index])
        {
            core.Money -= forgePrice[index];
            forge[index].GetComponent<Image>().color = colorToVisibel;
            forge[index].GetComponent<Image>().maskable = true;
            forge[index].transform.GetChild(2).gameObject.SetActive(false);
            forge[index].transform.GetChild(0).gameObject.SetActive(true);
            if (index < forge.Length - 1) forge[index + 1].transform.GetChild(2).gameObject.SetActive(true);
            isReady[index] = true;
            isBought[index] = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("MessagePanel").GetComponent<MessagePanelScript>().NotEnoughMoney(forgePrice[index], core.Money);
        }
    }

    public void OnForgeClicked(int index)
    {
        if (core.Ore < 10 && isReady[index])
            TutorialScript.tutorialScript.ShowForgeT();
        if(core.Ore >= MaxIngots[index] * 10 && isReady[index] == true)
        {
            int time = MaxIngots[index] * FORGERING_TIME;
            core.Ore -= MaxIngots[index] * 10;
            forge[index].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            forgering[index] = time;
            countOfIngots[index] = MaxIngots[index];
            forge[index].GetComponent<Image>().sprite = forgeOn;
            isReady[index] = false;
        }
        else if(isReady[index] == true)
        {
            int temp = MaxIngots[index] * 10;
            for(int i = MaxIngots[index]; i > 0; i--)
            {
                if(core.Ore >= temp && temp > 0)
                {
                    core.Ore -= temp;
                    forge[index].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    forgering[index] = temp / 10 * FORGERING_TIME;
                    countOfIngots[index] = temp / 10;
                    forge[index].GetComponent<Image>().sprite = forgeOn;
                    isReady[index] = false;
                    break;
                }
                temp -= 10;
            }
        }
        else if(!isReady[index] && forgering[index] > 2)
        {
            SFOptions(1, index);
        }
    }

    private void SFOptions(int cost, int index)
    {
        SFPanel.SetActive(true);
        SFcost = cost;
        _SFcost.text = SFcost.ToString();
        indexTemp = index;
    }

    public void OnUpgradeBoostClicked(int index)
    {
        SFOptions(2 * UpgradeLVL[index], index);
    }

    IEnumerator enumerator()
    {
        while (true)
        {
            for(int i = 0; i < forges.transform.childCount; i++)
            {
                if (forgering[i] > 1)
                    forgering[i]--;
                else if(forgering[i] == 1)
                {
                    forgering[i] = 0;
                    forge[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    forge[i].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    forge[i].GetComponent<Image>().sprite = forgeOff;
                }


                if (isUpgrading[i] && UpgradeTime[i] > 1)
                {
                    UpgradeTime[i]--;
                    forge[i].transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                }
                else if(isUpgrading[i] && UpgradeTime[i] == 1)
                {
                    if(UpgradeLVL[i] != forgeUpgrade.Length - 1)
                    {
                        isUpgrading[i] = false;
                        UpgradeLVL[i]++;
                        MaxIngots[i] = forgeUpgrade[UpgradeLVL[i] - 1].MaxIngots;
                        UpgradeTime[i] = forgeUpgrade[UpgradeLVL[i] - 1].UpgradingTime;
                        UpgradeCost[i] = forgeUpgrade[UpgradeLVL[i] - 1].UpgradeCost;
                        //forge[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = colorToUpgradeVisibel;
                        forge[i].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        forge[i].transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        forge[i].transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(true);
                        isReady[i] = true;
                    }
                    else
                    {
                        isUpgrading[i] = false;
                        UpgradeLVL[i]++;
                        MaxIngots[i] = forgeUpgrade[UpgradeLVL[i] - 1].MaxIngots;
                        forge[i].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        forge[i].transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        forge[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = colorToInVisibel;
                        isReady[i] = true;
                    }
                }

            }         

            yield return new WaitForSeconds(1);
        }
    }

    public void OnIngotClicked(int index)
    {
        core.Gold += countOfIngots[index];
        forge[index].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        isReady[index] = true;
    }

    public void OnUpgradeClicked(int index)
    {
        if(UpgradeCost[index] <= core.Money && isReady[index] == true && UpgradeLVL[index] < forgeUpgrade.Length)
        {
            core.Money -= UpgradeCost[index];
            isUpgrading[index] = true;
            isReady[index] = false;
            //forge[index].transform.GetChild(0).gameObject.GetComponent<Image>().color = colorToInVisibel;
            forge[index].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            forge[index].transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            forge[index].transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if(isReady[index] == true && UpgradeLVL[index] < forgeUpgrade.Length)
        {
            GameObject.FindGameObjectWithTag("MessagePanel").GetComponent<MessagePanelScript>().NotEnoughMoney(UpgradeCost[index], core.Money);
        }
    }

    public Save _Save(Save sv)
    {
        Save save = sv;

        save.MaxIngots = MaxIngots;
        save.forgering = forgering;
        save.isReady = isReady;
        save.isUpgrading = isUpgrading;
        save.F_isBought = isBought;
        save.countOfIngots = countOfIngots;
        save.UpgradeCost = UpgradeCost;
        save.UpgradeLVL = UpgradeLVL;
        save.UpgradeTime = UpgradeTime;
        save.SpecialFuel = SpecialFuel;

        return save;
    }

    public void _Load()
    {
        if (!core.NewGame && PlayerPrefs.HasKey("SAVE"))
        {
            MaxIngots = core.load.MaxIngots;
            forgering = core.load.forgering;
            isReady = core.load.isReady;
            isUpgrading = core.load.isUpgrading;
            isBought = core.load.F_isBought;
            countOfIngots = core.load.countOfIngots;
            UpgradeCost = core.load.UpgradeCost;
            UpgradeLVL = core.load.UpgradeLVL;
            UpgradeTime = core.load.UpgradeTime;
            SpecialFuel = core.load.SpecialFuel;

            DateTime loadDT = new DateTime(core.load.date[0], core.load.date[1], core.load.date[2], core.load.date[3], core.load.date[4], core.load.date[5]);
            TimeSpan timeSpan = DateTime.Now - loadDT;
            long totalsec = (long)timeSpan.TotalSeconds;
            for(int i = 0; i < size; i++)
            {
                if (isBought[i] && !isReady[i] && forgering[i] - totalsec > 1)
                {
                    forgering[i] -= totalsec;
                }
                else if (isBought[i] && !isReady[i] && !isUpgrading[i])
                {
                    forgering[i] = 1;
                }
                else if (isUpgrading[i] && UpgradeTime[i] - totalsec > 1)
                {
                    UpgradeTime[i] -= (int)totalsec;
                }
                else if (isUpgrading[i])
                {
                    UpgradeTime[i] = 1;
                }
            }

            StateLoad();
        }
    }

    private void StateLoad()
    {
        for(int i = 0; i < size; i++)
        {
            if (isReady[i])
            {
                MakeForgeVisible(i);
            }
            else if (forgering[i] > 0)
            {
                MakeForgeVisible(i);
                forge[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (isUpgrading[i])
            {
                MakeForgeVisible(i);
                //forge[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = colorToInVisibel;
                forge[i].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                forge[i].transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (isBought[i])
            {
                MakeForgeVisible(i);
                forge[i].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }

            if(UpgradeLVL[i] >= forgeUpgrade.Length - 1)
            {
                forge[i].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                forge[i].transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                forge[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = colorToInVisibel;
            }
        }
    }

    private void MakeForgeVisible(int i)
    {
        forge[i].GetComponent<Image>().color = colorToVisibel;
        forge[i].transform.GetChild(0).gameObject.SetActive(true);
        forge[i].transform.GetChild(2).gameObject.SetActive(false);
        if (i + 1 < size) forge[i + 1].transform.GetChild(2).gameObject.SetActive(true);
    }

    public void OnCancelClicked()
    {
        SFPanel.SetActive(false);
    }
    
    public void OnOKCliked(PanelSwitcher ps)
    {
        if(SpecialFuel >= SFcost)
        {
            if(SFcost == 1)
            {
                SpecialFuel -= SFcost;
                forgering[indexTemp] = 1;
            }
            else if (SFcost > 1)
            {
                SpecialFuel -= SFcost;
                UpgradeTime[indexTemp] = 1;
            }
        }
        else
        {
            ps.OnShopButtonCliked();
        }
        SFPanel.SetActive(false);
    }
}


[Serializable]
public class ForgeUpgrade
{
    public int UpgradingTime;
    public int MaxIngots;
    public int UpgradeCost;
}
