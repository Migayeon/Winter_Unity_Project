using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static int dataIndex = 1; // 세이브 데이터 번호  
    public static int cost = 60; // 학원비
    public static string playerName = "default"; // 플레이어 이름
    public static string arcademyName = "default"; // 학원 이름
    public static int maxStudent = 300; // 최대 학생 명수
    public static int maxProfessor = 30; // 최대 교수 명수

    public static int nineSuccess;
    public static int sevenSuccess;
    public static int fiveSuccess;


    // 교수 리스트
    public static List<ProfessorSystem.Professor> ProfessorList = new List<ProfessorSystem.Professor>();
    // 학생 리스트 
    public static List<StudentGroup[]> studentGroups = new List<StudentGroup[]>();

    /*
    public class Player
    {
        public  int dataIndex;
        public  string playerName;
        public  string arcademyName;
    }
    */

    public static List<ProfessorSystem.Professor> RandomProfessorList = new List<ProfessorSystem.Professor>(3);
    public static int ProfessorCount()
    {
        return ProfessorList.Count;
    } 

    public static int StudentGroupCount()
    {
        int sum = 0;
        foreach (var group in studentGroups)
        {
            sum += group[0].GetNumber() * 3;
        }
        return sum;
    }

    public static void LoadStudentData(string[] data, int lim)
    {
        studentGroups.Clear();
        for(int i = 0; i < lim/3; i++)
        {
            StudentGroup[] students = new StudentGroup[3];
            for (int j =0; j < 3;j++)
            {
                students[j] = new StudentGroup(data[3 * i + j]);
            }
            studentGroups.Add(students);
        }
    }

    public static void LoadProfessorData(string[] data, int lim)
    {
        ProfessorList.Clear();
        for(int i = 0;i < lim; i++)
        {
            ProfessorList.Add(new ProfessorSystem.Professor(data[i]));
        }
    }
}
