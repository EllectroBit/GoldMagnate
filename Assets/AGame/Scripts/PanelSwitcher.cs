using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject mine;
    public GameObject workers;
    public GameObject forge;
    public GameObject city;
    [Space]
    public GameObject PanelToLerp_Settings;
    public GameObject PanelToLerp_Shop; // another panel
    [Space]
    private UnityEngine.Vector2 Settings_start;
    private UnityEngine.Vector2 Settings_end;
    private UnityEngine.Vector2 Shop_start; // to another panel
    private UnityEngine.Vector2 Shop_end; // to another panel
    [Space]
    public float step;

    private float Settings_progress;
    private float Shop_progress; // to another panel

    private bool isSettingsON = false;
    private bool isShopON = false; // to another panel

    private void Start()
    {
        Settings_start = PanelToLerp_Settings.transform.position;
        Settings_end = Settings_start;

        Shop_start = PanelToLerp_Shop.transform.position;
        Shop_end = Shop_start;
        //step = 0.15f;
    }

    private void FixedUpdate()
    {
        PanelToLerp_Settings.transform.position = UnityEngine.Vector2.Lerp(Settings_start, Settings_end, Settings_progress);
        Settings_progress += step;

        PanelToLerp_Shop.transform.position = UnityEngine.Vector2.Lerp(Shop_start, Shop_end, Shop_progress);
        Shop_progress += step;
    }

    public void SwitchPanel(string panel)
    {
        switch (panel)
        {
            case "mine":
                mine.SetActive(!mine.activeSelf);
                break;
            case "workers":
                workers.SetActive(!workers.activeSelf);
                break;
            case "forge":
                forge.SetActive(!forge.activeSelf);
                break;
            case "city":
                city.SetActive(!city.activeSelf);
                break;
            default: break;
        }
    }

    public void OnSettingsButtonCliked()
    {
        Settings_progress = 0;
        if (isSettingsON)
        {
            isSettingsON = false;
            Settings_start = PanelToLerp_Settings.transform.position;
            Settings_end.y += 8.1f;
        }
        else
        {
            isSettingsON = true;
            Settings_start = PanelToLerp_Settings.transform.position;
            Settings_end.y -= 8.1f;
        }
    }

    public void OnShopButtonCliked()
    {
        Shop_progress = 0;
        if (isShopON)
        {
            isShopON = false;
            Shop_start = PanelToLerp_Shop.transform.position;
            Shop_end.y += 8.1f;
        }
        else
        {
            isShopON = true;
            Shop_start = PanelToLerp_Shop.transform.position;
            Shop_end.y -= 8.1f;
        }
    }
}
