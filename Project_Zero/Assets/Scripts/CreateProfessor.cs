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
 */


//UPDATED TODO : 24-01-31
// fix up code
// implement save data for returning from main menu (use PlayerInfo.cs)

public class CreateProfessor : ProfessorSystem
{    
    const int UniqueProfessorRarity = 5; //probability (edit later)
    const int BattleProfessorRarity = 15; //probability (edit later)
    const int RETRY_COST = 50;
    public static int professorGenerateSeed;
    public static int[] professorSeeds = new int[3];
    public static string GenerateName(int seed = 2147483647)
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
        System.Random randomGenerator;
        if (seed != 2147483647)
            randomGenerator = new System.Random(professorGenerateSeed);
        else
            randomGenerator = new System.Random();


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

    public static Professor CreateRandomProfessor(int num) // "static" keyword is included for testing purposes only (used in professor info management testing), may remove later
    {
        System.Random RandomGenerator = new System.Random(professorGenerateSeed);
        long ProfessorID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + Convert.ToString(num));
        string ProfessorName = GenerateName(professorGenerateSeed);
        int ProfessorTenure = 0;
        int ProfessorRarity = RandomGenerator.Next(0, 100);
        if (ProfessorRarity <= UniqueProfessorRarity)
            ProfessorRarity = 2;
        else if (ProfessorRarity <= UniqueProfessorRarity + BattleProfessorRarity)
            ProfessorRarity = 1;
        else
            ProfessorRarity = 0;
        int[] ProfessorStats = new int[professorStats];
        int StatScale = 0, salarySaleWeight = 0;
        switch (ProfessorRarity)
        {
            case 0:
                StatScale = RandomGenerator.Next(2700, 4200 + 1);
                salarySaleWeight = 3;
                break;
            case 1:
                StatScale = RandomGenerator.Next(2700, 4200 + 1);
                salarySaleWeight = 3;
                break;
            case 2:
                StatScale = RandomGenerator.Next(3600, 4500 + 1);
                salarySaleWeight = 3;
                break;
        }
        for (int i = 1; i < professorStats; i++)
        {
            switch (ProfessorRarity)
            {
                case 0:
                    ProfessorStats[i] += 100;
                    StatScale -= 100;
                    break;
                case 1:
                    ProfessorStats[i] += 100;
                    StatScale -= 100;
                    break;
                case 2:
                    ProfessorStats[i] += 100;
                    StatScale -= 100;
                    break;
            }
        }
        switch (ProfessorRarity)
        {
            case 0:
                ProfessorStats[0] += 200;
                StatScale -= 200;
                break;
            case 1:
                ProfessorStats[0] += 200;
                StatScale -= 200;
                break;
            case 2:
                ProfessorStats[0] += 300;
                StatScale -= 300;
                break;
        }
        for (int i = 0; i < StatScale; i++)
            ProfessorStats[RandomGenerator.Next(professorStats)]++;
        Professor NewProfessor = new Professor(ProfessorID, ProfessorName, ProfessorTenure, ProfessorRarity, ProfessorStats.ToList(), StatScale / salarySaleWeight);
        professorGenerateSeed = RandomGenerator.Next(-2147483648, 2147483647);
        return NewProfessor;
    }

    public static Professor CreateStandardProfessor(int num, int rarity)
    {
        System.Random RandomGenerator = new System.Random();
        long ProfessorID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + Convert.ToString(num));
        string ProfessorName = GenerateName();
        int ProfessorTenure = 0;
        int ProfessorRarity = rarity;
        int[] ProfessorStats = new int[professorStats];
        int StatScale = 0, salarySaleWeight = 0;
        switch (ProfessorRarity)
        {
            case 0:
                StatScale = 500;
                salarySaleWeight = 2;
                break;
            case 1:
                StatScale = 500;
                salarySaleWeight = 2;
                break;
        }
        for (int i = 1; i < professorStats; i++)
        {
            switch (ProfessorRarity)
            {
                case 0:
                    ProfessorStats[i] += 50;
                    StatScale -= 50;
                    break;
                case 1:
                    ProfessorStats[i] += 50;
                    StatScale -= 50;
                    break;
            }
        }
        switch (ProfessorRarity)
        {
            case 0:
                ProfessorStats[0] += 100;
                break;
            case 1:
                ProfessorStats[0] += 100;
                break;
        }
        for (int i = 0; i < StatScale; i++)
            ProfessorStats[RandomGenerator.Next(professorStats)]++;
        Professor NewProfessor = new Professor(ProfessorID, ProfessorName, ProfessorTenure, ProfessorRarity, ProfessorStats.ToList(), StatScale / salarySaleWeight);
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
    private TextMeshProUGUI PickedProfessorName, PickedProfessorType, PickedProfessorStat, PickedProfessorSalary, PickedProfessorDeposit;
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
        PickedProfessorDeposit = HideTextObject.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        PickedProfessorImage = HideTextObject.transform.GetChild(7).GetComponent<Image>();
    }
    void Start()
    {
        HideTextObject.SetActive(false);

        MaxProfessorsErrorMessage.enabled = false;
        RetryFailMessage.enabled = false;
        RetryCostInfo.enabled = false;

        HideTextObject.SetActive(false);
        MaxProfessorsErrorObject.SetActive(false);

        if (PlayerInfo.ProfessorCount() == PlayerInfo.maxProfessor)
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
        if (GoodsManager.goodsAr < pickedProfessor.depositNum)
            return;
        GoodsManager.goodsAr -= pickedProfessor.depositNum;
        PlayerInfo.ProfessorList.Add(pickedProfessor.professor);
        string professorName = pickedProfessor.name.text;
        if (professorName == "대 시후")
            AchievementManager.Achieve(0);
        PlayerInfo.ProfessorPickedStatus[index] = false;
        PickedProfessorName.text = professorName;
        PickedProfessorType.text = pickedProfessor.type.text;
        PickedProfessorStat.text = pickedProfessor.stat.text;
        PickedProfessorSalary.text = pickedProfessor.salary.text;
        PickedProfessorDeposit.text = pickedProfessor.deposit.text;
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
            PlayerInfo.RandomProfessorList.Add(CreateRandomProfessor(i));
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
        public TextMeshProUGUI deposit;
        public Button pickButton;
        public TextMeshProUGUI pickText;
        public Professor professor;
        public int depositNum;

        public ProfessorResume(Transform professorResumeTransform)
        {
            name = professorResumeTransform.GetChild(0).GetComponent<TextMeshProUGUI>();
            type = professorResumeTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
            stat = professorResumeTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
            salary = professorResumeTransform.GetChild(3).GetComponent<TextMeshProUGUI>();
            deposit = professorResumeTransform.GetChild(4).GetComponent<TextMeshProUGUI>();
            Transform pick = professorResumeTransform.GetChild(5);
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
            deposit.text = "";
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
            int weight = 2;
            switch (professor.ProfessorGetType())
            {
                case 0:
                    weight = 2;
                    break;
                case 1:
                    weight = 2;
                    break;
                case 2:
                    weight = 3;
                    break;
            }
            depositNum = professor.ProfessorGetSalary() * weight;
            deposit.text = "계약금 : " + Convert.ToString(depositNum);
        }

        public Professor GenerateNewProfessor(int index)
        {
            pickText.text = "채용";
            professorSeeds[index] = professorGenerateSeed;
            professor = CreateRandomProfessor(index);
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
