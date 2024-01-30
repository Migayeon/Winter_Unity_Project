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
public class ManageProfessorTest : MonoBehaviour
{
    public const int ProfessorAwayTime = 3;

    //min, max values for the "upgrade professor by how much" question (random generation)
    public const int ProfessorUpgradeValueMinimum = 50;
    public const int ProfessorUpgradeValueMaximum = 200;
    
    public GameObject ReferencePrefab; //reference Prefab (ProfessorInfo file)
    public GameObject content; //GameObject for spawning new instances of the reference prefab

    public Button ReturnButton; //Button for Return to Main Menu option

    public Button ExitPopupButton; //Button for exiting the Professor Information popup

    public GameObject PopUpObject; //GameObject for spawning the Professor Information popup

    public List<Button> ProfessorInfoButton = new List<Button>(); //List of Professors (as Button object)

    public TextMeshProUGUI PopupTitleText;
    public TextMeshProUGUI ProfessorName;
    public TextMeshProUGUI ProfessorType;
    public TextMeshProUGUI ProfessorSalary;
    public TextMeshProUGUI ProfessorStat;

    public TextMeshProUGUI UpgradeStatInfo1;
    public TextMeshProUGUI UpgradeStatInfo2;
    public TextMeshProUGUI UpgradeStatValue1;
    public TextMeshProUGUI UpgradeStatValue2;

    public TextMeshProUGUI[] ProfessorInfo = new TextMeshProUGUI[5];
    public TextMeshProUGUI[] PopupText = new TextMeshProUGUI[6];

    public TextMeshProUGUI ProfessorCountInfo;

    public GameObject ProfessorAwayRefuseMessage;
    public GameObject ProfessorFireRefuseMessage;

    public Button UpgradeButton1, UpgradeButton2;
    public Button AwayButton;
    public Button FireButton;

    public GameObject NoProfessorsHired;

    public Dictionary<int, string> KoreanStatList = new Dictionary<int, string>(6)
        {
            {0, "강의력"},
            {1, "마법이론"},
            {2, "마나감응"},
            {3, "손재주"},
            {4, "속성력"},
            {5, "영창"},
        };

