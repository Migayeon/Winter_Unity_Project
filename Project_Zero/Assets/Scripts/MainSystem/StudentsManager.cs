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
        period = TurnManager.turn;
        division = div;
        number = num;
        age = 0;
        cost = c;
        curriculum = null; // 함수 연결
        stat = new List<int> { 0, 0, 0, 0, 0 };

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
    public StudentGroup(string data)
    {
        string[] dataArr =data.Split('/');
        period = int.Parse(dataArr[0]);
        division = int.Parse(dataArr[1]);
        number = int.Parse(dataArr[2]);
        age = int.Parse(dataArr[3]);
        cost = int.Parse(dataArr[4]);
        curriculum = new List<int>();
        stat = new List<int>();
        for (int i = 5; i < 13; i++)
        {
            curriculum.Add(int.Parse(dataArr[i]));
        }
        for (int i = 13; i <= 17; i++)
        {
            stat.Add(int.Parse(dataArr[i]));
        }
    }
    public StudentGroup() { }

    private int period;
    private int division; // 분반
    private int number; // 학생 수
    private int age; // 공부 기간
    private int cost; // 학원비
    private List<int> curriculum; // 커리큘럼
    private List<int> stat; // 스탯

    public int GetPeriod() { return period; }
    public int GetDivision(){ return division; }
    public int GetNumber() { return number; }
    public int GetAge() { return age; }
    public int GetCost() { return cost; }
    public List<int> GetCurriculum() { return curriculum; }
    public List<int> GetStat() { return stat; }

    public void SetCurriCulum(List<int> newCurri)
    {
        curriculum = newCurri;
        return;
    }

    public void RandomStatUp()
    {
        for (int i = 0; i < 10; i++)
        {
            stat[Random.Range(0, 4)]++;
        }
    }

    public void CurriculumSequence()
    {
        Subject subject = SubjectTree.getSubject(curriculum[age]);
        List<int> enforceType = subject.enforceContents;
        for (int i = 0; i < enforceType.Count; i++)
        {
            stat[i] += enforceType[i];
        }
        age++;
        GoodsManager.goodsAr += cost * number;
        if (age == 8)
        {
            Graduation();
            return;
        }
    }

    public void Graduation()
    {
        foreach (int i in stat)
        {
            Debug.Log(i);
        }
    }

    public string StudentDataToString()
    {
        string data =
            period.ToString() + '/' +
            division.ToString() + '/' +
            number.ToString() + '/' +
            age.ToString() + '/' +
            cost.ToString();
        foreach (int i in curriculum)
        {
            data += '/' + i.ToString();
        }
        foreach(int i in stat)
        {
            data += "/" + i.ToString();
        }
        return data;
    }
    
}

public class StudentsManager : MonoBehaviour
{
   
    private void Awake()
    {
        /*
         
        구현할 것
        1. 활성화된 과목 비활성화 된 과목 분리
        2. 스크립트 오브젝트에 부여
        3.  

         */
    }
}
