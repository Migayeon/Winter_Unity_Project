using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

class info
{
    public int testclass;
    public string testname;
    public List<int> require = new List<int>();
}

public class TestCheckManager : MonoBehaviour
{
    public GameObject testcase;
    public Transform testContent;

    public GameObject student;
    public Transform studentContent;

    List<int> getStat()
    {
        List<int> stat = new List<int>();
        return stat;
        // [이론, 마나, 손재주, 원소, 영창]
        // [theory, mana, craft, element, attack]
    }

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(prefab);
        for (int i = 1; i < 16; i++)
        {
            var loadedJson = Resources.Load<TextAsset>("TestCase/" + i.ToString());
            
            info testInfo = JsonUtility.FromJson<info>(loadedJson.ToString());
            Debug.Log($"{testInfo.testclass}, {testInfo.testname}, {testInfo.require}");

            var loadedSprite = Resources.Load<Sprite>("UI/Test_Section/" + testInfo.testclass.ToString());
            Debug.Log(testInfo.testclass.ToString());
            GameObject test = Instantiate(testcase, testContent);

            test.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = testInfo.testname;
            test.transform.GetChild(2).GetComponent<Image>().sprite = loadedSprite;
        }

        for (int i = 0; i<PlayerInfo.studentGroups.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                StudentGroup studentgroup = PlayerInfo.studentGroups[i][j];
                GameObject studentInfo = Instantiate(student, studentContent);
                studentInfo.transform.GetChild(0).GetComponent<Text>().text =
                    $"{studentgroup.GetPeriod()}기 {studentgroup.GetDivision()}분반";
                studentInfo.transform.GetChild(1).GetComponent<Text>().text =
                    $"{8 - studentgroup.GetAge()}턴 뒤 시험";
                studentInfo.GetComponent<Button>().onClick.AddListener(delegate { StudentClicked(studentgroup.GetStat()); });
            }
        }
    }

    public void StudentClicked(List<int> stat)
    {
        // [이론, 마나, 손재주, 원소, 영창]

        /*
         
        현재 학생들의 스탯을 리스트로 넘겨받는 기능 구현.
        나머지는 연욱씨에게...
         
         */


        // 학생 스텟 기반으로 확률 계산
        // 진학사처럼 표현
    }
}
