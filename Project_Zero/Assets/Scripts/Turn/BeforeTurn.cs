using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeTurn : MonoBehaviour
{
    [SerializeField] private int correctionMax = 10;
    [SerializeField] private int correctionMin = 10;
    private int maxRate;
    private int minRate;
    void Start()
    {
        maxRate = GoodsManager.maxRate;
        minRate = GoodsManager.minRate;
        // 오르<->마정석 환전 비율 변환
        if (GoodsManager.exchangeRate <= 300) maxRate += correctionMax;
        if (GoodsManager.exchangeRate >= 700) minRate -= correctionMin;
        GoodsManager.exchangePercent = Random.Range(minRate, maxRate);
        GoodsManager.exchangeRate = GoodsManager.exchangeRate + (GoodsManager.exchangeRate * GoodsManager.exchangePercent / 100);


        // 이벤트 가져오기


        // DuringScene 불러오기
        SceneManager.LoadScene("DuringTurn");
    }
}
