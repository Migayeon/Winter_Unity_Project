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
using static ProfessorSystem;


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
    const int RETRY_COST = 50;
    public static int professorGenerateSeed;
    public static int[] professorSeeds = new int[3];
    public static string GenerateName()
    {
        StreamReader ReadEnglishName = new StreamReader("Assets/Resources/Names/englishNames.csv");
        StreamReader ReadKoreanName = new StreamReader("Assets/Resources/Names/koreanNames.csv");
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
        System.Random randomGenerator = new System.Random(professorGenerateSeed);

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
        System.Random RandomGenerator = new System.Random(professorGenerateSeed);
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
            ProfessorStats[i] = (int)((ProfessorStats[i]) * ((float)StatScale / StatSum));
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
        professorGenerateSeed = RandomGenerator.Next(-2147483648, 2147483647);

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

    [SerializeField]
    private Button PickedProfessorPopupExitButton;
    [SerializeField]
    private List<Transform> ProfessorTransforms;
    [SerializeField]
    private GameObject ButtonClickObject;
    [SerializeField]
    private GameObject HideTextObject;
    [SerializeField]
    private GameObject MaxProfessorsErrorObject;

    [SerializeField]
    private TextMeshProUGUI RetryCostInfo;
    [SerializeField]
    private TextMeshProUGUI RetryFailMessage;
    [SerializeField]
    private TextMeshProUGUI MaxProfessorsErrorMessage;
    [SerializeField]
    private Sprite[] professorIllust;

    private List<ProfessorResume> ProfessorResumes;
    private TextMeshProUGUI PickedProfessorName, PickedProfessorType, PickedProfessorStat, PickedProfessorSalary;
    private Image PickedProfessorImage;

    void Awake()
    {
        /*
         * Initialize objects (GameObject, TextMeshProUGUI, Button, etc)
         */

        //TextMeshProUGUI
        MaxProfessorsErrorMessage = MaxProfessorsErrorMessage.GetComponent<TextMeshProUGUI>();
        ProfessorResumes = new List<ProfessorResume>();
        for (int i = 0; i < 3; i++)
            ProfessorResumes.Add(new ProfessorResume(ProfessorTransforms[i]));
        PickedProfessorPopupExitButton = PickedProfessorPopupExitButton.GetComponent<Button>();
        PickedProfessorName = HideTextObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        PickedProfessorType = HideTextObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        PickedProfessorStat = HideTextObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        PickedProfessorSalary = HideTextObject.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        PickedProfessorImage = HideTextObject.transform.GetChild(6).GetComponent<Image>();
    }
    void Start()
    {
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
            RetryCostInfo.enabled = true;

            // issue part - control flow be damned, this code is spaghetti code that even FSM would avoid. XD
            // A pastafarian nightmare, if i do say so myself

            if (BeforeTurn.HaveToGenerateNewProfessors)
            {
                PlayerInfo.RandomProfessorList.Clear();
                for (int i = 0; i < 3; ++i)
                    PlayerInfo.RandomProfessorList.Add(ProfessorResumes[i].GenerateNewProfessor(i));
                BeforeTurn.HaveToGenerateNewProfessors = false;
            }
            else
            {
                for (int i = 0; i < 3; ++i)
                    ProfessorResumes[i].professor = PlayerInfo.RandomProfessorList[i];
            }
            foreach (ProfessorResume nowResume in ProfessorResumes)
                nowResume.Open();
        }

        for (int i = 0; i < 3; i++)
        {
            if (!PlayerInfo.ProfessorPickedStatus[i])
                ProfessorResumes[i].Close();
        }
    }

    public void PickProfessor(int index)
    {
        ProfessorResume pickedProfessor = ProfessorResumes[index];
        PlayerInfo.ProfessorList.Add(pickedProfessor.professor);
        string professorName = pickedProfessor.name.text;
        if (professorName == "대 시후")
            AchievementManager.Achieve(0);
        PlayerInfo.ProfessorPickedStatus[index] = false;
        PickedProfessorName.text = professorName;
        PickedProfessorType.text = pickedProfessor.type.text;
        PickedProfessorStat.text = pickedProfessor.stat.text;
        PickedProfessorSalary.text = pickedProfessor.salary.text;
        PickedProfessorImage.sprite = professorIllust[pickedProfessor.professor.ProfessorGetType()];
        ProfessorResumes[index].Close();

        HideTextObject.SetActive(true);
        PlayerInfo.ProfessorPicked = true;
        PlayerInfo.PickedProfessorInfo = pickedProfessor.professor;

        PickedProfessorPopupExitButton.enabled = true;
    }
    public void RetryProfessors()
    {
        Debug.Log("RetryProfessors");
        if (GoodsManager.goodsAr >= RETRY_COST)
        {
            GoodsManager.goodsAr -= RETRY_COST;
            PlayerInfo.RandomProfessorList.Clear();
            PlayerInfo.ProfessorPicked = false;
            for (int i = 0; i < 3; ++i)
            {
                PlayerInfo.RandomProfessorList.Add(ProfessorResumes[i].GenerateNewProfessor(i));
                ProfessorResumes[i].Open();
                PlayerInfo.ProfessorPickedStatus[i] = true;
            }
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

    public static string Save()
    {
        SaveData resumeData = new SaveData();
        resumeData.seed = professorGenerateSeed;
        resumeData.resumeSeeds = professorSeeds;
        resumeData.isOpened = PlayerInfo.ProfessorPickedStatus;
        return JsonUtility.ToJson(resumeData);
    }

    public static void Load(string json)
    {
        SaveData resumeData = JsonUtility.FromJson<SaveData>(json);
        professorSeeds = resumeData.resumeSeeds;
        PlayerInfo.ProfessorPickedStatus = resumeData.isOpened;
        BeforeTurn.HaveToGenerateNewProfessors = false;
        PlayerInfo.RandomProfessorList.Clear();
        for (int i = 0; i < 3; i++)
        {
            professorGenerateSeed = resumeData.resumeSeeds[i];
            PlayerInfo.RandomProfessorList.Add(CreateNewProfessor(i));
        }
        professorGenerateSeed = resumeData.seed;
    }

    class SaveData
    {
        public int seed;
        public int[] resumeSeeds;
        public bool[] isOpened;
    }

    class ProfessorResume
    {
        public TextMeshProUGUI name;
        public TextMeshProUGUI type;
        public TextMeshProUGUI stat;
        public TextMeshProUGUI salary;
        public Button pickButton;
        public TextMeshProUGUI pickText;
        public Professor professor;

        public ProfessorResume(Transform professorResumeTransform)
        {
            name = professorResumeTransform.GetChild(0).GetComponent<TextMeshProUGUI>();
            type = professorResumeTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
            stat = professorResumeTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
            salary = professorResumeTransform.GetChild(3).GetComponent<TextMeshProUGUI>();
            Transform pick = professorResumeTransform.GetChild(4);
            pickButton = pick.GetComponent<Button>();
            pickText = pick.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        public void Close()
        {
            pickButton.enabled = false;
            name.text = "";
            type.text = "";
            stat.text = "";
            salary.text = "";
            pickText.text = "";
            pickText.ForceMeshUpdate();
        }

        public void Open()
        {
            pickButton.enabled = true;
            name.text = professor.ProfessorGetName();
            type.text = professor.ProfessorGetTypeInString();
            stat.text = StatToString();
            salary.text = "월급 : " + Convert.ToString(professor.ProfessorGetSalary());
        }

        public Professor GenerateNewProfessor(int index)
        {
            pickText.text = "채용";
            professorSeeds[index] = professorGenerateSeed;
            professor = CreateNewProfessor(index);
            return professor;
        }
        public string StatToString()
        {
            List<int> professorStat= professor.ProfessorGetStats();
            List<string> KoreanStatList = new List<string>(6)
            {
                "강의력",
                "마법이론",
                "마나감응",
                "손재주",
                "속성력",
                "영창"
            };
            string temp = "";
            for (int j = 0; j < professorStats; ++j)
            {
                temp += KoreanStatList[j];
                temp += " : ";
                temp += professorStat[j];
                temp += "<br>";
            }
            return temp;
        }
    }
}
