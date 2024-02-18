using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;
using System.Linq;


// TODO list for ProfessorSystem

// 1. BUGFIX
// (try) changing GetComponentInChildren to GetComponent
// 2. UI rework & overhaul
// 3. Code documentation

/*
 * On first entry (in current turn) -> free roll
 * when picked, show professor and lock all other buttons except [Go Back to Menu] and [Reroll]
 * 
 * if user returns to menu then comes back again, and is NOT first roll
 * -> idea 1: show previously picked professor data
 * -> idea 2: show blank professor slots with buttons not active except for [Reroll]
 * preferably idea 1, but if it doesn't work we can revert to 2.
 * 
 * else -> create new professors (first roll)
 * 
 *
 * <Enter Scene>
 * if (First Roll)
 * {
 *      generate three new professors
 *      store currently generate data (so that it can be reshown if user goes back to main menu then comes back again)
 *      
 *      if (Player selects professor)
 *      {
 *          send data to PlayerInfo
 *          store selected professor data (so that it can be shown again if player goes to main menu then comes back)
 *          
 *          if (player selects prof)
 *          {
 *          ...
 *      }
 * }
 * else
 * {
 *      if (player had selected before)
 *      {
 *          show info of previously selected professor
 *      }
 *      else
 *      {
 *          ...
 *      }
 * }
 * 
 * ISSUE: FLOWCHART IS A BIT TOO COMPLICATED!
 * ***: try completely rewriting the code at some point for better readability and stuff
 */


