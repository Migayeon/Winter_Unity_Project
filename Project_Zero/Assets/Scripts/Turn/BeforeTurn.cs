using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeforeTurn : MonoBehaviour
{
    [SerializeField] private int correctionMax = 10;
    [SerializeField] private int correctionMin = 10;
    [SerializeField] GameObject eventImage;
    private int maxRate;
    private int minRate;
    public (string, int) eventInfo = ("", 0);
    void Start()
    {
        maxRate = GoodsManager.maxRate;
        minRate = GoodsManager.minRate;
        // 오르<->마정석 환전 비율 변환
        if (GoodsManager.exchangeRate <= 300) maxRate += correctionMax;
        if (GoodsManager.exchangeRate >= 700) minRate -= correctionMin;
        GoodsManager.exchangePercent = UnityEngine.Random.Range(minRate, maxRate);
        GoodsManager.exchangeRate = GoodsManager.exchangeRate + (GoodsManager.exchangeRate * GoodsManager.exchangePercent / 100);


        // 이벤트 가져오기
        eventInfo = EventManager.GetEvent();
        if (eventInfo.Item2 == -1)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            eventImage.SetActive(true);
            eventImage.GetComponent<Image>().sprite = EventManager.ShowEvent(eventInfo.Item1, eventInfo.Item2);
        }
        // string temp = GenerateName(); //for testing

    }
}
