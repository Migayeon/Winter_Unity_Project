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

    public const int UPGRADE_MIN_VAL = 50;
    public const int UPGRADE_MAX_VAL = 100;

    public System.Random randomseed = new System.Random();

    public static bool ProfessorCreateFirstTime; //static bool value storing if the current roll should be a 'first roll'
                                                 // -> player do not need to pay for a roll

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
                //implement results of away trip of professor
            }
        }

        //ManageProfessor.cs : Create new stat index & values for upgrade(강화)
        int[] randomindex = new int[2];
        int[] randomvalue = new int[2];

        //generate random stat indexes
        for (int i = 0; i < PlayerInfo.ProfessorList.Count; ++i)
        {
            List<int> templist = new List<int>();
            randomindex[0] = randomseed.Next(1, 7);
            randomindex[1] = randomindex[1];
            while (randomindex[1] == randomindex[0])
            {
                randomindex[1] = randomseed.Next(1, 7);
            }
            templist.Add(Math.Min(randomindex[0], randomindex[1])); //smaller index first
            templist.Add(Math.Max(randomindex[0], randomindex[1])); //larger index later
            PlayerInfo.UpgradeSkillIndex.Add(templist);
            templist.Clear();
        }

        //generate random upgrade values
        for (int i = 0; i < PlayerInfo.ProfessorList.Count; ++i)
        {
            List<int> templist = new List<int>();
            randomvalue[0] = randomseed.Next(UPGRADE_MIN_VAL, UPGRADE_MAX_VAL);
            randomvalue[1] = randomseed.Next(UPGRADE_MIN_VAL, UPGRADE_MAX_VAL);
            templist.Add(randomvalue[0]);
            templist.Add(randomvalue[1]);
            PlayerInfo.UpgradeSkillValue.Add(templist);
            templist.Clear();
        }

        // string temp = GenerateName(); //for testing


        ProfessorCreateFirstTime = true; //initialize as true (will get free roll)
    }
}
