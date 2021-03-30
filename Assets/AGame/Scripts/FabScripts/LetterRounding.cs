using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterRounding
{
    public string Rounding(long value)
    {
        if (value > 999999999)
            return (Math.Round((double)value / 100000000.0f) + "B");
        else if (value > 999999)
            return (Math.Round((double)value / 100000.0f) + "M");
        else if (value > 999)
            return (Math.Round((double)value / 1000.0f) + "k");
        else
            return value.ToString();
    }

    public string DateRounding(long value)
    {
        string res = "";

        int temp1 = (int)(value / 60 / 60);
        int temp2 = (int)(value / 60 % 60);
        int temp3 = (int)(value  % 60);

        res += DR(temp1) + ":";
        res += DR(temp2) + ":";
        res += DR(temp3);

        return res;
    }

    private string DR(int i)
    {
        if (i < 10)
            return $"0{i}";
        else
            return i.ToString();
    }
}
