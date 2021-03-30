using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GPS_Manager : MonoBehaviour
{
    [SerializeField] private string[] achiv;
    [Space]
    public Core core;
    public CityManager cityManager;

    private const string leaderboard = "CgkI6v3Oue4dEAIQAQ";

    void Start()
    {
        PlayGamesPlatform.Activate();
        if(!Social.localUser.authenticated)
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {

                }
                else
                {

                }
            });
    }

    private void Update()
    {
        CheckAchiv();
        Social.ReportScore(core.Money, leaderboard, (bool success) => { });
    }

    public void GetAchiv(int index)
    {
        Social.ReportProgress(achiv[index], 100.0f, (bool success) => {  });
    }

    private void CheckAchiv()
    {
        if (core.Money > 0)
            GetAchiv(0);
        if (core.Money >= 1000000)
            GetAchiv(1);
        if (core.Money >= 1000000000)
            GetAchiv(2);
        if (core.Money >= 15000000000)
            GetAchiv(3);
        if (core.Money >= 15000000000)
            GetAchiv(3);
        if (cityManager.employeeScript[0].isBought)
            GetAchiv(4);
        if (cityManager.employeeScript[8].isBought)
            GetAchiv(5);
    }

    public void ShowAchiv()
    {
        Social.ShowAchievementsUI();
    }

    public void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }
}
