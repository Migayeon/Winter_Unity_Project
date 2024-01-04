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
    
    public GameObject ReferencePrefab;
    public GameObject content;
    private GameObject myPrefab;

    public Button ReturnButton;

    public TextMeshProUGUI ProfessorName;
    public TextMeshProUGUI ProfessorType;
    public TextMeshProUGUI ProfessorSalary;
    public TextMeshProUGUI ProfessorStat1, ProfessorStat2;
    public TextMeshProUGUI[] ProfessorInfo = new TextMeshProUGUI[5];

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
        //To be implemented after PlayerInfo correctly stores current runtime data;
        /*
        int ProfessorCount = PlayerInfo.ProfessorList.Count;
        Debug.Log(ProfessorCount);
        */
        int ProfessorCount = 10;
        //temporary professor list;
        List<ProfessorSystem.Professor> TempProfessorList = new List<ProfessorSystem.Professor>(ProfessorCount);
        for (int i = 0; i < 10; ++i)
        {
            TempProfessorList.Add(CreateProfessor.CreateNewProfessor(i));
        }
        List<GameObject> ProfessorInfoObjects = new List<GameObject>(ProfessorCount);
        List<TextMeshProUGUI> ProfessorTMPData = new List<TextMeshProUGUI>(5);

        for (int i = 0; i < ProfessorCount; ++i)
        {
            ProfessorInfoObjects.Add(Instantiate(ReferencePrefab, new Vector3(0, 0, 0), Quaternion.identity, content.transform));
        }
        List<int> tempStatList = new List<int>(ProfessorSystem.professorStats);
        string tempStatStr = "";
        for (int i = 0; i < ProfessorCount; ++i)
        {
            ProfessorInfo = ProfessorInfoObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
            ProfessorInfo[0].text = TempProfessorList[i].ProfessorGetName();
            ProfessorInfo[1].text = TempProfessorList[i].ProfessorGetTypeInString();
            tempStatList = TempProfessorList[i].ProfessorGetStats();
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
            ProfessorInfo[4].text = "급여 : " + Convert.ToString(TempProfessorList[i].ProfessorGetSalary());
        }
        ReturnButton.onClick.AddListener(ReturnToMenu);
    }
    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu");
        SceneManager.LoadScene("Main");
    }
}

/*

- 매 턴마다 모든 교슈의 속성 중 랜덤으로 2개를 강화 가능
ex) A교수 속성1, 속성3 / B교수 속성5, 속성6

*/