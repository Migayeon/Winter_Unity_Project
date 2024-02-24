using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static int dataIndex = 1; // 세이브 데이터 번호  
    public static int cost = 60; // 학원비
    public static string playerName = "default"; // 플레이어 이름
    public static string arcademyName = "default"; // 학원 이름
    public static int maxStudent = 300; // 최대 학생 명수
    public static int maxProfessor = 30; // 최대 교수 명수

    public static int nineSuccess = 0;
    public static int sevenSuccess = 0;
    public static int fiveSuccess = 0;

    // 교수 리스트
    public static List<ProfessorSystem.Professor> ProfessorList = new List<ProfessorSystem.Professor>();
    // 학생 리스트 
    public static List<StudentGroup[]> studentGroups = new List<StudentGroup[]>();
    // 졸업한 학생 리스트
    public static List<StudentGroup> graduatedGroups = new List<StudentGroup>();
    // 졸업한 학생 데이터
    public static List<string> graduateList = new List<string>();

    public static List<List<int>> UpgradeSkillIndex = new List<List<int>>();
    public static List<List<int>> UpgradeSkillValue = new List<List<int>>();
    /*
    public class Player
    {
        public  int dataIndex;
        public  string playerName;
        public  string arcademyName;
    }
    */

    //CreateProfessor.cs manage
    public static List<ProfessorSystem.Professor> RandomProfessorList = new List<ProfessorSystem.Professor>(3);
    public static bool PickedRandomProfessor = false;
    public static bool ProfessorPicked = false;
    public static ProfessorSystem.Professor PickedProfessorInfo;
    public static bool[] ProfessorPickedStatus = { true, true, true }; //default initialization
    public static int ProfessorCount()
    {
        return ProfessorList.Count;
    } 
    public static int GraduatedStudentTotalNum()
    {
        int total = 0;
        foreach (var group in graduatedGroups)
        {
            total += group.GetNumber();
        }
        return total;
    }
    public static int StudentGroupCount()
    {
        int sum = 0;
        foreach (var group in studentGroups)
        {
            sum += group[0].GetNumber();
            sum += group[1].GetNumber();
            sum += group[2].GetNumber();
        }
        return sum;
    }

    public static List<string> StudentStatRandomUpgrade(int amount, int upgradeMax)
    {
        int randomIndex;
        List<string> upgradedStudent = new List<string>();
        List<int> studentsIndex = new List<int>();
        for (int i = 0; i < studentGroups.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (studentGroups[i][j].GetNumber() > 0)
                {
                    studentsIndex.Add(3 * i + j);
                }
            }
        }

        if(studentsIndex.Count == 0)
        {
            return upgradedStudent;
        }

        for (int i = 0; i < upgradeMax; i++)
        {
            randomIndex = Random.Range(0, studentsIndex.Count);
            studentGroups[randomIndex/3][randomIndex%3].RandomStatUp(amount);
            upgradedStudent.Add(studentGroups[randomIndex / 3][randomIndex % 3].GetPeriod().ToString() + "/" + (randomIndex % 3 + 1).ToString());
            studentsIndex.RemoveAt(randomIndex);
        }

        return upgradedStudent;
    }
}
