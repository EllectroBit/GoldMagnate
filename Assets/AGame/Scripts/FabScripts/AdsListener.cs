using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Assets.AGame.Scripts.FabScripts
{
    class AdsListener : IUnityAdsListener
    {
        private Core core;
        public static int RewardMoney;
        public static long RewardOre;
        public static string Reward;

        public AdsListener(Core core)
        {
            this.core = core;
        }

        public void OnUnityAdsDidError(string message)
        {
            
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            core.LoadAll();

            if(placementId == "rewardedVideo" && showResult == ShowResult.Finished)
            {
                if (Reward == "money")
                {
                    if (core.Money > RewardMoney)
                        core.Money += RewardMoney + (core.Money / 4);
                    else
                        core.Money += RewardMoney;
                }
                else if (Reward == "sf")
                {
                    ForgeManager.SpecialFuel++;
                }
                else if (Reward == "x2")
                {
                    core.Ore += RewardOre;
                }
            }
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            core._Save();
        }

        public void OnUnityAdsReady(string placementId)
        {
            
        }
    }
}
