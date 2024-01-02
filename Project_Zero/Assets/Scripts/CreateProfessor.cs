using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

//WIP
public class CreateProfessor : ProfessorSystem
{
    const int UniqueProfessorRarity = 5;
    const int TotalRarity = 100;
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
        
        //log for debug
        Debug.Log(string.Format("New random name created: {0}", finalRandomName));
        return finalRandomName;
    }

    Professor CreateNewProfessor(int iteration)
    {
        System.Random RandomGenerator = new System.Random();
        List<int> ProfessorRarityList = new List<int>(TotalRarity);
        for (int i = 0; i < UniqueProfessorRarity; ++i)
        {
            ProfessorRarityList.Add(1);
        }
        for (int i = UniqueProfessorRarity; i < TotalRarity; ++i)
        {
            ProfessorRarityList.Add(0);
        }

        long ProfessorID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss")) + (long)iteration;
        string ProfessorName = GenerateName();
        int ProfessorTenure = 0;
        int ProfessorRarity = ProfessorRarityList[RandomGenerator.Next(0, 100)];
        List<int> ProfessorStats = new List<int>(6);
        if (ProfessorRarity == 1)
        {
            for (int i = 0; i < ProfessorSystem.professorStats; ++i)
            {
                ProfessorStats.Add(666);  //temporary placeholder number, edit as needed
            }
        }
        else
        {
            int StatScale = RandomGenerator.Next(3000, 4001);
            int StatSum = 0;
            for (int i = 0; i < ProfessorSystem.professorStats; ++i)
            {
                ProfessorStats.Add(RandomGenerator.Next(1000, 10000));
                StatSum += ProfessorStats[i];
            }
            for (int i = 0; i < ProfessorSystem.professorStats; ++i)
            {
                ProfessorStats[i] = (int)((float)(ProfessorStats[i]) * ((float)StatScale / StatSum));
            }
            //3000~4000, divide among 6 stats;
        }
        //set salary (sum of all stats)
        int ProfessorSalary = 0;
        for (int i = 0; i < ProfessorSystem.professorStats; ++i)
        {
            ProfessorSalary += ProfessorStats[i];
        }
        Professor NewProfessor = new Professor(ProfessorID, ProfessorName, ProfessorTenure, ProfessorRarity, ProfessorStats, ProfessorSalary);
        Debug.Log(string.Format("Iteration {0}", iteration));
        NewProfessor.UnityDebugLogProfessorInfo();
        //test logging
        /*
        Debug.Log(string.Format("Iteration {0}", iteration));
        Debug.Log("Created new professor");
        Debug.Log(string.Format("Professor ID : {0}", ProfessorID));
        Debug.Log(string.Format("Professor Name : {0}", ProfessorName));
        Debug.Log(string.Format("Professor Tenure : {0}", ProfessorTenure));
        if (ProfessorRarity == 1)
            Debug.Log("Professor Rarity : unique");
        else
            Debug.Log("Professor Rarity : normal");
        string temp = "Professor Stats : ";
        for (int i = 0; i < ProfessorSystem.professorStats; ++i)
        {
            temp += ProfessorSystem.Professor.statlist[i];
            temp += " ";
            temp += Convert.ToString(ProfessorStats[i]);
            temp += "     ";
        }
        Debug.Log(temp);
        Debug.Log(ProfessorSalary);
        */
        return NewProfessor;

    }
    void Start()
    {
        List<Professor> NewProfessorList = new List<Professor>(3);
        Professor tempProfessor;
        for (int i = 0; i < 3; ++i)
        {
            tempProfessor = CreateNewProfessor(i);
            NewProfessorList.Add(tempProfessor);
        }
    }
}
