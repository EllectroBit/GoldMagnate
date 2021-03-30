using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Assets.AGame.Scripts.FabScripts;

public class AdsManager : MonoBehaviour
{
    private Core core;
    public DonateManager donateManager;

    void Start()
    {
        core = GameObject.FindGameObjectWithTag("MainScript").gameObject.GetComponent<Core>();

        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("3718277", false);
            Advertisement.AddListener(new AdsListener(core));
        }

        StartCoroutine(Ads());
    }

    public void ShowVideo()
    {
        if (Advertisement.IsReady() && !donateManager.IsNoAds)
        {
            Advertisement.Show("video");
        }
    }

    public static bool ShowRewardedVideo()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo");
            return true;
        }
        else
            return false;
    }

    IEnumerator Ads()
    {
        while (true)
        {
            yield return new WaitForSeconds(310);
            ShowVideo();
        }
    }

    public void OnAddMoneyClicked(int money)
    {
        AdsListener.RewardMoney = money;
        AdsListener.Reward = "money";
        ShowRewardedVideo();
    }

    public void OnAddSpecialFuelClicked()
    {
        AdsListener.Reward = "sf";
        ShowRewardedVideo();
    }
}
