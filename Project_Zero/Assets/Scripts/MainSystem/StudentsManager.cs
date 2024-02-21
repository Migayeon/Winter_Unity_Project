using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StudentGroup
{
    //SubjectManager.SubjectTree s = new SubjectManager.SubjectTree();
    public StudentGroup(int div, int num, int c)
    {
        period = TurnManager.turn - 1;
        division = div;
        number = num;
        age = 0;
        cost = c;
        curriculum = null; // 함수 연결
        stat = new List<int> { 0, 0, 0, 0, 0 };

        if (num > 0)
        {
            int lim = 0;
            int statSum = Random.Range(300, 400);
            int lastSum = statSum;

            for (int i = 0; i < 5; i++)
            {
                stat[i] = 10 + statSum * (Random.Range(lim, lim + 20) - lim) / 100;
                lastSum -= stat[i];
            }
            for (int i = 0; i < lastSum; i++)
            {
                if (stat[i % 5] >= 100)
                {
                    lastSum++;
                    continue;
                }
                stat[i % 5]++;
            }
        }
    }
    public StudentGroup() { }

    [SerializeField]
    private int period;

    [SerializeField]
    private int division; // 분반

    [SerializeField]
    private int number; // 학생 수

    [SerializeField] private int age; // 공부 기간
    [SerializeField] private int cost; // 학원비
    [SerializeField] private int exam = 0; // 볼 시험 종류(저장 구현해야함)
    [SerializeField] private int passedNum = 0; // 합격한 학생 수(저장 구현해야함)

    [SerializeField]
    private List<int> curriculum; // 커리큘럼

    [SerializeField]
    private List<int> stat; // 스탯

    public int GetPeriod() { return period; }
    public int GetDivision(){ return division; }
    public int GetNumber() { return number; }
    public int GetAge() { return age; }
    public int GetCost() { return cost; }
    public int GetExam() { return exam; }
    public int GetPassedNum() { return passedNum; }
    public List<int> GetCurriculum() { return curriculum; }
    public List<int> GetStat() { return stat; }

    public void SetCurriCulum(List<int> newCurri)
    {
        curriculum = newCurri;
        return;
    }
    public void SetExam(int idx)
    {
        exam = idx;
    }
    public void SetPassedNum(int num)
    {
        passedNum = num;
    }
    public void RandomStatUp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            stat[Random.Range(0, 5)]++;
        }
    }

    public bool CurriculumSequence()
    {
        Subject subject = SubjectTree.getSubject(curriculum[age]);
        int[] subjectWeight = new int[5] { 1, 2, 4, 6, 12 };
        List<int> enforceType = subject.enforceContents;
        for (int i = 0; i < enforceType.Count; i++)
        {
            stat[i] +=
                StudentsManager.professorInfoInSubject[subject.id, 0][0] *
                StudentsManager.professorInfoInSubject[subject.id, i + 1][0] *
                subjectWeight[subject.tier - 1] *
                enforceType[i] 
                / 2000000; 
        }
        age++;
        GoodsManager.goodsAr += cost * number;
        if (age == 8)
        {
            return false;
        }
        return true;
    }
    
}

public class StudentsManager : MonoBehaviour
{
    /*
    교수 탐색 후 과목마다 보너스를 찾아내는 함수... 

     1. afterTurn에서 사용합니다
     2. 첫번째 인덱스는 과목 번호, 두번째 인덱스는 스탯 번호, 리스트 내부에는 스탯들
     3. 한 과목에 여러명의 교수가 배치되면 어떻게 능력치를 부여할지... 
     
     */

    public static List<int>[,] professorInfoInSubject;

    public static void UpdateProfessorInfoInSubject()
    {
        professorInfoInSubject = new List<int>[30, 6];
        for(int i = 0; i < 30; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                professorInfoInSubject[i, j] = new List<int>();
            }
        }
        foreach (ProfessorSystem.Professor professor in PlayerInfo.ProfessorList)
        {
            List<int> subjectList = professor.ProfessorGetSubjects();
            List<int> statList = professor.ProfessorGetStats();
            for(int i = 0; i < subjectList.Count; i++)
            {
                for(int j = 0; j < 6; j++)
                {
                    Debug.Log($"{i} {j}");
                    Debug.Log(professorInfoInSubject[subjectList[i], j]);
                    professorInfoInSubject[subjectList[i], j].Add(statList[j]);
                }
            }
        }
    }
    
}
