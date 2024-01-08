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
        maxRate = GoodsManager.maxRate;
        minRate = GoodsManager.minRate;
        GoodsManager.exchangePercent = UnityEngine.Random.Range(minRate, maxRate);

        // 이벤트 가져오기
        eventInfo = EventManager.GetEvent();
        if (eventInfo.Item2 == -1)
        {
            eventTitle.text = "오늘은 평화로운 날입니다.";
            eventContent.text = "아무 일도 일어나지 않았습니다.";
            eventChanges.text = "";
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
        GoodsManager.exchangeRate = GoodsManager.exchangeRate + (int)((double)GoodsManager.exchangeRate * GoodsManager.exchangePercent / 100);

        for (int i = 0; i < PlayerInfo.ProfessorCount(); ++i)
        {
            if (PlayerInfo.ProfessorList[i].ProfessorGetAwayTime() > 1)
            {
                PlayerInfo.ProfessorList[i].ProfessorChangeAwayTime();
            }
            else if (PlayerInfo.ProfessorList[i].ProfessorGetAwayTime() == 1)
            {
                PlayerInfo.ProfessorList[i].ProfessorChangeAwayTime();
                PlayerInfo.ProfessorList[i].ProfessorChangeAwayStatus(false);
            }
        }

        // string temp = GenerateName(); //for testing

    }
}
