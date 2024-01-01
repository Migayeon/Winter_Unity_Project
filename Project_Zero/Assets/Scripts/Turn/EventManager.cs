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
using System.IO;
using System.Collections.Generic;

public class GenerateRandomEventCommon
{
    public const int NoneProbability = 4;
    public const int CommonProbability = 3;
    public const int RareProbability = 2;
    public const int UniqueProbability = 1;
    public const int totalProbability = NoneProbability + CommonProbability + RareProbability + UniqueProbability;

    public const int CommonCaseCount = 6;
    public const int RareCaseCount = 6;
    public const int UniqueCaseCount = 6;
    public static void Main(string[] args)
    {
        
        int returnValue;
        int rarityType = 0;
        string FileName;
        bool OpenFileFlag;
        List<int> probability = new List<int>(totalProbability);
        Random generateRandom = new Random();
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
        else if (rarity == 1) //common
        {
            FileName = "\\Assets\\Resources\\Events\\Common.csv";
            OpenFileFlag = true;
            rarityType = 1;
        }

        else if (rarity == 2) //rare
        {
            FileName = "\\Assets\\Resources\\Events\\Rare.csv";
            OpenFileFlag = true;
            rarityType = 2;
        }
        else //unique
        {
            FileName = "\\Assets\\Resources\\Events\\Unique.csv";
            OpenFileFlag = true;
            rarityType = 3;
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
            if (rarityType == 1)
            {
                List<int> cases = new List<int>(CommonCaseCount);
                for (int i = 0; i < CommonCaseCount; ++i)
                {
                    for (int j = 0; j < datalist[i, 0]; ++j)
                    {
                        cases.Add(datalist[i, 1]);
                    }
                }
                int returnValueIndex = generateRandom.Next(0, CommonCaseCount);
                returnValue = cases[returnValueIndex];
            }
            else if (rarityType == 2)
            {
                List<int> cases = new List<int>(RareCaseCount);
                for (int i = 0; i < RareCaseCount; ++i)
                {
                    for (int j = 0; j < datalist[i, 0]; ++j)
                    {
                        cases.Add(datalist[i, 1]);
                    }
                }
                int returnValueIndex = generateRandom.Next(0, RareCaseCount);
                returnValue = cases[returnValueIndex];
            }
            else
            {
                List<int> cases = new List<int>(UniqueCaseCount);
                for (int i = 0; i < UniqueCaseCount; ++i)
                {
                    for (int j = 0; j < datalist[i, 0]; ++j)
                    {
                        cases.Add(datalist[i, 1]);
                    }
                }
                int returnValueIndex = generateRandom.Next(0, UniqueCaseCount);
                returnValue = cases[returnValueIndex];
            }
        }
        else
        {
            returnValue = -1;
        }
        //code for testing output
        /*
        Console.WriteLine(FileName);
        Console.WriteLine(returnValue);
        */
    }
}