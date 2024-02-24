// Instantiates 10 copies of Prefab each 2 units apart from each other

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

/* TODO:
 * - Fix random generation of skills that can be upgraded
 * idea:
 * 1) at start of turn, generate random skill indexes and values for current # of professors
 * 2) when player enters ManageProfessor scene, check the number of professors. If there are deltas (increases),
 *    generate new indexes and values for the added professors
 * 3) store current data in PlayerInfo.cs, update when neded
 * 
 *    TODO: create list that stores if professor is currently employed
 *    (to make sure deletions are handled properly and to show right skill upgrade indexes and values)
 *    
 *    lots of stuff to do, lol
 */


/* Issue Tracker (updated 2024-02-18) */
/*
 * Issue 1: Professor Upgrade StatList
 * 
 * 
 * 
 * 
 * 
 */

public class ManageProfessor : MonoBehaviour
{

    /* UI Objects */
    
    // TextMeshPro

    public TextMeshProUGUI PopupTitleText;
    public TextMeshProUGUI ProfessorName;
    public TextMeshProUGUI ProfessorType;
    public TextMeshProUGUI ProfessorSalary;
    public TextMeshProUGUI ProfessorStat;


    public TextMeshProUGUI[] ProfessorInfo = new TextMeshProUGUI[5];
    public TextMeshProUGUI[] PopupText = new TextMeshProUGUI[6];

    public TextMeshProUGUI ProfessorCountInfo;

    // Button

    public Button ReturnButton; //Button for Return to Main Menu option
    public Button ExitPopupButton; //Button for exiting the Professor Information popup

    public Button UpgradeButton;
    public Button AwayButton;
    public Button FireButton;

    /* GameObjects */

    public GameObject ProfessorAwayRefuseMessage; // Error message when player cannot send professor away
    public GameObject ProfessorFireRefuseMessage; // Error message when player cannot fire professor
    public GameObject NoProfessorsHired; // message when player does not have any professors hired

    public GameObject ReferencePrefab; //reference Prefab for instantiation Professor buttons

    public GameObject content; //GameObject for spawning new instances of the reference prefab

    public GameObject PopUpObject; //GameObject for spawning the Professor Information popup


    public List<Button> ProfessorButtons = new List<Button>(); //List of Professors (as Button objects, see prefab)



    /* constants */

    public const int ProfessorAwayTime = 3; //number of turns professor is "away" for

    //min, max values for the "upgrade professor by how much" question (random generation)
    public const int ProfessorUpgradeValueMinimum = 50;
    public const int ProfessorUpgradeValueMaximum = 200;
    


    // Dictionary object referencing what skill the index of the Professor Stats list represents

    public Dictionary<int, string> KoreanStatList = new Dictionary<int, string>(6)
        {
            {0, "강의력"},
            {1, "마법이론"},
            {2, "마나감응"},
            {3, "손재주"},
            {4, "속성력"},
            {5, "영창"},
        };

    
    /* Random Number Generation */
    public System.Random randomseed = new System.Random();


    /* ProfessorStatsIndexToString */
    // Function for turning Professor stats into a string

    // This function is used to format Professor Stat data in a way that can be read by TextMeshPro and shown to the player.

    // i.e. if IndexBegin = 2 and IndexEnd = 4, the returned string will look like this:
    // 마나감응 : 1234<br>손재주 : 1234<br>속성력 : 1234<br>
    string ProfessorStatsIndexToString(List<int> statlist, int IndexBegin, int IndexEnd)
    {
        string ProfessorStats = "";
        for (int j = IndexBegin; j <= IndexEnd; ++j)
        {
            ProfessorStats += KoreanStatList[j];
            ProfessorStats += " ";
            ProfessorStats += statlist[j];
            ProfessorStats += "<br>";
        }
        return ProfessorStats;
    }


    /* ShowMessageForLimitedTime */
    //this function will show a GameObject for a limited amount of time.
    IEnumerator ShowMessageForLimitedTime(GameObject tt, float time)
    {
        tt.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        tt.SetActive(false);
    }

