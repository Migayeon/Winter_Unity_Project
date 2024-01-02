/*
variable returnValue equals the eventID resulting from the random selection
If "None" was selected, returnValue will be -1.

Note: this code is absolute garbage tier code that only barely works.
I might clean it up to make it perform better, no promises though

IMPORTANT: DO NOT DETELE THE (0,0) AT THE TOPMOST LINES OF THE CSV FILES
중요: CSV 파일들의 첫 번째 행에 있는 (0,0) 데이터를 삭제하지 말아주세요!
*/

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenerateRandomEvent : MonoBehaviour
{
    //you can edit these constants before compile
    public const int NoneProbability = 4;
    public const int CommonProbability = 3;
    public const int RareProbability = 2;
    public const int UniqueProbability = 1;
    public const int totalProbability = NoneProbability + CommonProbability + RareProbability + UniqueProbability;

    public const int CommonCaseCount = 6;
    public const int RareCaseCount = 6;
    public const int UniqueCaseCount = 6;
    static (string, int) GetEvent()
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

    static void SetEvent(string rarity, int EventID, int changeValue)
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