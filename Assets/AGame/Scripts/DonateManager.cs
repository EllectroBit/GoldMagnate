using Assets.AGame.Scripts.FabScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonateManager : MonoBehaviour
{
    public GameObject dParent;
    public GameObject WorkersReport;

    private Core core;
    private long afkOreTemp;

    private void Awake()
    {
        _Load();
    }

    void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").gameObject.GetComponent<Core>();
    }

    public void AddMoney(long i)
    {
        core.Money += i;
    }

    public void AddSpecialFuel(int i)
    {
        ForgeManager.SpecialFuel += i;
    }

    public bool IsForgeBoosted
    {
        get
        {
            if (PlayerPrefs.HasKey("IAP_IS_FORGE_BOOST") && PlayerPrefs.GetString("IAP_IS_FORGE_BOOST") == "true")
                return true;
            else
                return false;
        }
        set
        {
            if (value)
                PlayerPrefs.SetString("IAP_IS_FORGE_BOOST", "true");
        }
    }

    public bool IsGoldPriceBoosted
    {
        get
        {
            if (PlayerPrefs.HasKey("IAP_IS_GOLD_PRICE_BOOST") && PlayerPrefs.GetString("IAP_IS_GOLD_PRICE_BOOST") == "true")
                return true;
            else
                return false;
        }
        set
        {
            if (value)
                PlayerPrefs.SetString("IAP_IS_GOLD_PRICE_BOOST", "true");
        }
    }

    public bool IsNoAds
    {
        get
        {
            return true;
            //if (PlayerPrefs.HasKey("IAP_IS_NO_ADS") && PlayerPrefs.GetString("IAP_IS_NO_ADS") == "true")
            //    return true;
            //else
            //    return false;
        }
        set
        {
            if (value)
                PlayerPrefs.SetString("IAP_IS_NO_ADS", "true");
        }
    }

    public void OnForgeBoostButtonClicked(GameObject gameObject)
    {
        IAPManager.instance.BuyForgeBoost();
    }
    
    public void OnGoldBoostButtonClicked(GameObject gameObject)
    {
        IAPManager.instance.BuyGoldPriceBoost();
    }

    public void OnNoAdsButtonClicked(GameObject gameObject)
    {
        IAPManager.instance.BuyNoAds();
    }

    public void OnMoneyButtonClicked(string str)
    {
        switch (str)
        {
            case "10k":
                IAPManager.instance.BuyMoney10k();
                break;
            case "1m":
                IAPManager.instance.BuyMoney1m();
                break;
            case "10m":
                IAPManager.instance.BuyMoney10m();
                break;
            case "1b":
                IAPManager.instance.BuyMoney1b();
                break;
            case "100b":
                IAPManager.instance.BuyMoney100b();
                break;
        }
    }

    public void OnSpecialFuelButtonClicked(string str)
    {
        switch (str)
        {
            case "1":
                IAPManager.instance.BuySpecialFuel1();
                break;
            case "10":
                IAPManager.instance.BuySpecialFuel10();
                break;
            case "100":
                IAPManager.instance.BuySpecialFuel100();
                break;
        }
    }

    public void _Load()
    {
        if (IsForgeBoosted)
            dParent.transform.GetChild(0).gameObject.SetActive(false);
        if (IsGoldPriceBoosted)
            dParent.transform.GetChild(1).gameObject.SetActive(false);
        if (IsNoAds)
            dParent.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void WorkersAfkOre(long afkOre)
    {
        afkOreTemp = afkOre;
        LetterRounding lr = new LetterRounding();
        WorkersReport.SetActive(true);
        WorkersReport.transform.GetChild(1).gameObject.GetComponent<Text>().text = lr.Rounding(afkOre);
        WorkersReport.transform.GetChild(4).gameObject.GetComponent<Text>().text = lr.Rounding(afkOre / 10);
    }

    public void OnOreGetBTClicked()
    {
        core.Ore += afkOreTemp;
        WorkersReport.SetActive(false);
    }

    public void OnAdBTClicked()
    {
        AdsListener.RewardOre = afkOreTemp * 2;
        AdsListener.Reward = "x2";
        WorkersReport.SetActive(false);
        AdsManager.ShowRewardedVideo();
    }

    public void OnGetIngotsBTClicked(bool bought)
    {
        if (!bought)
        {
            IAPManager.instance.BuyGetIngots();
        }
        else
        {
            core.Gold += afkOreTemp / 10;
            WorkersReport.SetActive(false);
        }
    }
}
