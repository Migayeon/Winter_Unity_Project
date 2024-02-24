/*
DOCUMENTATION

<Constants>
- NoneProbability, CommonProbability, RareProbability, UniqueProbability refers to the probability of each type of event.
- totalProbability is the sum of the above four constants.

- CommonCaseCount, RareCaseCount, UniqueCaseCount refers to the number of events that exist for each rarity type.


<Functions>
GetEvent()
- This function, when called, calculates a random event based on rarity.
- returns a tuple of a string and an integer.
- the return string is  "common", "rare", or "unique", and refers to the rarity of the event that was randomly selected.
- The return integer is the EventID of the event that was randomly selected.

SetEvent()
- This function, when called, changes the value associated to an EventID.
- returns void.
- Accepts a string and two integers as parameters.
- The string *MUST* be one of the three: "common", "rare", or "unique". (NO EXCEPTION HANDLING IMPLEMENTED)
- The first integer is the EventID that you wish to change.
- The second integer is the amount you want to change the current value by.



IMPORTANT: DO NOT DETELE THE (0,0) AT THE TOPMOST LINES OF THE CSV FILES
중요: CSV 파일들의 첫 번째 행에 있는 (0,0) 데이터를 삭제하지 말아주세요!
*/

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics.Tracing;
using Unity.VisualScripting.Antlr3.Runtime;

public class EventManager : MonoBehaviour
{
    //you can edit these constants before compile


    public const int NoneProbability = 0;   // 65
    public const int CommonProbability = 20;
    public const int RareProbability = 10;
    public const int UniqueProbability = 5;
    public const int totalProbability = NoneProbability + CommonProbability + RareProbability + UniqueProbability;

