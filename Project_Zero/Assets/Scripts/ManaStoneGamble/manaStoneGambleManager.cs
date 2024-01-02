using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class manaStoneGambleManager : MonoBehaviour
{
    public static double maxPower = 3;
    public static double nowPower = 0;
    public static double stopPower = maxPower;
    public static double selectedPower = 0;
    public static bool isPlaying = true;

    public void startGame()
    {
        nowPower = 0;
        System.Random rand = new System.Random();
        stopPower = rand.NextDouble() * (maxPower - 0.5) + 0.5;
        isPlaying = true;
    }

    public void stop()
    {
        selectedPower = nowPower;
    }

    public void FixedUpdate()
    {
        if (nowPower < stopPower && isPlaying)
        {
            nowPower += 0.01;
        }
        else
        {
            nowPower = maxPower;
            isPlaying = false;
        }        
    }
}
