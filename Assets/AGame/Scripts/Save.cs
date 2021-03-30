using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    public Save(int workers, int employees, int forges)
    {
        LVL = new int[employees];
        currentEnergy = new int[employees];
        E_maxXP = new long[employees];
        E_currentXP = new long[employees];
        isBought = new bool[employees];

        goldPerSec = new long[workers];
        goldPerSecPerLVL = new long[workers];
        W_LVL = new int[workers];
        upgradeCost = new long[workers];
        currentUpgradeCost = new long[workers];
        upgradeMult = new int[workers];
        W_isBought = new bool[workers];

        forgering = new long[forges];
        MaxIngots = new int[forges];
        isReady = new bool[forges];
        isUpgrading = new bool[forges];
        countOfIngots = new int[forges];
        UpgradeCost = new long[forges];
        UpgradeLVL = new int[forges];
        UpgradeTime = new int[forges];

        date = new int[6];
    }
    //Core
    public long Money;
    public long Ore;
    public long Gold;
    public float music;
    public int[] date;

    //Mine
    public int counter;
    public int UpgradeCounter;
    public long OrePerClick;

    //Forge
    public int[] MaxIngots;
    public long[] forgering;
    public bool[] isReady;
    public bool[] isUpgrading;
    public bool[] F_isBought;
    public int[] countOfIngots;
    public long[] UpgradeCost;
    public int[] UpgradeLVL;
    public int[] UpgradeTime;
    public int SpecialFuel;

    //CityManager
    public int SaleLVL;
    public int ingotsToSell;
    public long tempXP;
    public long currentXP;
    public long maxXP;
    public int possibilityToDrop;
    public long XPcounter;
    //public EmployeeScript[] employeeScript;

    //Employees
    public int[] LVL;
    public int[] currentEnergy;
    public long[] E_maxXP;
    public long[] E_currentXP;
    public bool[] isBought;

    //WorkerManager
    //private WorkerScript[] workerScript;

    //Workers
    public long[] goldPerSec;
    public long[] goldPerSecPerLVL;
    public int[] W_LVL;
    public long[] upgradeCost;
    public long[] currentUpgradeCost;
    public int[] upgradeMult;
    public bool[] W_isBought;
}