//UPDATED TODO : 24-01-31
// fix up code
// implement save data for returning from main menu (use PlayerInfo.cs)

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

    IEnumerator ShowPopupForSeconds(GameObject PopupObject, float PopupTime)
    {
        PopupObject.SetActive(true);
        yield return new WaitForSeconds(PopupTime);
        PopupObject.SetActive(false);
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

    //TODO: clean up TextMeshProUGUI assets (use arrays?)
    //ex) public TextMeshProUGUI[,] ProfessorData = new TextMeshProUGUI[3,3];

    public Button PickProfessor1Button;
    public Button PickProfessor2Button;
    public Button PickProfessor3Button;
    public Button RetryProfessorsButton;
    public Button ReturnMenuButton;
    public Button PickedProfessorPopupExitButton;
    public GameObject ProfessorObject1, ProfessorObject2, ProfessorObject3;
    public GameObject ButtonClickObject;
    public GameObject HideTextObject;
    public GameObject MaxProfessorsErrorObject;
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

    public TextMeshProUGUI RetryCostInfo;
    public TextMeshProUGUI RetryFailMessage;
    public TextMeshProUGUI MaxProfessorsErrorMessage;

    public TextMeshProUGUI PickProfessor1ButtonText;
    public TextMeshProUGUI PickProfessor2ButtonText;
    public TextMeshProUGUI PickProfessor3ButtonText;

    public Dictionary<int, string> KoreanStatList = new Dictionary<int, string>(6)
        {
            {0, "강의력"},
            {1, "마법이론"},
            {2, "마나감응"},
            {3, "손재주"},
            {4, "속성력"},
            {5, "영창"},
        };


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

    void Awake()
    {
        /*
         * Initialize objects (GameObject, TextMeshProUGUI, Button, etc)
         */

        //TextMeshProUGUI
        MaxProfessorsErrorMessage = MaxProfessorsErrorMessage.GetComponent<TextMeshProUGUI>();

        Professor1Name = Professor1Name.GetComponent<TextMeshProUGUI>();
        Professor1Type = Professor1Type.GetComponent<TextMeshProUGUI>();
        Professor1Stat = Professor1Stat.GetComponent<TextMeshProUGUI>();
        Professor1Salary = Professor1Salary.GetComponent<TextMeshProUGUI>();

        Professor2Name = Professor2Name.GetComponent<TextMeshProUGUI>();
        Professor2Type = Professor2Type.GetComponent<TextMeshProUGUI>();
        Professor2Stat = Professor2Stat.GetComponent<TextMeshProUGUI>();
        Professor2Salary = Professor2Salary.GetComponent<TextMeshProUGUI>();

        Professor3Name = Professor3Name.GetComponent<TextMeshProUGUI>();
        Professor3Type = Professor3Type.GetComponent<TextMeshProUGUI>();
        Professor3Stat = Professor3Stat.GetComponent<TextMeshProUGUI>();
        Professor3Salary = Professor3Salary.GetComponent<TextMeshProUGUI>();


        //Button
        PickProfessor1Button = PickProfessor1Button.GetComponent<Button>();
        PickProfessor2Button = PickProfessor2Button.GetComponent<Button>();
        PickProfessor3Button = PickProfessor3Button.GetComponent<Button>();
        RetryProfessorsButton = RetryProfessorsButton.GetComponent<Button>();
        ReturnMenuButton = ReturnMenuButton.GetComponent<Button>();
        PickedProfessorPopupExitButton = PickedProfessorPopupExitButton.GetComponent<Button>();

        //TextMeshProUGUI - Text content of Buttons
        PickProfessor1ButtonText = PickProfessor1ButtonText.GetComponent<TextMeshProUGUI>();
        PickProfessor2ButtonText = PickProfessor2ButtonText.GetComponent<TextMeshProUGUI>();
        PickProfessor3ButtonText = PickProfessor3ButtonText.GetComponent<TextMeshProUGUI>();

        // Add listeners to basic buttons
        RetryProfessorsButton.onClick.AddListener(RetryProfessors);
        ReturnMenuButton.onClick.AddListener(ReturnMenu);

    }
    void Start()
    {
        PickProfessor1ButtonText.text = "";
        PickProfessor2ButtonText.text = "";
        PickProfessor3ButtonText.text = "";

        //disable professor pick buttons
        PickProfessor1Button.enabled = false;
        PickProfessor2Button.enabled = false;
        PickProfessor3Button.enabled = false;
        //PickedProfessorPopupExitButton.enabled = false;

        HideTextObject.SetActive(false);

        MaxProfessorsErrorMessage.enabled = false;
        RetryFailMessage.enabled = false;
        RetryCostInfo.enabled = false;

        HideTextObject.SetActive(false);

        MaxProfessorsErrorObject.SetActive(false);

        //FIX THIS PART!!!
        //control flow is fucked lol, might have to redo the whole thing
        //end my suffering

        if (PlayerInfo.ProfessorCount() == PlayerInfo.maxProfessor) // if player has reached the maximum number of professors
        {
            MaxProfessorsErrorObject.SetActive(true);
            MaxProfessorsErrorMessage.text = string.Format("<안내><br><br>교수는 최대 {0}명 고용할 수 있습니다.", PlayerInfo.maxProfessor);
            Debug.Log("FIRST ROLL");
        }
        else
        {
            PickProfessor1ButtonText.text = "채용";
            PickProfessor2ButtonText.text = "채용";
            PickProfessor3ButtonText.text = "채용";
            MaxProfessorsErrorObject.SetActive(false);

            RetryCostInfo.enabled = true;
            RetryFailMessage.enabled = false;

            List<Professor> NewProfessors = new List<Professor>();

            // issue part - control flow be damned, this code is spaghetti code that even FSM would avoid.
            // A pastafarian nightmare, if i do say so myself

            
            if (BeforeTurn.ProfessorCreateFirstTime)
            {
                for (int i = 0; i < 3; ++i)
                {
                    NewProfessors.Add(CreateNewProfessor(i));
                    PlayerInfo.RandomProfessorList[i] = NewProfessors[i];
                }
                PlayerInfo.GenerateNewRandomProfessorList = false;
                BeforeTurn.ProfessorCreateFirstTime = false;

            }
            else
            {
                if (PlayerInfo.GenerateNewRandomProfessorList)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        NewProfessors.Add(CreateNewProfessor(i));
                        PlayerInfo.RandomProfessorList[i] = NewProfessors[i];
                    }
                    PlayerInfo.GenerateNewRandomProfessorList = false;
                }
                else
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        NewProfessors.Add(PlayerInfo.RandomProfessorList[i]);
                    }
                }
            }

            PickProfessor1Button.enabled = true;
            PickProfessor2Button.enabled = true;
            PickProfessor3Button.enabled = true;

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
             * \
             * 
            PickProfessor1Button = ButtonClickObject.GetComponentInChildren<Button>();
            PickProfessor2Button = ButtonClickObject.GetComponentInChildren<Button>();
            PickProfessor3Button = ButtonClickObject.GetComponentInChildren<Button>();
            RetryProfessorsButton = ButtonClickObject.GetComponentInChildren<Button>();
            ReturnMenuButton = ButtonClickObject.GetComponentInChildren<Button>();
            */


            PickProfessor1Button.onClick.AddListener(() => PickProfessor1(NewProfessors[0]));
            PickProfessor2Button.onClick.AddListener(() => PickProfessor2(NewProfessors[1]));
            PickProfessor3Button.onClick.AddListener(() => PickProfessor3(NewProfessors[2]));

            PickedProfessorPopupExitButton.onClick.RemoveAllListeners();
            PickedProfessorPopupExitButton.onClick.AddListener(() => ExitPopup(HideTextObject));
        }

        if (!PlayerInfo.ProfessorPickedStatus[0])
        {
            Professor1Name.text = "";
            Professor1Type.text = "";
            Professor1Stat.text = "";
            Professor1Salary.text = "";
            PickProfessor1Button.enabled = false;
        }
        if (!PlayerInfo.ProfessorPickedStatus[1])
        {
            Professor2Name.text = "";
            Professor2Type.text = "";
            Professor2Stat.text = "";
            Professor2Salary.text = "";
            PickProfessor2Button.enabled = false;
        }
        if (!PlayerInfo.ProfessorPickedStatus[2])
        {
            Professor3Name.text = "";
            Professor3Type.text = "";
            Professor3Stat.text = "";
            Professor3Salary.text = "";
            PickProfessor3Button.enabled = false;
        }
    }

    public void PickProfessor1(Professor InsertProf)
    {
        Debug.Log("PickProfessor1");
        PlayerInfo.ProfessorList.Add(InsertProf);
        if (InsertProf.ProfessorGetName() == "대 시후")
        {
            AchievementManager.Achieve(0);
        }
        PlayerInfo.ProfessorPickedStatus[0] = false;
        
        PickProfessor1Button.enabled = false;



        Debug.Log(Professor1Name.text);
        Debug.Log(Professor1Type.text);
        Debug.Log(Professor1Stat.text);
        Debug.Log(Professor1Salary.text);
        Professor1Name.text = "";
        Professor1Type.text = "";
        Professor1Stat.text = "";
        Professor1Salary.text = "";
        PickProfessor1ButtonText.text = "";
        Professor1Name.ForceMeshUpdate();
        Professor1Type.ForceMeshUpdate();
        Professor1Stat.ForceMeshUpdate();
        Professor1Salary.ForceMeshUpdate();
        PickProfessor1ButtonText.ForceMeshUpdate();
        /*
        Professor1Name.ForceMeshUpdate(true);
        Professor1Type.ForceMeshUpdate(true);
        Professor1Stat.ForceMeshUpdate(true);
        Professor1Salary.ForceMeshUpdate(true);
        */
        PickedProfessorName.text = InsertProf.ProfessorGetName();
        PickedProfessorType.text = InsertProf.ProfessorGetTypeInString();
        PickedProfessorStat.text = StatToString(InsertProf.ProfessorGetStats());
        PickedProfessorSalary.text = "월급 : " + Convert.ToString(InsertProf.ProfessorGetSalary());

        HideTextObject.SetActive(true);
        PlayerInfo.ProfessorPicked = true;
        PlayerInfo.PickedProfessorInfo = InsertProf;

        PickedProfessorPopupExitButton.enabled = true;
    }

    public void PickProfessor2(Professor InsertProf)
    {
        Debug.Log("PickProfessor2");
        PlayerInfo.ProfessorList.Add(InsertProf);
        if (InsertProf.ProfessorGetName() == "대 시후")
        {
            AchievementManager.Achieve(0);
        }
        PlayerInfo.ProfessorPickedStatus[1] = false;

        PickProfessor2Button.enabled = false;

        Debug.Log(Professor1Name.text);
        Debug.Log(Professor1Type.text);
        Debug.Log(Professor1Stat.text);
        Debug.Log(Professor1Salary.text);

        Professor2Name.text = "";
        Professor2Type.text = "";
        Professor2Stat.text = "";
        Professor2Salary.text = "";
        PickProfessor2ButtonText.text = "";
        Professor2Name.ForceMeshUpdate();
        Professor2Type.ForceMeshUpdate();
        Professor2Stat.ForceMeshUpdate();
        Professor2Salary.ForceMeshUpdate();
        PickProfessor2ButtonText.ForceMeshUpdate();

        PickedProfessorName.text = InsertProf.ProfessorGetName();
        PickedProfessorType.text = InsertProf.ProfessorGetTypeInString();
        PickedProfessorStat.text = StatToString(InsertProf.ProfessorGetStats());
        PickedProfessorSalary.text = "월급 : " + Convert.ToString(InsertProf.ProfessorGetSalary());

        HideTextObject.SetActive(true);
        PlayerInfo.ProfessorPicked = true;
        PlayerInfo.PickedProfessorInfo = InsertProf;

        PickedProfessorPopupExitButton.enabled = true;

    }

    public void PickProfessor3(Professor InsertProf)
    {
        Debug.Log("PickProfessor3");
        PlayerInfo.ProfessorList.Add(InsertProf);
        if (InsertProf.ProfessorGetName() == "대 시후")
        {
            AchievementManager.Achieve(0);
        }
        PlayerInfo.ProfessorPickedStatus[2] = false;

        PickProfessor3Button.enabled = false;

        Debug.Log(Professor1Name.text);
        Debug.Log(Professor1Type.text);
        Debug.Log(Professor1Stat.text);
        Debug.Log(Professor1Salary.text);

        Professor3Name.text = "";
        Professor3Type.text = "";
        Professor3Stat.text = "";
        Professor3Salary.text = "";
        PickProfessor3ButtonText.text = "";
        Professor3Name.ForceMeshUpdate();
        Professor3Type.ForceMeshUpdate();
        Professor3Stat.ForceMeshUpdate();
        Professor3Salary.ForceMeshUpdate();
        PickProfessor3ButtonText.ForceMeshUpdate();

        PickedProfessorName.text = InsertProf.ProfessorGetName();
        PickedProfessorType.text = InsertProf.ProfessorGetTypeInString();
        PickedProfessorStat.text = StatToString(InsertProf.ProfessorGetStats());
        PickedProfessorSalary.text = "월급 : " + Convert.ToString(InsertProf.ProfessorGetSalary());

        HideTextObject.SetActive(true);
        PlayerInfo.ProfessorPicked = true;
        PlayerInfo.PickedProfessorInfo = InsertProf;
    }
    public void RetryProfessors()
    {
        Debug.Log("RetryProfessors");
        if (GoodsManager.goodsAr >= 50)
        {
            GoodsManager.goodsAr -= 50;
            Array.Clear(PlayerInfo.RandomProfessorList, 0, PlayerInfo.RandomProfessorList.Length);
            PlayerInfo.GenerateNewRandomProfessorList = true;
            PlayerInfo.ProfessorPicked = false;

            for (int i = 0; i < 3; ++i)
            {
                PlayerInfo.ProfessorPickedStatus[i] = true;
            }
            Start();
        }
        else
        {
            RetryCostInfo.enabled = false;
            RetryFailMessage.enabled = true;
        }
    }

    public void ReturnMenu()
    {
        Debug.Log("ReturnMenu");
        LoadingSceneManager.LoadScene("Main");
    }

    public void ExitPopup(GameObject PopupObject)
    {
        Debug.Log("ExitPopup");
        PopupObject.SetActive(false);
    }
}
