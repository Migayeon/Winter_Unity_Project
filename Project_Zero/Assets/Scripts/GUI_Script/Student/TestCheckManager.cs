using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class Info
{
    public int testclass;
    public string testname;
    public List<int> require = new List<int>();
}

public class TestCheckManager : MonoBehaviour
{
    [SerializeField]
    private GameObject realExam;
    public GameObject testcase;
    public Transform testContent;

    public GameObject student;
    public Transform studentContent;
    public StudentGroup currentSelectedGroup = null;
    private int currentSelectedTest = -1;

    private GameObject[] testButton = new GameObject[15];
    private Info[] testInfo = new Info[15];
    List<Info> infoList = new List<Info>();

    // Start is called before the first frame update
    void Start()
    {
        realExam.SetActive(false);
        //Instantiate(prefab);
        for (int i = 0; i < 15; i++)
        {
            var loadedJson = Resources.Load<TextAsset>("TestCase/" + i.ToString());

            testInfo[i] = JsonUtility.FromJson<Info>(loadedJson.ToString());
            Debug.Log($"{testInfo[i].testclass}, {testInfo[i].testname}, {testInfo[i].require}");
            infoList.Add(testInfo[i]);

            var loadedSprite = Resources.Load<Sprite>("UI/Test_Section/" + testInfo[i].testclass.ToString());
            Debug.Log(testInfo[i].testclass.ToString());
            testButton[i] = Instantiate(testcase, testContent);

            testButton[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = testInfo[i].testname;
            testButton[i].transform.GetChild(2).GetComponent<Image>().sprite = loadedSprite;
            int j = i;
            testButton[i].GetComponent<Button>().onClick.AddListener(delegate { TestClicked(j); });
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
                Debug.Log("잘 됨");
                studentInfo.GetComponent<Button>().onClick.AddListener(delegate { StudentClicked(studentgroup); });
            }
        }
    }
    public void TestClicked(int idx)
    {
        realExam.SetActive(true);
        if (currentSelectedGroup != null) 
        {
            currentSelectedTest = idx;
            realExam.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                $"{currentSelectedGroup.GetPeriod()}기 {currentSelectedGroup.GetDivision()}분반" +
                $"\n\"{testInfo[idx].testname}\"시험을 \n보도록 하겠습니까?";
        }
        else
        {
            realExam.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "학생 그룹을 선택해 주세요";
        }

    }
    public void ConfirmTest()
    {
        currentSelectedGroup.SetExam(currentSelectedTest);
    }
    public void StudentClicked(StudentGroup stg)
    {
        // [이론, 마나, 손재주, 원소, 영창]
        currentSelectedGroup = stg;
        // Debug.Log(currentSelectedGroup.GetDivision());
        List<int> stat=stg.GetStat();
        int cnt = 0;
        
        GameObject[] gm = GameObject.FindGameObjectsWithTag("TestCase");
        foreach(GameObject g in gm)
        {
            float possibility = 0;
            Debug.Log(stat[0]);
            if (stat[0] > infoList[cnt].require[0])
            {
                possibility += 10;
                possibility += Mathf.Log(stat[0] - infoList[cnt].require[0]) * 2;
            }
            if (stat[1] > infoList[cnt].require[1])
            {
                possibility += 10;
                possibility += Mathf.Log(stat[1] - infoList[cnt].require[1]) * 2;
            }
            if (stat[2] > infoList[cnt].require[2])
            {
                possibility += 10;
                possibility += Mathf.Log(stat[2] - infoList[cnt].require[2]) * 2;
            }
            if (stat[3] > infoList[cnt].require[3])
            {
                possibility += 10;
                possibility += Mathf.Log(stat[3] - infoList[cnt].require[3]) * 2;
            }
            if (stat[4] > infoList[cnt].require[4])
            {
                possibility += 10;
                possibility += Mathf.Log(stat[4] - infoList[cnt].require[4]) * 2;
            }

            var loadedSprite1 = Resources.Load<Sprite>("UI/Test_Section/" + "0to20");
            var loadedSprite2 = Resources.Load<Sprite>("UI/Test_Section/" + "20to40");
            var loadedSprite3 = Resources.Load<Sprite>("UI/Test_Section/" + "40to60");
            var loadedSprite4 = Resources.Load<Sprite>("UI/Test_Section/" + "60to80");
            var loadedSprite5 = Resources.Load<Sprite>("UI/Test_Section/" + "80to100");

            if (possibility >= 0 && possibility < 20)
            {
                g.transform.GetChild(3).GetComponent<Image>().sprite = loadedSprite1;
            }
            else if (possibility >= 20 && possibility < 40)
            {
                g.transform.GetChild(3).GetComponent<Image>().sprite = loadedSprite2;
            }
            if (possibility >= 40 && possibility < 60)
            {
                g.transform.GetChild(3).GetComponent<Image>().sprite = loadedSprite3;
            }
            if (possibility >= 60 && possibility < 80)
            {
                g.transform.GetChild(3).GetComponent<Image>().sprite = loadedSprite4;
            }
            if (possibility >= 80)
            {
                g.transform.GetChild(3).GetComponent<Image>().sprite = loadedSprite5;
            }

            /* 합격확률 계산 방법 (ver 1)
             * 
             * 총 5개의 능력치를 기반으로 계산을 한다.
             * [이론, 마나, 손재주, 원소, 영창] 순서이다.
             * 
             * 해당 시험의 '이론' 능력치 요구량을 넘은 학생 집단이면
             * 10%의 합격 확률을 얻는다.
             * 또한 학생 집단의 '이론' 능력치 - 해당 시험의 '이론' 요구량에
             * 로그를 씌우고 * 2 를 한다. 이를 다시 합격확률에 더한다.
             * 
             * (그 이유는 특정 능력치에 대해 학생 집단이 너무 뛰어날 경우
             * 이를 선형적으로 반영하면 다른 능력치가 약해도 합격 확률이 매우 높아지기 때문)
             * 
             * 이 과정을 각 능력치에 대해 반복한다. (총 5번 계산하게 된다)
             * 
             * 현재 합격 확률이 높은 이유는 테스트를 위해서
             * 시험의 요구 능력치를 매우 낮춘 상태이다. 앞으로 조정하자.
             */

            cnt++;
        }

        // 학생 스텟 기반으로 확률 계산 OK
        // 진학사처럼 표현 OK
        }
    }