    void Awake()
    {

    }
    void Start()
    {
        /* // Initialization // */

        /* Step 1 */
        int ProfessorCount = PlayerInfo.ProfessorList.Count; // get the number of professors that the player currently has

        ProfessorCountInfo.text = Convert.ToString(ProfessorCount); //show number of professors on upper left corner

        //set Error messages as inactive (not shown to player)
        ProfessorAwayRefuseMessage.SetActive(false);
        ProfessorFireRefuseMessage.SetActive(false);
        PopUpObject.SetActive(false);

        //add Listener to Return To Menu button
        ReturnButton.onClick.AddListener(ReturnToMenu);

        if (ProfessorCount == 0)
        {
            NoProfessorsHired.SetActive(true); //player has not hired any professors
        }
        else
        {
            NoProfessorsHired.SetActive(false);
        }

        //List of GameObjects that store Professors
        List<GameObject> ProfessorObjects = new List<GameObject>();
        
        //instantiate GameObject objects that store professor data from prefab
        for (int i = 0; i < ProfessorCount; ++i)
        {
            ProfessorObjects.Add(Instantiate(ReferencePrefab, new Vector3(-1920, 0, 0), Quaternion.identity, content.transform));
        }

        //Professor type (in integer value)
        int ProfessorTypeInt;


        /* Step 2 */
        for (int i = 0; i < ProfessorCount; ++i)
        {
            int profIndex = i;
            ProfessorButtons.Add(ProfessorObjects[i].GetComponent<Button>()); //add button

            Debug.Log("Index Value : " + i);
            Debug.Log("Professor ID : " + PlayerInfo.ProfessorList[i].ProfessorGetID());
            Debug.Log("ProfessorButtons List length : " + ProfessorButtons.Count);
            Debug.Log("ProfessorList List length : " + PlayerInfo.ProfessorList.Count);
            ProfessorButtons[i].onClick.AddListener(() => ShowInfoOnPopup(profIndex));

            ProfessorInfo = ProfessorObjects[i].GetComponentsInChildren<TextMeshProUGUI>();

            /* Get Professor Information */

            ProfessorInfo[0].text = PlayerInfo.ProfessorList[i].ProfessorGetName();

            ProfessorInfo[1].text = PlayerInfo.ProfessorList[i].ProfessorGetTypeInString();

            ProfessorTypeInt = PlayerInfo.ProfessorList[i].ProfessorGetType();

            //turn professor stats to string
            ProfessorInfo[2].text = ProfessorStatsIndexToString(PlayerInfo.ProfessorList[i].ProfessorGetStats(), 0, 2);
            ProfessorInfo[3].text = ProfessorStatsIndexToString(PlayerInfo.ProfessorList[i].ProfessorGetStats(), 3, 5);

            ProfessorInfo[4].text = "급여 : " + Convert.ToString(PlayerInfo.ProfessorList[i].ProfessorGetSalary());

            //generate random skill values
            int index1 = 0, index2 = 0;
            while (index1 == index2)
            {
                index1 = randomseed.Next(0, 6);
                index2 = randomseed.Next(0, 6);
            }
            List<int> TempListIndex = new List<int>(2);
            List<int> TempListValue = new List<int>(2);
            TempListIndex.Add(Math.Min(index1, index2));
            TempListIndex.Add(Math.Max(index1, index2));
            PlayerInfo.UpgradeSkillIndex.Add(TempListIndex); //temporary, for testing, will not work in main
            Debug.Log("UpgradeSkillIndex Length : " + PlayerInfo.UpgradeSkillIndex.Count);
            //PlayerInfo.UpgradeSkillIndex[i] = TempListIndex;

            TempListValue.Add(randomseed.Next(ProfessorUpgradeValueMinimum, ProfessorUpgradeValueMaximum + 1));
            TempListValue.Add(randomseed.Next(ProfessorUpgradeValueMinimum, ProfessorUpgradeValueMaximum + 1));
            PlayerInfo.UpgradeSkillValue.Add(TempListValue); //temporary, for testing, will not work in main
            //PlayerInfo.UpgradeSkillValue[i] = TempListValue;
        }
    }

    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu");
        LoadingSceneManager.LoadScene("Main");
    }

    public void ShowInfoOnPopup(int idx)
    {
        Debug.Log("Entered ShowInfoOnPopup || ProfessorList Count : " + PlayerInfo.ProfessorList.Count + " || Index : " + idx);
        ProfessorSystem.Professor ProfData = PlayerInfo.ProfessorList[idx];
        Debug.Log("ShowInfoOnPopup called : " + idx);
        List<int> tempStatList = new List<int>(6);
        ExitPopupButton.onClick.AddListener(RemovePopup);
        PopupText = PopUpObject.GetComponentsInChildren<TextMeshProUGUI>();
        PopupText[0].text = ProfData.ProfessorGetName();
        PopupText[1].text = ProfData.ProfessorGetTypeInString();
        int profTypeInt = ProfData.ProfessorGetType();

        if (profTypeInt == 2)
        {
            PopupText[1].color = new Color32(238, 184, 196, 255);
        }
        else if (profTypeInt == 1)
        {
            PopupText[1].color = new Color32(230, 212, 123, 255);
        }
        if (ProfData.ProfessorGetAwayStatus() == true)
            PopupText[2].text = "출장중";
        else
            PopupText[2].text = "";
        PopupText[3].text = Convert.ToString(ProfData.ProfessorGetTenureInTurns());
        PopupText[4].text = Convert.ToString(ProfData.ProfessorGetSalary());
        List<int> PopupStatList = ProfData.ProfessorGetStats();
        string PopupStatStr = "";
        for (int i = 0; i < 6; ++i)
        {
            PopupStatStr += KoreanStatList[i];
            PopupStatStr += " : ";
            PopupStatStr += PopupStatList[i];
            PopupStatStr += "<br>";
        }
        PopupText[5].text = PopupStatStr;

        Debug.Log("Index : " + idx);
        Debug.Log("List Length : " + PlayerInfo.UpgradeSkillIndex.Count);
        Debug.Log(PlayerInfo.UpgradeSkillIndex[idx].Count);
        Debug.Log(PlayerInfo.UpgradeSkillIndex[idx][0]);
        Debug.Log(PlayerInfo.UpgradeSkillIndex[idx][1]);
        int UpgradeIndex1 = PlayerInfo.UpgradeSkillIndex[idx][0];
        int UpgradeIndex2 = PlayerInfo.UpgradeSkillIndex[idx][1];
        int UpgradeValue1 = PlayerInfo.UpgradeSkillValue[idx][0];
        int UpgradeValue2 = PlayerInfo.UpgradeSkillValue[idx][1];
        //UpgradeStatInfo1.text = KoreanStatList[UpgradeIndex1];
        //UpgradeStatInfo2.text = KoreanStatList[UpgradeIndex2];

        UpgradeButton.onClick.RemoveAllListeners();
        AwayButton.onClick.RemoveAllListeners();
        FireButton.onClick.RemoveAllListeners();
        UpgradeButton.onClick.AddListener(() => UpgradeStats(idx, UpgradeIndex1, UpgradeIndex2, UpgradeValue1, UpgradeValue2));
        AwayButton.onClick.AddListener(() => ProfessorSendAway(ProfData));
        FireButton.onClick.AddListener(() => FireProfessor(ProfData));

        PopUpObject.SetActive(true);
    }

    public void RemovePopup()
    {
        Debug.Log("Remove Popup call");
        PopUpObject.SetActive(false);
    }

    public void UpgradeStats(int ProfessorIndex, int StatIndex1, int StatIndex2, int StatValue1, int StatValue2)
    {
        Debug.Log("UpgradeStats | ProfIndex :" + ProfessorIndex + "   Stat1 : "  + StatIndex1 + "  Stat2 : " + StatIndex2);
        PlayerInfo.ProfessorList[ProfessorIndex].ProfessorUpgradeStat(StatIndex1, StatValue1);
        PlayerInfo.ProfessorList[ProfessorIndex].ProfessorUpgradeStat(StatIndex2, StatValue2);
    }

    public void ProfessorSendAway(ProfessorSystem.Professor ProfData)
    {
        Debug.Log("ProfessorSendAway");
        bool flag = true;
        for (int i = 0; i < PlayerInfo.studentGroups.Count; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (SubjectTree.canRemoveProfessor(ProfData.ProfessorGetID(), PlayerInfo.studentGroups[i][j].GetCurriculum()) == false)
                {
                    goto ExitLoop;
                }
            }
        }
        ExitLoop:
        Debug.Log("ExitLoop Passed");
        if (flag)
        {
            PlayerInfo.ProfessorList[PlayerInfo.ProfessorList.FindIndex(x => x.ProfessorGetID() == ProfData.ProfessorGetID())].ProfessorSetAwayStatus(true, randomseed.Next(1, 3));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            StartCoroutine(ShowMessageForLimitedTime(ProfessorAwayRefuseMessage, 1.0f));
        }
    }
    public void FireProfessor(ProfessorSystem.Professor ProfData)
    {
        Debug.Log("FireProfessor");
        Debug.Log("COUNTS : " + PlayerInfo.studentGroups.Count);
        bool flag = true;
        for (int i = 0; i < PlayerInfo.studentGroups.Count; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (SubjectTree.canRemoveProfessor(ProfData.ProfessorGetID(), PlayerInfo.studentGroups[i][j].GetCurriculum()) == false)
                {
                    goto ExitLoop;
                }
            }
        }
        ExitLoop:
        Debug.Log("ExitLoop Passed");
        Debug.Log("COUNTS : " + PlayerInfo.studentGroups.Count);
        if (flag)
        {
            PlayerInfo.ProfessorList.RemoveAt(PlayerInfo.ProfessorList.FindIndex(x => x.ProfessorGetID() == ProfData.ProfessorGetID()));
            int achieveCode = 12; // 업적 - 귀하의 노고에 감사드리며...
            AchievementManager.Achieve(achieveCode);
            achieveCode = 11;
            AchievementManager.CreateLocalStat(achieveCode);
            AchievementManager.localStat[achieveCode]++;
            if (AchievementManager.localStat[achieveCode] == 10)
                AchievementManager.Achieve(achieveCode);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            StartCoroutine(ShowMessageForLimitedTime(ProfessorFireRefuseMessage, 1.0f));
            //ProfessorAwayRefuseMessage.enabled = true;
        }
    }
}

/*

- 매 턴마다 모든 교슈의 속성 중 랜덤으로 2개를 강화 가능
ex) A교수 속성1, 속성3 / B교수 속성5, 속성6

구현 todo:
- 속성 랜덤 2개 선택
- 강화 버튼 & UI
- 강화 성공 메세지
- 강화 데이터 전송&수정

버그 todo:
- (suggestion) change GetComponentInChildren to GetComponent
*/

/*
Get student groups from StudentsManager.cs
iterate through them all using the fucntions from SubjectManager.cs (subjectTree.canRemoveProfessor)

*/