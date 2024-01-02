using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeTurn : MonoBehaviour
{
    [SerializeField] private int correctionMax = 10;
    [SerializeField] private int correctionMin = 10;
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


        // string temp = GenerateName(); //for testing
        
    }

    //이름 무작위 생성 코드
    //필요에 맞게 알아서 수정하면 됩니다
    public static string GenerateName()
    {
        StreamReader ReadEnglishName = new StreamReader("Assets\\Resources\\Names\\englishNames.csv");
        StreamReader ReadKoreanName = new StreamReader("Assets\\Resources\\Names\\koreanNames.csv");
        string line;
        string[] row;
        int randomLastNameIndex, randomFirstNameIndex;
        string RandomLastName, RandomFirstName, finalRandomName;
        List<string> lastName = new List<string>();
        List<string> firstName = new List<string>();
        while ((line = ReadEnglishName.ReadLine()) != null)
        {
            row = line.Split(',');
            if (row[0] != "")
            {
                lastName.Add(row[0]);
            }
            if (row[1] != "")
            {
            firstName.Add(row[1]);
            }
        }
        ReadEnglishName.Close();
        while ((line = ReadKoreanName.ReadLine()) != null)
        {
            row = line.Split(',');
            if (row[0] != "")
            {
                lastName.Add(row[0]);
            }
            if (row[1] != "")
            {
            firstName.Add(row[1]);
            }
        }
        ReadKoreanName.Close();
        System.Random randomGenerator = new System.Random();

        randomLastNameIndex = randomGenerator.Next(0, lastName.Count);
        RandomLastName = lastName[randomLastNameIndex];

        randomFirstNameIndex = randomGenerator.Next(0, firstName.Count);
        RandomFirstName = firstName[randomFirstNameIndex];

        finalRandomName = RandomLastName + " " + RandomFirstName;
        Debug.Log(finalRandomName);
        return finalRandomName;
    }
}