    public const int CommonCaseCount = 6;
    public const int RareCaseCount = 6;
    public const int UniqueCaseCount = 6;
    public static (string, int) GetEvent()
    {
        System.Random generateRandom = new System.Random();
        string[] filenames = new string[] { "Assets/Resources/Events/Common.csv", "Assets/Resources/Events/Rare.csv", "Assets/Resources/Events/Unique.csv" }; //enter filenames here
        string[] rarityTypes = new string[] { "common", "rare", "unique" };
        int SelectRandomEvent(int rarity, int casecount, int[,] datalist)
        {
            
            List<int> cases = new List<int>(casecount);
            for (int i = 0; i < casecount; ++i)
            {
                for (int j = 0; j < datalist[i, 1]; ++j)
                {
                    cases.Add(datalist[i, 0]);
                }
            }
            int returnValueIndex = generateRandom.Next(0, casecount);
            return cases[returnValueIndex];
        }

        (string, int) returnValue;
        string FileName;
        bool OpenFileFlag;

        List<int> probability = new List<int>(totalProbability);

        int randnum = generateRandom.Next(0, totalProbability);

        for (int i = 0; i < NoneProbability; ++i)
        {
            probability.Add(0);
        }
        for (int i = 0; i < CommonProbability; ++i)
        {
            probability.Add(1);
        }
        for (int i = 0; i < RareProbability; ++i)
        {
            probability.Add(2);
        }
        for (int i = 0; i < UniqueProbability; ++i)
        {
            probability.Add(3);
        }

        int rarity = probability[randnum];

        if (rarity == 0)
        {
            FileName = null;
            OpenFileFlag = false;
        }
        else
        {
            FileName = filenames[rarity-1];
            OpenFileFlag = true;
        }

        if (OpenFileFlag)
        {
            StreamReader sr = new StreamReader(FileName);
            string line;
            string[] row;
            int[,] datalist = new int[30, 2];
            int idx = 0;
            sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                row = line.Split(',');
                datalist[idx, 0] = Convert.ToInt32(row[0]);
                datalist[idx, 1] = Convert.ToInt32(row[1]);
                idx++;
            }
            sr.Close();

            if (rarity == 1)
            {
                returnValue = (rarityTypes[rarity - 1], SelectRandomEvent(rarity, CommonCaseCount, datalist));
            }
            else if (rarity == 2)
            {
                returnValue = (rarityTypes[rarity - 1], SelectRandomEvent(rarity, RareCaseCount, datalist));
            }
            else
            {
                returnValue = (rarityTypes[rarity - 1], SelectRandomEvent(rarity, UniqueCaseCount, datalist));
            }
        }
        else
        {
            returnValue = ("none", -1);
        }
        //code for testing output
        Debug.Log(FileName);
        Debug.Log(returnValue);
        return returnValue;
    }

    public static void SetEvent(string rarity, int EventID, int changeValue)
    {
        int rarityIndex;
        string[] filenames = new string[] { "Assets\\Resources\\Events\\Common.csv", "Assets\\Resources\\Events\\Rare.csv", "Assets\\Resources\\Events\\Unique.csv" }; //enter filenames here
        int[] filelength = new int[] { CommonCaseCount, RareCaseCount, UniqueCaseCount };
        bool FoundEventID = false;
        if (rarity == "common")
        {
            rarityIndex = 0;
        }
        else if (rarity == "rare")
        {
            rarityIndex = 1;
        }
        else
        {
            rarityIndex = 2;
        }

        StreamReader sr = new StreamReader(filenames[rarityIndex]);
        string line;
        string[] row;
        int[,] datalist = new int[30, 2];
        int idx = 0;
        sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
            row = line.Split(',');
            datalist[idx, 0] = Convert.ToInt32(row[0]);
            datalist[idx, 1] = Convert.ToInt32(row[1]);
            idx++;
        }
        sr.Close();
            
        for (int j = 0; j < filelength[rarityIndex]; ++j)
        {
            if (datalist[j, 0] == EventID)
            {
                //code for testing
                Debug.Log("EventID found");
                Debug.Log(filenames[rarityIndex]);
                Debug.Log("<Original CSV Data>");
                for (int k = 0; k < filelength[rarityIndex]; ++k)
                {
                    Debug.Log(datalist[k, 0]);
                    Debug.Log(datalist[k, 1]);
                }
                //end

                FoundEventID = true;
                if (datalist[j, 1] + changeValue > 0)
                {
                    datalist[j, 1] += changeValue;
                }
                //code for testing
                Debug.Log("<New CSV Data>");
                for (int k = 0; k < filelength[rarityIndex]; ++k)
                {
                    Debug.Log(datalist[k, 0]);
                    Debug.Log(datalist[k, 1]);
                }
                //end
                break;

            }
        }
        if (FoundEventID)
        {
            StreamWriter sw = new StreamWriter(filenames[rarityIndex]);
            sw.WriteLine("0,0");
            for (int j = 0; j < filelength[rarityIndex]; ++j)
            {
                sw.WriteLine(Convert.ToString(datalist[j, 0]) + "," + Convert.ToString(datalist[j, 1]));
            }
            sw.Close();
        }
        //code for testing output
        
    }

    // Code for showing appropriate Event Sprites
    // return: Title & Content
    [System.Serializable]
    public class EventInfo
    {
        public string title;
        public string content;
        public int effectFormula;
        public int effectType;
        public List<int> effectAmount;
        public EventInfo()
        {
            title = "";
            content = "";
            effectFormula = 0;
            effectType = 0;
            effectAmount = new List<int>();
        }
    }

    public static List<string> ShowEvent(string rarity, int eventID)
    {
        EventInfo eventInfo = new EventInfo();
        List<string> returnList=new List<string>();
        string filePath = "";
        string[] fileNames = new string[] { "Events/Text/Common/",
            "Events/Text/Rare/", "Events/Text/Unique/" }; //enter filenames here
        int rarityIndex;
        if (rarity == "common") rarityIndex = 0;
        else if (rarity == "rare") rarityIndex = 1;
        else rarityIndex = 2;
        filePath = fileNames[rarityIndex];
        // json파일 받아오는거 + 효과 적용 구현해야함
        var loadJson = Resources.Load<TextAsset>(filePath + eventID.ToString());
        eventInfo = JsonUtility.FromJson<EventInfo>(loadJson.ToString());
        returnList.Add(eventInfo.title);
        returnList.Add(eventInfo.content);
        returnList.Add(EventChanges(eventInfo));
        return returnList;
    }
    // 이벤트 적용시키는 함수(나중에 수정할 예정)
    private static string EventChanges(EventInfo info)
    {
        string returnString = "";
        string type = "";
        string formula = "";
        string amount = "";
        if (info.effectAmount.Count == 1)
        {
            amount = info.effectAmount[0].ToString();
        }
        else
        {
            int rand = UnityEngine.Random.Range(info.effectAmount[0], info.effectAmount[1]);
            amount = rand.ToString();
        }
        switch (info.effectFormula)
        {
            case 0:
                if (Convert.ToInt32(amount) >= 0)  
                {
                    formula = "+";
                }
                switch (info.effectType)
                {
                    case 0:
                        type = "아르 ";
                        GoodsManager.goodsAr += Convert.ToInt32(amount);
                        break;
                    case 1:
                        type = "마정석 ";
                        GoodsManager.goodsStone += Convert.ToInt32(amount);
                        break;
                    case 2:
                        type = "명성 ";
                        GoodsManager.GoodsConstFame += Convert.ToInt32(amount);
                        break;
                    case 3:
                        string upgradedStudentsInfo = PlayerInfo.StudentStatRandomUpgrade(Convert.ToInt32(amount), 1)[0];
                        List<string> showList = upgradedStudentsInfo.Split('/').ToList();
                        type = $"{showList[0]}기 {showList[1]}분반 무작위 스탯 ";
                        break;
                    case 4:
                        type = "마정석 가격 ";
                        GoodsManager.exchangePercent = Convert.ToInt32(amount);
                        amount += "%";
                        break;
                    case 5:
                        type = "무작위 교수 스탯 ";
                        GoodsManager.goodsAr += Convert.ToInt32(amount); // 나중에 학생 스탯으로 변경
                        break;
                    case 6:
                        type = "마케팅 비용 ";
                        ClassEx.marA -= 1000;
                        ClassEx.marB -= 1000;
                        ClassEx.marC -= 1000;
                        ClassEx.marD -= 1000;
                        break;
                    default:
                        break;
                }
                returnString = "( " + type + formula + amount + " )";
                break;
            case 1:
                formula = "x";
                switch (info.effectType)
                {
                    case 0:
                        type = "아르 ";
                        GoodsManager.goodsAr *= Convert.ToInt32(amount);
                        break;
                    case 1:
                        type = "마정석 ";
                        GoodsManager.goodsStone *= Convert.ToInt32(amount);
                        break;
                    case 2:
                        type = "명성 ";
                        GoodsManager.GoodsConstFame *= Convert.ToInt32(amount);
                        break;
                    case 3:
                        string upgradedStudentsInfo = PlayerInfo.StudentStatRandomUpgrade(Convert.ToInt32(amount), 1)[0];
                        List<string> showList = upgradedStudentsInfo.Split('/').ToList();
                        type = $"{showList[0]}기 {showList[1]}분반 무작위 스탯 ";
                        break;
                    case 4:
                        type = "마정석 가격 ";
                        GoodsManager.exchangePercent *= Convert.ToInt32(amount);
                        amount += "%";
                        break;
                    case 5:
                        type = "무작위 교수 스탯 ";
                        GoodsManager.goodsAr *= Convert.ToInt32(amount); // 나중에 학생 스탯으로 변경
                        break;
                    default:
                        break;
                }
                returnString = "( " + type + formula + amount + " )";
                break;
            case 2:
                formula = "x";
                switch (info.effectType)
                {
                    case 0:
                        type = "아르 ";
                        GoodsManager.goodsAr /= Convert.ToInt32(amount);
                        break;
                    case 1:
                        type = "마정석 ";
                        GoodsManager.goodsStone /= Convert.ToInt32(amount);
                        break;
                    case 2:
                        type = "명성 ";
                        GoodsManager.GoodsConstFame /= Convert.ToInt32(amount);
                        break;
                    case 3:
                        type = "무작위 학생 그룹 무작위 스탯 ";
                        GoodsManager.goodsAr /= Convert.ToInt32(amount); // 나중에 학생 스탯으로 변경
                        break;
                    case 4:
                        type = "마정석 가격 ";
                        GoodsManager.exchangePercent /= Convert.ToInt32(amount);
                        amount += "%";
                        break;
                    case 5:
                        type = "무작위 교수 스탯 ";
                        GoodsManager.goodsAr /= Convert.ToInt32(amount); // 나중에 학생 스탯으로 변경
                        break;
                    default:
                        break;
                }
                double realAmount = 1 / Convert.ToDouble(amount);
                returnString = "( " + type + formula + realAmount + " )";
                break;
            default:
                break;
        }
        return returnString;
    }

    public void Start()
    {
        //test
        Debug.Log("GetEvent Test");
        (string, int) GeneratedRandomEvent = GetEvent();
        Debug.Log("SetEvent Test");
        Debug.Log("Enter rarity, EventID and delta, separated by whitespace");
        string rarityInput = "common";
        int id = 3;
        int delta = 5;
        SetEvent(rarityInput, id, delta);
    }
}