    public System.Random randomseed = new System.Random();
    void Start()
    {
        Debug.Log("====================== MANAGEPROFESSOR.CS START ======================");

        int ProfessorCount = PlayerInfo.ProfessorList.Count;
        Debug.Log("ProfessorCount : " + ProfessorCount);

        ProfessorAwayRefuseMessage.SetActive(false);
        ProfessorFireRefuseMessage.SetActive(false);

        ProfessorCountInfo.text = Convert.ToString(ProfessorCount);

        if (ProfessorCount == 0)
        {
            NoProfessorsHired.SetActive(true);
        }
        else
        {
            NoProfessorsHired.SetActive(false);
        }
        PopUpObject.SetActive(false);

        //temporary # of professors
        //int ProfessorCount = 10;

        //temporary professor list;
        //List<ProfessorSystem.Professor> TempProfessorList = new List<ProfessorSystem.Professor>(ProfessorCount);
        /*
        for (int i = 0; i < 10; ++i)
        {
            PlayerInfo.ProfessorList.Add(CreateProfessor.CreateNewProfessor(i));
        }
        */
        //List of GameObjects that store professor information (prefab)

        List<GameObject> ProfessorInfoObjects = new List<GameObject>();
                                
        List<TextMeshProUGUI> ProfessorTMPData = new List<TextMeshProUGUI>(5);

        //instantiate Button objects that store professor data from prefab
        for (int i = 0; i < ProfessorCount; ++i)
        {
            ProfessorInfoObjects.Add(Instantiate(ReferencePrefab, new Vector3(-1920, 0, 0), Quaternion.identity, content.transform));
        }

        //Create list for storing professor stat data
        List<int> tempStatList = new List<int>(ProfessorSystem.professorStats);

        //Create temporary string for printing professor information (as string)
        string tempStatStr = "";

        //Professor type (in integer value)
        int profTypeInt;

        //for all professors
        for (int i = 0; i < ProfessorCount; ++i)
        {
            Debug.Log("Iteration : " + i);

            ProfessorInfoButton.Add(ProfessorInfoObjects[i].GetComponent<Button>()); //add button

            int idx = i;
            ProfessorInfoButton[i].onClick.AddListener(() => ShowInfoOnPopup(PlayerInfo.ProfessorList[idx], idx)); //change later (from TempProfessorList to actual Player data list)

            Debug.Log(ProfessorInfoButton.Count);
            // change index from 0 to (other variable)

            ProfessorInfo = ProfessorInfoObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
            ProfessorInfo[0].text = PlayerInfo.ProfessorList[i].ProfessorGetName();
            ProfessorInfo[1].text = PlayerInfo.ProfessorList[i].ProfessorGetTypeInString();
            Debug.Log(ProfessorInfo[1].text);
            Debug.Log(PlayerInfo.ProfessorList[i].ProfessorGetTypeInString());
            profTypeInt = PlayerInfo.ProfessorList[i].ProfessorGetType();
            if (profTypeInt == 2)
            {
                ProfessorInfo[1].color = new Color32(238, 184, 196, 255);
            }
            else if (profTypeInt == 1)
            {
                ProfessorInfo[1].color = new Color32(230, 212, 123, 255);
            }
            tempStatList = PlayerInfo.ProfessorList[i].ProfessorGetStats();
            tempStatStr = "";
            for (int j = 0; j < 3; ++j)
            {
                tempStatStr += KoreanStatList[j];
                tempStatStr += " ";
                tempStatStr += tempStatList[j];
                tempStatStr += "<br>";
            }
            ProfessorInfo[2].text = tempStatStr;
            tempStatStr = "";
            for (int j = 3; j < 6; ++j)
            {
                tempStatStr += KoreanStatList[j];
                tempStatStr += " ";
                tempStatStr += tempStatList[j];
                tempStatStr += "<br>";
            }
            ProfessorInfo[3].text = tempStatStr;
            ProfessorInfo[4].text = "급여 : " + Convert.ToString(PlayerInfo.ProfessorList[i].ProfessorGetSalary());

            //generate random skill indexes and delta values
            /* TODO
             * This part currently does not seem to work (no idea why)
             * will fix later
             */
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
            PlayerInfo.UpgradeSkillIndex[i] = TempListIndex;
            TempListIndex.Clear();

            TempListValue.Add(randomseed.Next(ProfessorUpgradeValueMinimum, ProfessorUpgradeValueMaximum + 1));
            TempListValue.Add(randomseed.Next(ProfessorUpgradeValueMinimum, ProfessorUpgradeValueMaximum + 1));
            PlayerInfo.UpgradeSkillValue[i] = TempListValue;
            TempListValue.Clear();
        }
        ReturnButton.onClick.AddListener(ReturnToMenu);
    }

    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu");
        SceneManager.LoadScene("Main");
    }

    public void ShowInfoOnPopup(ProfessorSystem.Professor ProfData, int idx)
    {
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
        //hijack for testing
        PopupText[2].text = "출장중";
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
        Debug.Log(PlayerInfo.UpgradeSkillIndex[idx][0]);
        Debug.Log(PlayerInfo.UpgradeSkillIndex[idx][1]);
        int UpgradeIndex1 = PlayerInfo.UpgradeSkillIndex[idx][0];
        int UpgradeIndex2 = PlayerInfo.UpgradeSkillIndex[idx][1];
        UpgradeStatInfo1.text = KoreanStatList[UpgradeIndex1];
        UpgradeStatInfo2.text = KoreanStatList[UpgradeIndex2];
        UpgradeStatValue1.text = Convert.ToString(PlayerInfo.UpgradeSkillValue[idx][0]);
        UpgradeStatValue2.text = Convert.ToString(PlayerInfo.UpgradeSkillValue[idx][1]);
        UpgradeButton1.onClick.AddListener(() => UpgradeStats(UpgradeIndex1));
        UpgradeButton2.onClick.AddListener(() => UpgradeStats(UpgradeIndex2));
        AwayButton.onClick.AddListener(() => ProfessorSendAway(ProfData));
        FireButton.onClick.AddListener(() => FireProfessor(ProfData));
        PopUpObject.SetActive(true);
    }
    public void RemovePopup()
    {
        Debug.Log("Remove Popup call");
        PopUpObject.SetActive(false);
    }

    public void UpgradeStats(int StatIndex)
    {
        Debug.Log("UpgradeStats : " + StatIndex);
        
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
            StartCoroutine(ShowMessageForLimitedTime(ProfessorAwayRefuseMessage));
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            StartCoroutine(ShowMessageForLimitedTime(ProfessorFireRefuseMessage));
            //ProfessorAwayRefuseMessage.enabled = true;
        }
    }
    IEnumerator ShowMessageForLimitedTime(GameObject tt)
    {
        tt.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        tt.SetActive(false);
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