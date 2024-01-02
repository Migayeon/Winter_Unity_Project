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
    [HideInInspector] public int betAr = 0;
    [HideInInspector] public bool isCalculated;
    public static double maxPower = 13, minPower = 0.7;
    public static double nowPower = 0;
    public static double stopPower = maxPower;
    [SerializeField] public double selectedPower = 0;
    public static State isPlaying = State.None;
    [SerializeField]
    private double increasePercent;
    [SerializeField]
    private GameObject startButtonObject, stopButtonObject, restartButtonObject,
        nowPowerDisplayObject, selectedPowerDisplayObject, revenueDisplayObject, startWarningDisplayObject, betInputField;
    [SerializeField]
    private TMP_Text nowPowerDisplay, selectedPowerDisplay;
    private System.Random rand = new System.Random();
    public void Awake()
    {
        isCalculated = false;
        betAr = 0;
        selectedPowerDisplayObject.SetActive(false);
        nowPowerDisplayObject.SetActive(false);
        startButtonObject.SetActive(true);
        restartButtonObject.SetActive(false);
        stopButtonObject.SetActive(false);
        betInputField.SetActive(true);
        revenueDisplayObject.SetActive(false);
        startWarningDisplayObject.SetActive(false);
    }

    public void resetGame()
    {
        betInputField.GetComponent<TMP_InputField>().text = "";
        isCalculated = false;
        betAr = 0;
        nowPower = 0;
        stopPower = maxPower;
        selectedPower = 0;
        isPlaying = State.None;
        selectedPowerDisplayObject.SetActive(false);
        nowPowerDisplayObject.SetActive(false);
        startButtonObject.SetActive(true);
        restartButtonObject.SetActive(false);
        stopButtonObject.SetActive(false);
        betInputField.SetActive(true);
        revenueDisplayObject.SetActive(false); 
    }

    public void startGame()
    {
        if (betAr <= 0)
        {
            startWarningDisplayObject.SetActive(true);
        }
        else
        {
            isCalculated = false;
            GoodsManager.goodsAr -= betAr;
            betAr = (int)(betAr * 0.5);
            nowPower = 0;
            selectedPower = 0;
            stopPower = rand.NextDouble() * (maxPower - minPower) + minPower;
            isPlaying = State.Playing;
            selectedPowerDisplayObject.SetActive(false);
            nowPowerDisplayObject.SetActive(true);
            startButtonObject.SetActive(false);
            restartButtonObject.SetActive(false);
            stopButtonObject.SetActive(true);
            betInputField.SetActive(false);
            revenueDisplayObject.SetActive(false);
        }
    }

    public void stop()
    {
        selectedPower = nowPower;
        stopButtonObject.SetActive(false);
        if (rand.NextDouble() < increasePercent / 100)
            stopPower = Math.Max(stopPower * 1.2, maxPower);
        isPlaying = State.Selected;
        selectedPowerDisplayObject.SetActive(true);
        selectedPowerDisplay.text = String.Concat("× ", String.Format("{0:0.000}", (Math.Round(nowPower * 1000) / 1000).ToString()));
    }
    public void UpdateBetAr(string amount)
    {
        try
        {
            betAr = Convert.ToInt32(amount);
        }
        catch 
        {
            if (amount.Length <= 0) betAr = 0;
        }
        if (betAr > GoodsManager.goodsAr)
        {
            betAr = (int)(GoodsManager.goodsAr);
            betInputField.GetComponent<TMP_InputField>().text = GoodsManager.goodsAr.ToString();
        }
        
    }


    public void FixedUpdate()
    {
        if (isPlaying == State.Playing || isPlaying == State.Selected)
        {
            nowPower += (double) rand.Next(0, 30) / 1000;
            float reverseLogPower = 1 - (float)(Math.Log(nowPower + 1) / Math.Log(maxPower + 1));
            nowPowerDisplay.color = new Color(200, 255 * reverseLogPower, 255 * reverseLogPower);
            nowPowerDisplay.fontSize = 36 + (int) (36 * (1 - reverseLogPower));
            if (nowPower >= stopPower)
            {
                nowPower = stopPower;
                stopButtonObject.SetActive(false);
                nowPowerDisplay.color = new Color(200, 255, 255);
                isPlaying = State.Die;
            }
            nowPowerDisplay.text = String.Concat("× ", String.Format("{0:0.000}", (Math.Round(nowPower * 1000) / 1000).ToString()));
        }
        else if (isPlaying == State.Die)
        {
            nowPowerDisplay.text = String.Concat("종료\n× ", String.Format("{0:0.000}", (Math.Round(nowPower * 1000) / 1000).ToString()));
            restartButtonObject.SetActive(true);
            if (selectedPower < minPower)
            {
                selectedPowerDisplayObject.SetActive(true);
                selectedPowerDisplay.color = new Color(200, 255, 255);
                selectedPowerDisplay.text = "× 0";
            }

            // 정산 타임
            if (!isCalculated)
            {
                int revenueInThisRound = (int)(betAr * selectedPower);
                GoodsManager.goodsAr += revenueInThisRound;
                revenueDisplayObject.SetActive(true);
                revenueDisplayObject.GetComponent<TMP_Text>().text = $"얻은 수익: {revenueInThisRound}";
                isCalculated = true;
            }
        }
    }
}
