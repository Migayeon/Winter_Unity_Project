using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class manaStoneGambleManager : MonoBehaviour
{
    public enum State
    {
        None,
        Ready,
        Playing,
        Selected,
        Die
    }
    public static double maxPower = 5;
    public static double nowPower = 0;
    public static double stopPower = maxPower;
    public static double selectedPower = 0;
    public static State isPlaying = State.None;
    [SerializeField]
    private GameObject startButtonObject, stopButtonObject, nowPowerDisplayObject, selectedPowerDisplayObject;
    [SerializeField]
    private TMP_Text nowPowerDisplay, selectedPowerDisplay;
    private System.Random rand = new System.Random();
    public void Awake()
    {
        selectedPowerDisplayObject.SetActive(false);
        nowPowerDisplayObject.SetActive(false);
        startButtonObject.SetActive(true);
        stopButtonObject.SetActive(false);
    }

    public void startGame()
    {
        nowPower = 0;
        stopPower = rand.NextDouble() * (maxPower - 0.5) + 0.5;
        isPlaying = State.Playing;
        selectedPowerDisplayObject.SetActive(false);
        nowPowerDisplayObject.SetActive(true);
        startButtonObject.SetActive(false);
        stopButtonObject.SetActive(true);
    }

    public void stop()
    {
        selectedPower = nowPower;
        selectedPowerDisplayObject.SetActive(false);
        selectedPowerDisplay.text = String.Concat("× ", String.Format("{0:0.000}", (Math.Round(nowPower * 1000) / 1000).ToString()));
    }

    public void FixedUpdate()
    {
        if (isPlaying == State.Playing || isPlaying == State.Selected)
        {
            nowPower += (double) rand.Next(0, 30) / 1000;
            float reverseLogPower = 1 - (float)(Math.Log(nowPower + 1) / Math.Log(maxPower + 1));

            nowPowerDisplay.fontSize = 36 + (int) (36 * (1 - reverseLogPower));
            if (nowPower >= stopPower)
            {
                nowPower = stopPower;
                stopButtonObject.SetActive(false);
                nowPowerDisplay.color = new Color(200, 255, 255);
                isPlaying = State.Die;
            }
            nowPowerDisplay.text = String.Concat("× ", String.Format("{0:0.000}", (Math.Round(nowPower * 1000) / 1000).ToString()));
            nowPowerDisplay.color = new Color(200, 255 * reverseLogPower, 255 * reverseLogPower);
        }
        else if (isPlaying == State.Die)
        {
            nowPowerDisplay.text = String.Concat("종료\n× ", String.Format("{0:0.000}", (Math.Round(nowPower * 1000) / 1000).ToString()));
        }
    }
}
