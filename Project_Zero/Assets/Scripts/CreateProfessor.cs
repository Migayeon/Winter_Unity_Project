using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


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

    Professor CreateNewProfessor(int num)
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

        long ProfessorID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + Convert.ToString(num));
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
        Debug.Log(string.Format("num {0}", num));
        NewProfessor.UnityDebugLogProfessorInfo();
        //test logging
        /*
        Debug.Log(string.Format("num {0}", num));
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
    public GameObject TestObject;
    public TextMeshProUGUI Professor1Name;
    public TextMeshProUGUI Professor1Stat;
    public TextMeshProUGUI Professor1Salary;
    public TextMeshProUGUI Professor2Name;
    public TextMeshProUGUI Professor2Stat;
    public TextMeshProUGUI Professor2Salary;
    public TextMeshProUGUI Professor3Name;
    public TextMeshProUGUI Professor3Stat;
    public TextMeshProUGUI Professor3Salary;

    public Dictionary<int, string> KoreanStatList = new Dictionary<int, string>(6)
        {
            {0, "강의력"},
            {1, "마법이론"},
            {2, "마나감응"},
            {3, "손재주"},
            {4, "속성력"},
            {5, "영창"},
        };

   //public TextMeshProUGUI[,] ProfessorData = new TextMeshProUGUI[3,3];

    void Awake()
    {

    }
    void Start()
    {
        List<Professor> NewProfessors = new List<Professor>();
        for (int i = 0; i < 3; ++i)
        {
            NewProfessors.Add(CreateNewProfessor(i));
        }

        Professor1Name.text = NewProfessors[0].ProfessorGetName();
        Professor2Name.text = NewProfessors[1].ProfessorGetName();
        Professor3Name.text = NewProfessors[2].ProfessorGetName();
        
        Professor1Salary.text = "월급 : " + Convert.ToString(NewProfessors[0].ProfessorGetSalary());
        Professor2Salary.text = "월급 : " + Convert.ToString(NewProfessors[1].ProfessorGetSalary());
        Professor3Salary.text = "월급 : " + Convert.ToString(NewProfessors[2].ProfessorGetSalary());
        Debug.Log("CHECK");
        {

            List<int> tempStatData = new List<int>(professorStats);
            tempStatData = NewProfessors[0].ProfessorGetStats();
            string temp = "";
            for (int j = 0; j < professorStats; ++j)
            {
                temp += KoreanStatList[j];
                temp += " : ";
                temp += tempStatData[j];
                temp += "\n";
                Debug.Log(string.Format("Iteration {0}", j));
            }
            Professor1Stat.text = temp;
        }
        Debug.Log("passed First Loop");
        {

            List<int> tempStatData = new List<int>(professorStats);
            tempStatData = NewProfessors[1].ProfessorGetStats();
            string temp = "";
            for (int j = 0; j < professorStats; ++j)
            {
                temp += KoreanStatList[j];
                temp += " : ";
                temp += tempStatData[j];
                temp += "\n";
            }
            Professor2Stat.text = temp;
        }
        {

            List<int> tempStatData = new List<int>(professorStats);
            string temp = "";
            tempStatData = NewProfessors[2].ProfessorGetStats();
            for (int j = 0; j < professorStats; ++j)
            {
                temp += KoreanStatList[j];
                temp += " : ";
                temp += tempStatData[j];
                temp += "\n";
            }
            Professor3Stat.text = temp;
        }
        Professor1Name = TestObject.GetComponentInChildren<TextMeshProUGUI>(); 
        Professor2Name = TestObject.GetComponentInChildren<TextMeshProUGUI>();
        Professor3Name = TestObject.GetComponentInChildren<TextMeshProUGUI>();
        Professor1Stat = TestObject.GetComponentInChildren<TextMeshProUGUI>(); 
        Professor2Stat = TestObject.GetComponentInChildren<TextMeshProUGUI>();
        Professor3Stat = TestObject.GetComponentInChildren<TextMeshProUGUI>();

        
        /*
        if (false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        */
    }
}
