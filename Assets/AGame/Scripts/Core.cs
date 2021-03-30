using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    private const int FLY_OBJ = 15;

    public bool NewGame;
    [Space]
    public long Ore = 0;
    public long Gold = 0;
    public long Money = 0;
    [Space]
    public GameObject g1;
    public GameObject g2;
    public GameObject g3;
    public GameObject clickObjPrefab;
    public GameObject clickPerent_Gold;
    public GameObject clickPerent_Money;
    public Color color;
    public MusicControl musicControl;
    public GameObject infoPanel;

    private bool gTemp = true;

    public Save save { get; set; }
    private WorkersManager workersManager;
    private CityManager cityManager;
    private MineManager mineManager;
    private ForgeManager forgeManager;
    private NotificationsScript notifications;
    private ClickObjectFly[] coPool_Gold = new ClickObjectFly[FLY_OBJ];
    private ClickObjectFly[] coPool_Money = new ClickObjectFly[FLY_OBJ];
    private GreenRed[] gr_Gold = new GreenRed[FLY_OBJ];
    private GreenRed[] gr_Money = new GreenRed[FLY_OBJ];
    public Save load { get; set; }

    private byte Obj_GoldCounter;
    private byte Obj_MoneyCounter;
    private long goldTemp;
    private long moneyTemp;

    private void Awake()
    {
        load = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SAVE"));
    }

    void Start()
    {
        g2.SetActive(true);
        g3.SetActive(true);
        g1.SetActive(true);

        workersManager = transform.parent.transform.gameObject.transform.GetChild(5).gameObject.GetComponent<WorkersManager>();
        cityManager = transform.parent.transform.gameObject.transform.GetChild(4).gameObject.GetComponent<CityManager>();
        mineManager = transform.parent.transform.gameObject.transform.GetChild(2).gameObject.GetComponent<MineManager>();
        forgeManager = transform.parent.transform.gameObject.transform.GetChild(3).gameObject.GetComponent<ForgeManager>();
        notifications = transform.parent.transform.GetChild(6).gameObject.GetComponent<NotificationsScript>();

        goldTemp = Gold;
        moneyTemp = Money;

        for (int i = 0; i < FLY_OBJ; i++)
        {
            coPool_Gold[i] = Instantiate(clickObjPrefab, clickPerent_Gold.transform).GetComponent<ClickObjectFly>();
            gr_Gold[i] = coPool_Gold[i].GetComponent<GreenRed>();
            coPool_Gold[i].GetComponent<Text>().color = color;
            coPool_Money[i] = Instantiate(clickObjPrefab, clickPerent_Money.transform).GetComponent<ClickObjectFly>();
            gr_Money[i] = coPool_Money[i].GetComponent<GreenRed>();
            coPool_Money[i].GetComponent<Text>().color = color;
        }

        _Load();
        StartCoroutine(AutoSave());
    }

    private void Update()
    {
        if (gTemp)
        {
            g1.SetActive(false);
            g2.SetActive(false);
            g3.SetActive(false);
            gTemp = false;
        }

        FlyObj();
    }

    public void _Save()
    {
        save = new Save(workersManager.countOfLines, cityManager.EmployeesCount, forgeManager.size);

        save.Money = Money;
        save.Gold = Gold;
        save.Ore = Ore;

        save = mineManager._Save(save);
        save = forgeManager._Save(save);
        save = cityManager._Save(save);

        for (int i = 0; i < workersManager.countOfLines; i++)
            save = workersManager.workerScript[i]._Save(save);
        for (int i = 0; i < cityManager.EmployeesCount; i++)
            save = cityManager.employeeScript[i]._Save(save);

        save.music = musicControl.current;

        save.date[0] = DateTime.Now.Year;
        save.date[1] = DateTime.Now.Month;
        save.date[2] = DateTime.Now.Day;
        save.date[3] = DateTime.Now.Hour;
        save.date[4] = DateTime.Now.Minute;
        save.date[5] = DateTime.Now.Second;

        PlayerPrefs.SetString("SAVE", JsonUtility.ToJson(save));
    }

    public void LoadAll() //for ads
    {
        load = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SAVE"));
        _Load();
        forgeManager._Load();
        workersManager.needToLoad = true;
    }

    private void _Load()
    {
        if (!NewGame && PlayerPrefs.HasKey("SAVE"))
        {
            Money = load.Money;
            Gold = load.Gold;
            Ore = load.Ore;
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            _Save();
            SetNotification();
        }
    }
#else

    private void OnApplicationQuit()
    {
        _Save();
    }
#endif

    private void SetNotification()
    {
        for(int i = 0; i < forgeManager.size; i++)
        {   
            if(forgeManager.UpgradeTime[i] > 5 && forgeManager.isUpgrading[i])
            {
                notifications.CreateNotificationU("Upgrading Done", $"{i + 1} forge is upgraded", forgeManager.UpgradeTime[i]);
            }
            else if (forgeManager.forgering[i] > 5 && !forgeManager.isReady[i])
            {
                notifications.CreateNotificationF("Forgering Done", $"{i + 1} forge is done", forgeManager.forgering[i]);
            }
        }
    }

    private void FlyObj()
    {
        int y = -1;
        int x = 0;

        if (Gold < goldTemp)
        {
            if (Obj_GoldCounter < coPool_Gold.Length)
            {
                gr_Gold[Obj_GoldCounter].SetColor(false);
                coPool_Gold[Obj_GoldCounter].StartMotion(x, y, $"-{goldTemp - Gold}");
            }
            else
                Obj_GoldCounter = 0;
            Obj_GoldCounter++;
        }
        else if(Gold > goldTemp)
        {
            if (Obj_GoldCounter < coPool_Gold.Length)
            {
                gr_Gold[Obj_GoldCounter].SetColor(true);
                coPool_Gold[Obj_GoldCounter].StartMotion(x, y, $"+{Gold - goldTemp}");
            }
            else
                Obj_GoldCounter = 0;
            Obj_GoldCounter++;
        }

        if(Money < moneyTemp)
        {
            if (Obj_MoneyCounter < coPool_Money.Length)
            {
                gr_Money[Obj_MoneyCounter].SetColor(false);
                coPool_Money[Obj_MoneyCounter].StartMotion(x, y, $"-{moneyTemp - Money}");
            }
            else
                Obj_MoneyCounter = 0;
            Obj_MoneyCounter++;
        }
        else if(Money > moneyTemp)
        {
            if (Obj_MoneyCounter < coPool_Money.Length)
            {
                gr_Money[Obj_MoneyCounter].SetColor(true);
                coPool_Money[Obj_MoneyCounter].StartMotion(x, y, $"+{Money - moneyTemp}");
            }
            else
                Obj_MoneyCounter = 0;
            Obj_MoneyCounter++;
        }

        moneyTemp = Money;
        goldTemp = Gold;
    }

    private void ShowFullRes(string str)
    {
        Text text = infoPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        switch (str)
        {
            case "ore":
                text.text = Ore.ToString();
                break;
            case "gold":
                text.text = Gold.ToString();
                break;
            case "money":
                text.text = Money.ToString();
                break;
            default: break;
        }
    }

    public void OnInfoDown(string str)
    {
        infoPanel.SetActive(true);
        ShowFullRes(str);
    }

    public void OnInfoUP()
    {
        infoPanel.SetActive(false);
    }

    public IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            _Save();
        }
    }
}
