using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CurriculumSetting : MonoBehaviour
{
    public List<int> CurriculumList;

    public Transform manager;
    public Text warningMessage;
    public Text infoMessage;
    public GameObject subjectGameobject;
    public Button next;

    public StudentGroup[] studentGroup = new StudentGroup[3];

    private int div = 0;
    private int num = 0;
    private int localMinimum;
    private int coefficient;
    private bool isShift;

    public enum returnState
    {
        haveClosedSubject,
        overMaximum,
        normal
    }

    public void SubjectClick(int i)
    {
        if (SubjectTree.subjectState[i] != SubjectTree.State.Open)
        {
            StartCoroutine(WarningMessage("아직 해금하지 않은 과목입니다."));
            return;
        }
        if (CurriculumList.Contains(i))
        {
            CurriculumCancel(i);
            return;
        }
        if (CurriculumList.Count == 8)
        {
            if (!isShift)
            {
                followCurriculum(i);
            }
            else
            {
                StartCoroutine(WarningMessage("커리큘럼의 길이가 최대입니다."));
            }
            return;
        }
        else
        {
            CurriculumList.Add(i);
            if (!SubjectTree.isVaildCurriculum(CurriculumList))
            {
                CurriculumList.Remove(i);
                returnState tmp = followCurriculum(i);
                if (tmp == returnState.overMaximum)
                {
                    StartCoroutine(WarningMessage("커리큘럼 길이가 최대를 초과하여 일부 하위 과목만 선택했습니다."));
                }
                else if (tmp == returnState.haveClosedSubject)
                {
                    StartCoroutine(WarningMessage("아직 해금하지 않은 과목이 있습니다."));
                }
                /* CurriculumList.Remove(i);
                 * StartCoroutine(WarningMessage("해당 과목의 이수 조건을 만족하지 못했습니다."));
                 */
                return;
            }
            Image status = subjectGameobject.transform.GetChild(i).GetComponent<Image>();
            status.color = Color.green;
            GameObject order = status.transform.GetChild(0).gameObject;
            order.GetComponent<Text>().text = CurriculumList.Count.ToString();
            order.SetActive(true);
            manager.GetComponent<CurriculumTreeDrawingManager>().drawTree(CurriculumList);
        }
    }

    public returnState followCurriculum(int queryId)
    {
        returnState flag = returnState.normal;
        for (int j = 0; j < CurriculumList.Count; j++)
        {
            Image statusImg = subjectGameobject.transform.GetChild(CurriculumList[j]).GetComponent<Image>();
            statusImg.color = Color.white;
            statusImg.transform.GetChild(0).gameObject.SetActive(false);
        }
        List<int> curriForClickedSubject = SubjectTree.getCurriculumFor(queryId);
        if (isShift)
        {
            for (int i = 0; i < curriForClickedSubject.Count; i++)
            {
                if (!CurriculumList.Contains(curriForClickedSubject[i]))
                {
                    if (CurriculumList.Count < 8)
                    {
                        if (SubjectTree.subjectState[curriForClickedSubject[i]] != SubjectTree.State.Open)
                        {
                            StartCoroutine(WarningMessage("아직 해금하지 않은 과목이 있습니다."));
                            flag = returnState.haveClosedSubject;
                            break;
                        }
                        CurriculumList.Add(curriForClickedSubject[i]);
                    }
                    else
                    {
                        flag = returnState.overMaximum;
                        break;
                    }
                }
            }
        }
        else
        {
            CurriculumList = new List<int>();
            for (int i = 0; i < curriForClickedSubject.Count; i++)
            {
                if (SubjectTree.subjectState[curriForClickedSubject[i]] != SubjectTree.State.Open)
                {
                    StartCoroutine(WarningMessage("아직 해금하지 않은 과목이 있습니다."));
                    flag = returnState.haveClosedSubject;
                    break;
                }
                CurriculumList.Add(curriForClickedSubject[i]);
            }
        }
        for (int j = 0; j < CurriculumList.Count; j++)
        {
            Image statusImg = subjectGameobject.transform.GetChild(CurriculumList[j]).GetComponent<Image>();
            statusImg.color = Color.green;
            GameObject orderGameObj = statusImg.transform.GetChild(0).gameObject;
            orderGameObj.GetComponent<Text>().text = (j + 1).ToString();
            orderGameObj.SetActive(true);
        }
        manager.GetComponent<CurriculumTreeDrawingManager>().drawTree(CurriculumList);
        return flag;
    }

    public void CurriculumCancel(int i)
    {
        int time = CurriculumList.Count - CurriculumList.IndexOf(i);
        int next = i;
        for (int j = 0; j < time; j++)
        {
            int subject = CurriculumList[CurriculumList.Count - 1];
            Image status = subjectGameobject.transform.GetChild(subject).GetComponent<Image>();
            status.color = Color.white;
            status.transform.GetChild(0).gameObject.SetActive(false);
            CurriculumList.RemoveAt(CurriculumList.Count - 1);
            next = subject;
        }
        manager.GetComponent<CurriculumTreeDrawingManager>().drawTree(CurriculumList);
    }

    public void NewCurriculum()
    {
        CurriculumList = new List<int>();
        div++;
        studentGroup[div - 1] = new StudentGroup(div, num, PlayerInfo.cost);
        foreach (var subject in subjectGameobject.transform.GetComponentsInChildren<Button>())
        {
            subject.image.color = Color.white;
            subject.transform.GetChild(0).gameObject.SetActive(false);
        }
        List<int> stat = studentGroup[div - 1].GetStat();
        infoMessage.text = $"그룹{div}\r\n\r\n" +
            $"인원 수 : {num}명\r\n\r\n" +
            "Stat\r\n" +
            $"마법 이론 : {stat[0]} \r\n" +
            $"마나 감응 : {stat[1]}\r\n" +
            $" 손재주   : {stat[2]}\r\n" +
            $" 속성력   : {stat[3]}\r\n" +
            $"  영창    : {stat[4]}";
    }

    public void SaveCurriculum()
    {
        if (CurriculumList.Count < 8)
        {
            StartCoroutine(WarningMessage("커리큘럼은 8과목으로 구성되어야 합니다."));
            return;
        }
        studentGroup[div - 1].SetCurriCulum(CurriculumList);
        if (div == 3)
        {
            PlayerInfo.studentGroups.Add(studentGroup);
            //GoodsUIUpdate.ShowUI();
            SceneManager.LoadScene("BeforeTurn");
            return;
        }
        NewCurriculum();
        manager.GetComponent<CurriculumTreeDrawingManager>().drawTree(CurriculumList);
    }

    IEnumerator WarningMessage(string message)
    {
        warningMessage.text = message;
        warningMessage.enabled = true;
        yield return new WaitForSeconds(1.0f);
        warningMessage.enabled = false;
    }

    private void Awake()
    {
        coefficient = UnityEngine.Random.Range(9000, 12000);
        localMinimum = UnityEngine.Random.Range(320, 400);
        num = PlayerInfo.cost - localMinimum;
        Debug.Log(num);
        GoodsManager.CalculateEndedFame();
        num = (num * num) / coefficient * GoodsManager.goodsCalculatedEndedFame;
        Debug.Log(num);

        warningMessage.enabled = false;
        foreach (var subject in subjectGameobject.transform.GetComponentsInChildren<Button>())
        {
            subject.onClick.AddListener(delegate { SubjectClick(Convert.ToInt32(subject.name)); });
        }

        NewCurriculum();
        next.onClick.AddListener(SaveCurriculum);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            isShift = true;
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            isShift = false;
    }
}
