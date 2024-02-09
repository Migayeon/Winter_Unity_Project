using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
    [HideInInspector]
    public int betAr = 0;
    [HideInInspector]
    public bool isCalculated;
    public static double maxPower = 12, minPower = 0.7;
    public static double nowPower = 0;
    public static double stopPower = maxPower;
    public double selectedPower = 0;
    public static State isPlaying = State.None;
    [SerializeField]
    private Transform gotoMenuBtn;
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
        gotoMenuBtn.gameObject.SetActive(true);
        isPlaying = State.None;
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
        gotoMenuBtn.gameObject.SetActive(true);
    }

    private double fastPow(double x, int a)
    {
        if (a == 0) return 1;
        else if (a == 1) return x;
        else
        {
            double tmp = fastPow(x, a / 2);
            if (a % 2 == 1) return tmp * tmp * x;
            return tmp * tmp;
        }
    }

    public double betaIntFunc(double x)
    {
        // alpha = 2, beta = 5 인 베타함수를 0부터 x까지 적분한 값
        // 기댓값 : 2/7, 분산 : 0.1
        double tmp = fastPow(1 - x, 5);
        return 5 * tmp * (1 - x) - 6 * tmp + 1;
    }

    public double getBetaRandom()
    {
        double y = rand.NextDouble();
        double left = 0;
        double right = 1;
        double x;
        while (true)
        {
            x = (left + right) / 2;
            double tmp = betaIntFunc(x);
            if (tmp < y) left = x;
            else right = x;
            if (Math.Abs(y - betaIntFunc(x)) < 0.001) break;
        }
        return x * (maxPower - minPower) + minPower;
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
            nowPower = 0;
            selectedPower = 0;
            stopPower = getBetaRandom();
            isPlaying = State.Playing;
            selectedPowerDisplayObject.SetActive(false);
            nowPowerDisplayObject.SetActive(true);
            startButtonObject.SetActive(false);
            restartButtonObject.SetActive(false);
            stopButtonObject.SetActive(true);
            betInputField.SetActive(false);
            gotoMenuBtn.gameObject.SetActive(false);
            revenueDisplayObject.SetActive(false);
        }
    }

    public void stop()
    {
        selectedPower = nowPower;
        stopButtonObject.SetActive(false);
        if (rand.NextDouble() <= increasePercent / 100)
            stopPower = Math.Min(stopPower * 1.2, maxPower);
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
            betAr = GoodsManager.goodsAr;
            if (GoodsManager.goodsAr > 0)
            {
                betInputField.GetComponent<TMP_InputField>().text = GoodsManager.goodsAr.ToString();
            }
            else
            {
                betAr = 0;
                betInputField.GetComponent<TMP_InputField>().text = "0";
            }
        }
    }


    public void FixedUpdate()
    {
        if (isPlaying == State.Playing || isPlaying == State.Selected)
        {
            nowPower += (double) rand.Next(0, 30) / 1000;
            float reverseLogPower = 1 - (float)(Math.Log(nowPower + 1) / Math.Log(maxPower + 1));
            nowPowerDisplay.color = new Color(1, reverseLogPower, reverseLogPower);
            nowPowerDisplay.fontSize = 36 + (int) (36 * (1 - reverseLogPower));
            if (nowPower >= stopPower)
            {
                nowPower = stopPower;
                stopButtonObject.SetActive(false);
                nowPowerDisplay.color = new Color(1, 1, 1);
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
                selectedPowerDisplay.color = new Color(1, 1, 1);
                selectedPowerDisplay.text = "× 0";
            }

            // 정산 타임
            if (!isCalculated)
            {
                int revenueInThisRound = (int)(betAr * selectedPower * 0.5);
                GoodsManager.goodsAr += revenueInThisRound;
                revenueDisplayObject.SetActive(true);
                revenueDisplayObject.GetComponent<TMP_Text>().text = $"얻은 수익: {revenueInThisRound}";
                if (selectedPower == 0)
                {
                    const int ACHIEVEMNET_ID = 10;
                    AchievementManager.CreateLocalStat(ACHIEVEMNET_ID, 0);
                    AchievementManager.localStat[ACHIEVEMNET_ID] += betAr;
                    if (AchievementManager.localStat[ACHIEVEMNET_ID] >= 100000)
                        AchievementManager.Achieve(ACHIEVEMNET_ID);
                }
                isCalculated = true;
            }
        }
    }
}
