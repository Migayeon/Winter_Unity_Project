using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerData(int _t, int _a, int _s, int _sN, int _f) 
    {
        turn = _t;
        ar = _a;
        stone = _s;
        studentsNum = _sN;
        fame = _f;
    }
    public PlayerData() { }
    public int turn, ar, stone, studentsNum, fame; //, debt; 빚 추가해야 함.
    public string myName, arcademyName;

    //public string[] professor; // 클래스 정보 받아서 하나의 스트링으로
    //public string[] students;
    /*
     
    재화 시스템에 따라 더 추가하기. (무엇을 저장해야 하는가?) 

    */
    public string[] DataPreview()
    {
        return new string[5]{myName,arcademyName,turn.ToString(),ar.ToString(),fame.ToString()};
    }
}
public class ProfessorData
{
    private string[] professorData;
    private int professorNum;

    public ProfessorData() 
    {
        int i = 0;
        professorNum = PlayerInfo.ProfessorCount();
        professorData = new string[professorNum];
        foreach (ProfessorSystem.Professor professor in PlayerInfo.ProfessorList) 
        {
            professorData[i] = professor.ProfessorDataToString();
            i++;
        }
    }
}
public class StudentData
{
    private int groupNum;
    private string[] studentData;

    public StudentData() 
    {
        int i = 0;
        groupNum = PlayerInfo.studentGroups.Count*3;
        studentData = new string[groupNum];
        foreach (StudentGroup[] students in PlayerInfo.studentGroups)
        {
            foreach(StudentGroup student in students)
            {
                studentData[i] = student.StudentDataToString();
                i++;
            }
        }
    }

    public int GetGroupNum()
    {
        return groupNum;
    }

    public string[] GetStudentData()
    {
        return studentData;
    }
}

public class SaveManager : MonoBehaviour
{
    private static ProfessorSystem.Professor professor1;
    private static string path = Application.dataPath + '/';

    public static string[] PlayerDataPreview(int i)
    {
        string jsonData = File.ReadAllText(path + "player" + i.ToString());
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        return playerData.DataPreview();
    }
    public static void PlayerDataLoad(int i)
    {
        string jsonData = File.ReadAllText(path + "player" + i.ToString());
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        TurnManager.turn = playerData.turn;
        GoodsManager.goodsAr = playerData.ar;
        GoodsManager.goodsStone = playerData.stone;
        GoodsManager.goodsConstFame = playerData.fame;
        PlayerInfo.playerName = playerData.myName;
        PlayerInfo.arcademyName = playerData.arcademyName;
    }

    public static void PlayerDataSave(int i)
    {
        PlayerData newSave = new PlayerData();
        newSave.turn = TurnManager.turn;
        newSave.ar = GoodsManager.goodsAr;
        newSave.stone = GoodsManager.goodsStone;
        newSave.fame = GoodsManager.goodsConstFame;
        //newSave.studentsNum = GoodsManager.goodsStudent;
        /*
         
        이름 / 학원 저장 스크립트 추가해야 함.
         
         */
        newSave.myName = PlayerInfo.playerName;
        newSave.arcademyName = PlayerInfo.arcademyName;

        string json = JsonUtility.ToJson(newSave, true);
        File.WriteAllText(path+"player"+i.ToString(),json);
        Debug.Log("Success");
        return;
    }

    public static void StudentSave(int i)
    {
        StudentData studentData = new StudentData();
        string json = JsonUtility.ToJson(studentData, true);
        File.WriteAllText(path + "student" + i.ToString(), json);
        Debug.Log("Success");
    }

    public static void StudentLoad(int i)
    {
        string jsonData = File.ReadAllText(path + "player" + i.ToString());
        StudentData studentData = JsonUtility.FromJson<StudentData>(jsonData);
        PlayerInfo.LoadStudentData(studentData.GetStudentData(),studentData.GetGroupNum());
    }

    private static string ProfessorSave(ProfessorSystem.Professor professor)
    {
        string information = 
            professor.ProfessorGetID().ToString() + '/' +
            professor.ProfessorGetName() + '/' +
            professor.ProfessorGetTenureInTurns().ToString() + '/' +
            professor.ProfessorGetType().ToString() + '/';
        List<int> stat = professor.ProfessorGetStats();
        foreach (int i in stat)
            information += i.ToString() + '/';
        information += professor.ProfessorGetSalary() + '/';
        information += professor.ProfessorGetAwayStatus() ? "1" : "0";

        return information;
    }

    public static void SaveProcess()
    {
        int i = PlayerInfo.dataIndex;

        if (!PlayerPrefs.HasKey("saveFile"))
        {
            PlayerPrefs.SetInt("saveFile", 1);
        }
        if (!PlayerPrefs.HasKey("save" + i.ToString())) 
        {
            PlayerPrefs.SetInt("save" + i.ToString(), 1);
        }
        PlayerPrefs.Save();

        PlayerDataSave(i);
        //ProfessorSave(i);
        //StudentSave(i);

    }

    public static void LoadProcess()
    {
        int i = PlayerInfo.dataIndex;
        PlayerDataLoad(i);
    }

    private void Awake()
    {
        /*
        List<int> stat = new List<int>{1, 2, 3, 4, 5, 6 };
        professor1 = new ProfessorSystem.Professor(1, "장형수", 10, 1, stat);
        */


        /*
        SaveData save = LoadData(1);
        Debug.Log(save.name);
        Debug.Log(save.turn);
        Debug.Log(save.tc[0]);
        Debug.Log(save.tc[1]);
        */
        //SaveProcess(1);
        //SaveData saved = LoadProcess(1);
        //Debug.Log(saved.stat[0]);
    }
}

