using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BeforeTurn : MonoBehaviour
{
    [SerializeField] private int correctionMax = 10;
    [SerializeField] private int correctionMin = 10;
    [SerializeField] TMP_Text eventTitle;
    [SerializeField] TMP_Text eventContent;
    [SerializeField] TMP_Text eventChanges;
    private int maxRate;
    private int minRate;
    public (string, int) eventInfo = ("", 0);
    void Start()
    {
        // 오르<->마정석 환전 비율 변환
        if (GoodsManager.exchangeRate <= 300) maxRate += correctionMax;
        if (GoodsManager.exchangeRate >= 700) minRate -= correctionMin;
        GoodsManager.exchangePercent = UnityEngine.Random.Range(minRate, maxRate);
        maxRate = GoodsManager.maxRate;
        minRate = GoodsManager.minRate;

        // 이벤트 가져오기
        eventInfo = EventManager.GetEvent();
        if (eventInfo.Item2 == -1)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            List<string> eventList = new List<string>();
            eventList = EventManager.ShowEvent(eventInfo.Item1, eventInfo.Item2);
            eventTitle.text = eventList[0];
            eventContent.text = eventList[1];
            eventChanges.text = eventList[2];
        }

        // 이벤트 가져온 이후 실제 환전 비율에 따라 가격 변환
        GoodsManager.exchangeRate = GoodsManager.exchangeRate + (GoodsManager.exchangeRate * GoodsManager.exchangePercent / 100);


        // string temp = GenerateName(); //for testing

    }
}
