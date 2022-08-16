using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EDeveloper
{
    MinghuiYu
}
public static class RainbowLog
{
    static EDeveloper developer = EDeveloper.MinghuiYu;

    public static void ColorLog(string LogMsg)
    {
        switch (developer)
        {
            case EDeveloper.MinghuiYu:
                Debug.Log("<color=#00B6FFD8>"+ developer.ToString()+"::    " + LogMsg + "</color>");
                break;
        }
    }
    public static void ColorLogWarning(string LogMsg)
    {
        switch (developer)
        {
            case EDeveloper.MinghuiYu:
                Debug.LogWarning("<color=#00B6FFD8>" + developer.ToString() + "::    " + LogMsg + "</color>");
                break;
        }
    }
    public static void ColorLogError(string LogMsg)
    {
        switch (developer)
        {
            case EDeveloper.MinghuiYu:
                Debug.LogError("<color=#00B6FFD8>" + developer.ToString() + "::    " + LogMsg + "</color>");
                break;
        }
    }
}
