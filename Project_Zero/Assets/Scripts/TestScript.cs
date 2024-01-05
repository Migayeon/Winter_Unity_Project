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

public class Example : MonoBehaviour
{
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

    public TextMeshProUGUI[] ProfessorInfo = new TextMeshProUGUI[5];
    public TextMeshProUGUI[] PopupText = new TextMeshProUGUI[6];

    public Dictionary<int, string> KoreanStatList = new Dictionary<int, string>(6)
        {
            {0, "강의력"},
            {1, "마법이론"},
            {2, "마나감응"},
            {3, "손재주"},
            {4, "속성력"},
            {5, "영창"},
        };

    void Start()
    {
        int ProfessorCount = PlayerInfo.ProfessorList.Count;
        Debug.Log(ProfessorCount);

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
        List<GameObject> ProfessorInfoObjects = new List<GameObject>(ProfessorCount);


        List<TextMeshProUGUI> ProfessorTMPData = new List<TextMeshProUGUI>(5);

        //instantiate Button objects that store professor data from prefab
        for (int i = 0; i < ProfessorCount; ++i)
        {
            ProfessorInfoObjects.Add(Instantiate(ReferencePrefab, new Vector3(0, 0, 0), Quaternion.identity, content.transform));
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
            ProfessorInfoButton.Add(ProfessorInfoObjects[i].GetComponent<Button>()); //add button
            int idx = i;
            ProfessorInfoButton[i].onClick.AddListener(() => ShowInfoOnPopup(PlayerInfo.ProfessorList[idx])); //change later (from TempProfessorList to actual Player data list)
            Debug.Log(ProfessorInfoButton.Count);
            // change index from 0 to (other variable)
            Debug.Log("CHECK");
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
        }
        ReturnButton.onClick.AddListener(ReturnToMenu);
    }

    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu");
        SceneManager.LoadScene("Main");
    }
    public void ShowInfoOnPopup(ProfessorSystem.Professor ProfData)
    {
        Debug.Log("ShowInfoOnPopup called");
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
        PopUpObject.SetActive(true);
    }
    public void RemovePopup()
    {
        Debug.Log("Remove Popup call");
        PopUpObject.SetActive(false);
    }
}

/*

- 매 턴마다 모든 교슈의 속성 중 랜덤으로 2개를 강화 가능
ex) A교수 속성1, 속성3 / B교수 속성5, 속성6

*/