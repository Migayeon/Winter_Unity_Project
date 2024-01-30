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
    [SerializeField]
    private Transform subjectInfoUI;
    [SerializeField]
    private ESC_Manager ESC_Script;

    public StudentGroup[] studentGroup = new StudentGroup[3];

    private int div = 0;
    private int num = 0;
    private int sum = 0;
    private int localMinimum;
    private int coefficient;
    private bool isShift;
    private bool selectFixed;

    public enum returnState
    {
        haveClosedSubject,
        noProfessor,
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
        else if (SubjectTree.professorInSubjectCnt[i] == 0)
        {
            StartCoroutine(WarningMessage("교수를 배치하지 않은 과목입니다."));
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
            subjectInfoUI.gameObject.SetActive(true);
            selectFixed = true;
            if (!SubjectTree.isVaildCurriculum(CurriculumList))
            {
                CurriculumList.Remove(i);
                returnState tmp = followCurriculum(i);
                if (tmp == returnState.overMaximum)
                {
                    StartCoroutine(WarningMessage("커리큘럼 길이가 최대를 초과하여 일부 하위 과목만 선택했습니다.", 2));
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
                        if (SubjectTree.professorInSubjectCnt[curriForClickedSubject[i]] == 0)
                        {
                            StartCoroutine(WarningMessage("아직 교수를 배치하지 않은 과목이 있습니다."));
                            flag = returnState.noProfessor;
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
                if (SubjectTree.professorInSubjectCnt[curriForClickedSubject[i]] == 0)
                {
                    StartCoroutine(WarningMessage("아직 교수를 배치하지 않은 과목이 있습니다."));
                    flag = returnState.noProfessor;
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
        selectFixed = false;
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
        if (num + PlayerInfo.StudentGroupCount() + sum > PlayerInfo.maxStudent) 
        {
            num = Mathf.Max(0, PlayerInfo.maxStudent - PlayerInfo.StudentGroupCount() - sum);
        }
        div++;
        studentGroup[div - 1] = new StudentGroup(div, num, PlayerInfo.cost);
        foreach (var subject in subjectGameobject.transform.GetComponentsInChildren<Button>())
        {
            subject.image.color = Color.white;
            subject.transform.GetChild(0).gameObject.SetActive(false);
        }
        CurriculumList = new List<int>();
        if (num > 0)
        {
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
        else
        {
            foreach (var subject in subjectGameobject.transform.GetComponentsInChildren<Button>())
            {
                subject.onClick.RemoveAllListeners();
            }
            next.onClick.RemoveAllListeners();
            next.onClick.AddListener(SkipCurriculum);
            infoMessage.text = $"그룹{div}\r\n\r\n" +
                $"인원 수 : {num}명\r\n\r\n" +
                "Stat\r\n" +
                "\r\n" +
                "해당 분반에 학생이 없습니다\r\n" +
                "\r\n" +
                "커리큘럼을 분배할 수 없습니다\r\n"+
                "다음 버튼을 눌러주세요";
        }
        sum += num;
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

    public void SkipCurriculum()
    {
        for (int i = 0; i < 8; i++) CurriculumList.Add(0);
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

    IEnumerator WarningMessage(string message, float time = 1.0f)
    {
        warningMessage.text = message;
        warningMessage.enabled = true;
        yield return new WaitForSeconds(time);
        warningMessage.enabled = false;
    }

    private void Awake()
    {
        sum = 0;
        coefficient = UnityEngine.Random.Range(8000, 10000);
        localMinimum = UnityEngine.Random.Range(400, 500);
        num = PlayerInfo.cost - localMinimum;
        GoodsManager.CalculateEndedFame();
        num = (num * num / coefficient) * (GoodsManager.goodsCalculatedEndedFame / 100);
        //Debug.Log($"학생수 : {num}\n 명성 : {GoodsManager.goodsCalculatedEndedFame}");
        warningMessage.enabled = false;
        foreach (var subject in subjectGameobject.transform.GetComponentsInChildren<Button>())
        {
            subject.onClick.AddListener(delegate { SubjectClick(Convert.ToInt32(subject.name)); });
        }

        next.onClick.AddListener(SaveCurriculum);
        NewCurriculum();
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            isShift = true;
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            isShift = false;
        if (ESC_Script.isPause) return;
        if (!selectFixed)
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray2 = new Ray2D(mp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
            if (hit.collider != null)
            {
                Transform nowTransform = hit.collider.transform;
                int nowTransformId = int.Parse(nowTransform.name);
                setSubjectInfoUI(nowTransformId);
            }
            else
            {
                subjectInfoUI.gameObject.SetActive(false);
            }
        }
    }
    public void setSubjectInfoUI(int id)
    {
        subjectInfoUI.gameObject.SetActive(true);
        Subject subjectInfo = SubjectTree.getSubject(id);
        subjectInfoUI.GetChild(1).GetComponent<TMP_Text>().text = subjectInfo.name;
        List<int> enforceInfo = subjectInfo.enforceContents;
        string tmpText = "";
        for (int i = 0; i < enforceInfo.Count; i++)
        {
            if (enforceInfo[i] != 0)
                tmpText += SubjectTree.subjectsInfo.enforceTypeName[i] + " + " + enforceInfo[i] + "%\n";
        }
        subjectInfoUI.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = tmpText;
    }
}
