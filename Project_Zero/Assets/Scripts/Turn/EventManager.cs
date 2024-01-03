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

public class EventManager : MonoBehaviour
{
    //you can edit these constants before compile


    public const int NoneProbability = 65;
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
        string[] filenames = new string[] { "Assets\\Resources\\Events\\Common.csv", "Assets\\Resources\\Events\\Rare.csv", "Assets\\Resources\\Events\\Unique.csv" }; //enter filenames here
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
    public static Sprite ShowEvent(string rarity, int eventID)
    {
        Sprite returnSprite;
        string filePath = "";
        string[] filenames = new string[] { "Sprites/Common/",
            "Sprites/Rare/", "Sprites/Unique/" }; //enter filenames here
        int rarityIndex = 0;
        if (rarity == "common") rarityIndex = 0;
        else if (rarity == "rare") rarityIndex = 1;
        else rarityIndex = 2;
        filePath = filenames[rarityIndex];
        returnSprite = Resources.Load(filePath + eventID.ToString(), typeof(Sprite)) as Sprite;
        return returnSprite;
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