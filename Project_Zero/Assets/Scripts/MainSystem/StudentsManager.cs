using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StudentGroup
{
    //SubjectManager.SubjectTree s = new SubjectManager.SubjectTree();
    public StudentGroup(int _d, int _n, int _a, int _co, int[,] _c, int[] _st)
    {
        division = _d;
        number = _n;
        age = _a;
        cost = _co;
        curriculum = _c;
        stat = _st;
    }
    public StudentGroup() { }

    private int division; // 분반
    private int number; // 학생 수
    private int age; // 공부 기간
    private int cost; // 학원비
    private int[,] curriculum; // 커리큘럼
    private int[] stat; // 스탯

    public int GetDivision(){ return division; }
    public int GetNumber() { return number; }
    public int GetAge() { return age; }
    public int GetCost() { return cost; }
    public int[,] GetCurriculum() { return curriculum; }
    public int[] GetStat() { return stat; }

    public void CurriculumSequence()
    {
        if (age == 8)
        {
            SelectTest();

            return;
        }
        //List<int> result = ;
        curriculum[age, 1] = 1;
        age++;
    }

    public void SelectTest()
    {
        /*
         
        시험 선택 함수. 

         */
    }
    
}

public class StudentsManager : MonoBehaviour
{
    public static StudentGroup CreateStudentGroup(int div, int num, int cost)
    {
        int[,] curriculum = null; // 커리큘럼 호출 함수 연결
        int[] stat = new int[5];
        int lim = 0;
        int statSum = Random.Range(300, 400);
        int lastSum = statSum;

        for (int i = 0; i < 5; i++)
        {
            stat[i] = 10 + statSum * (Random.Range(lim, lim + 20)-lim) / 100;
            lastSum -= stat[i];
        }
        for (int i = 0; i < lastSum; i++)
        {
            if (stat[i % 5] >= 100)
            {
                lastSum++;
                continue;
            }
            stat[i%5]++;
        }
        StudentGroup studentGroup = new StudentGroup(div, num, 0, cost, curriculum, stat);
        return studentGroup;
    }


    private void Awake()
    {
        for (int i = 0; i < 20; i++)
        {
            CreateStudentGroup(1, 1, 1);
        }
    }
}
