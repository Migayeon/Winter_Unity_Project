using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

//WIP
public class CreateProfessor : ProfessorSystem
{
    const int UniqueProfessorRarity = 5;
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

    Professor CreateNewProfessor(int iteration)
    {
        Professor NewProfessor = new Professor();
        System.Random RandomGenerator = new System.Random();
        List<int> ProfessorRarityList = new List<int>(100);
        for (int i = 0; i < UniqueProfessorRarity; ++i)
        {
            ProfessorRarityList[i] = 1;
        }
        for (int i = UniqueProfessorRarity; i < 100; ++i)
        {
            ProfessorRarityList[i] = 0;
        }

        int ProfessorID = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss")) + iteration;
        string ProfessorName = GenerateName();
        int ProfessorTenure = 0;
        int ProfessorRarity = ProfessorRarityList[RandomGenerator.Next(0, 100)];
        if (ProfessorRarity == 1)
        {
            //insert
        }
        else
        {
            //3000~4000, divide among 6 stats;
            
        }
        //set salary (sum of all stats)
        //insert above calculated values into new Professor class object
        //return object
        Debug.Log(NewProfessor);
        return NewProfessor;

    }
    void Start()
    {
        List<Professor> NewProfessors = new List<Professor>(3);
        for (int i = 0; i < 3; ++i)
        {
            NewProfessors.Add(CreateNewProfessor(i));
        }
    }
}
