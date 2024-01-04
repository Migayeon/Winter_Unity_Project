using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//WIP

public class CreateProfessor : ProfessorSystem
{
    const int UniqueProfessorRarity = 5; //probability (edit later)
    const int BattleProfessorRarity = 15; //probability (edit later)
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

    public static Professor CreateNewProfessor(int num) // "static" keyword is included for testing purposes only (used in professor info management testing), may remove later
    {
        System.Random RandomGenerator = new System.Random();
        List<int> ProfessorRarityList = new List<int>(TotalRarity);
        for (int i = 0; i < UniqueProfessorRarity; ++i)
        {
            ProfessorRarityList.Add(2);
        }
        for (int i = 0; i < BattleProfessorRarity; ++i)
        {
            ProfessorRarityList.Add(1);
        }
        for (int i = 0; i < TotalRarity - BattleProfessorRarity - UniqueProfessorRarity; ++i)
        {
            ProfessorRarityList.Add(0);
        }

        long ProfessorID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + Convert.ToString(num));
        string ProfessorName = GenerateName();
        int ProfessorTenure = 0;
        int ProfessorRarity = ProfessorRarityList[RandomGenerator.Next(0, 100)];
        List<int> ProfessorStats = new List<int>(6);
        if (ProfessorRarity == 2)
        {
            for (int i = 0; i < ProfessorSystem.professorStats; ++i)
            {
                ProfessorStats.Add(666);  //Temporary stats for <BATTLE> type Professor, edit later
            }
        }
        else if (ProfessorRarity == 1)
        {
            for (int i = 0; i < ProfessorSystem.professorStats; ++i)
            {
                ProfessorStats.Add(666);  //Temporary stats for <BATTLE> type Professor, edit later
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

    public Button PickProfessor1Button;
    public Button PickProfessor2Button;
    public Button PickProfessor3Button;
    public Button RetryProfessorsButton;
    public Button ReturnMenuButton;
    public GameObject ShowTextObject1, ShowTextObject2, ShowTextObject3;
    public GameObject ButtonClickObject;
    public GameObject HideTextObject;
    public TextMeshProUGUI PickedProfessorName, PickedProfessorType, PickedProfessorStat, PickedProfessorSalary;
    public TextMeshProUGUI Professor1Name;
    public TextMeshProUGUI Professor1Type;
    public TextMeshProUGUI Professor1Stat;
    public TextMeshProUGUI Professor1Salary;
    public TextMeshProUGUI Professor2Name;
    public TextMeshProUGUI Professor2Type;
    public TextMeshProUGUI Professor2Stat;
    public TextMeshProUGUI Professor2Salary;
    public TextMeshProUGUI Professor3Name;
    public TextMeshProUGUI Professor3Type;
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
    
    public string StatToString(List<int> stat)
    {
        
        string temp = "";
        for (int j = 0; j < professorStats; ++j)
        {
            temp += KoreanStatList[j];
            temp += " : ";
            temp += stat[j];
            temp += "<br>";
        }
        return temp;
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
        Professor1Type.text = NewProfessors[0].ProfessorGetTypeInString();
        Professor2Type.text = NewProfessors[1].ProfessorGetTypeInString();
        Professor3Type.text = NewProfessors[2].ProfessorGetTypeInString();
        {

            List<int> tempStatData = new List<int>(professorStats);
            tempStatData = NewProfessors[0].ProfessorGetStats();
            Professor1Stat.text = StatToString(tempStatData);
        }
        {

            List<int> tempStatData = new List<int>(professorStats);
            tempStatData = NewProfessors[1].ProfessorGetStats();
            Professor2Stat.text = StatToString(tempStatData);
        }
        {

            List<int> tempStatData = new List<int>(professorStats);
            tempStatData = NewProfessors[2].ProfessorGetStats();
            Professor3Stat.text = StatToString(tempStatData);
        }
        Professor1Salary.text = "월급 : " + Convert.ToString(NewProfessors[0].ProfessorGetSalary());
        Professor2Salary.text = "월급 : " + Convert.ToString(NewProfessors[1].ProfessorGetSalary());
        Professor3Salary.text = "월급 : " + Convert.ToString(NewProfessors[2].ProfessorGetSalary());

        /*
        PickProfessor1Button = ButtonClickObject.GetComponentInChildren<Button>();
        PickProfessor2Button = ButtonClickObject.GetComponentInChildren<Button>();
        PickProfessor3Button = ButtonClickObject.GetComponentInChildren<Button>();
        RetryProfessorsButton = ButtonClickObject.GetComponentInChildren<Button>();
        ReturnMenuButton = ButtonClickObject.GetComponentInChildren<Button>();
        */

        PickProfessor1Button.onClick.AddListener(() => PickProfessor1(NewProfessors[0]));
        PickProfessor2Button.onClick.AddListener(() => PickProfessor2(NewProfessors[1]));
        PickProfessor3Button.onClick.AddListener(() => PickProfessor3(NewProfessors[2]));
        RetryProfessorsButton.onClick.AddListener(RetryProfessors);
        ReturnMenuButton.onClick.AddListener(ReturnMenu);

        HideTextObject.SetActive(false);
        Professor1Name = ShowTextObject1.GetComponentInChildren<TextMeshProUGUI>();
        Professor2Name = ShowTextObject2.GetComponentInChildren<TextMeshProUGUI>();
        Professor3Name = ShowTextObject3.GetComponentInChildren<TextMeshProUGUI>();
        Professor1Type = ShowTextObject1.GetComponentInChildren<TextMeshProUGUI>();
        Professor2Type = ShowTextObject2.GetComponentInChildren<TextMeshProUGUI>();
        Professor3Type = ShowTextObject3.GetComponentInChildren<TextMeshProUGUI>();
        Professor1Stat = ShowTextObject1.GetComponentInChildren<TextMeshProUGUI>();
        Professor2Stat = ShowTextObject2.GetComponentInChildren<TextMeshProUGUI>();
        Professor3Stat = ShowTextObject3.GetComponentInChildren<TextMeshProUGUI>();
        Professor1Salary = ShowTextObject1.GetComponentInChildren<TextMeshProUGUI>();
        Professor2Salary = ShowTextObject2.GetComponentInChildren<TextMeshProUGUI>();
        Professor3Salary = ShowTextObject3.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void PickProfessor1(Professor InsertProf)
    {
        
        Debug.Log("PickProfessor1");
        PlayerInfo.ProfessorList.Add(InsertProf);
        /*
        Professor1Name.ForceMeshUpdate(true);
        Professor2Name.ForceMeshUpdate(true);
        Professor3Name.ForceMeshUpdate(true);
        Professor1Type.ForceMeshUpdate(true);
        Professor2Type.ForceMeshUpdate(true);
        Professor3Type.ForceMeshUpdate(true);
        Professor1Stat.ForceMeshUpdate(true);
        Professor2Stat.ForceMeshUpdate(true);
        Professor3Stat.ForceMeshUpdate(true);
        Professor1Salary.ForceMeshUpdate(true);
        Professor2Salary.ForceMeshUpdate(true);
        Professor3Salary.ForceMeshUpdate(true);
        */
        PickedProfessorName.text = InsertProf.ProfessorGetName();
        PickedProfessorType.text = InsertProf.ProfessorGetTypeInString();
        PickedProfessorStat.text = StatToString(InsertProf.ProfessorGetStats());
        PickedProfessorSalary.text = "월급 : " + Convert.ToString(InsertProf.ProfessorGetSalary());
        PickedProfessorName = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorType = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorStat = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorSalary = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        HideTextObject.SetActive(true);
    }
        public void PickProfessor2(Professor InsertProf)
    {
        Debug.Log("PickProfessor2");
        PlayerInfo.ProfessorList.Add(InsertProf);
        PickedProfessorName.text = InsertProf.ProfessorGetName();
        PickedProfessorType.text = InsertProf.ProfessorGetTypeInString();
        PickedProfessorStat.text = StatToString(InsertProf.ProfessorGetStats());
        PickedProfessorSalary.text = "월급 : " + Convert.ToString(InsertProf.ProfessorGetSalary());
        PickedProfessorName = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorType = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorStat = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorSalary = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        HideTextObject.SetActive(true);

    }
    public void PickProfessor3(Professor InsertProf)
    {
        Debug.Log("PickProfessor3");
        PlayerInfo.ProfessorList.Add(InsertProf);
        PickedProfessorName.text = InsertProf.ProfessorGetName();
        PickedProfessorType.text = InsertProf.ProfessorGetTypeInString();
        PickedProfessorStat.text = StatToString(InsertProf.ProfessorGetStats());
        PickedProfessorSalary.text = "월급 : " + Convert.ToString(InsertProf.ProfessorGetSalary());
        PickedProfessorName = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorType = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorStat = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        PickedProfessorSalary = HideTextObject.GetComponentInChildren<TextMeshProUGUI>();
        HideTextObject.SetActive(true);
    }
    public void RetryProfessors()
    {
        Debug.Log("RetryProfessors");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnMenu()
    {
        Debug.Log("ReturnMenu");
        SceneManager.LoadScene("Main");
    }
}
