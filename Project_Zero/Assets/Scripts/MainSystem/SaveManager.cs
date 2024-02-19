using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerData(int _t, int _a, int _s, int _f) 
    {
        turn = _t;
        ar = _a;
        stone = _s;
        fame = _f;
    }
    public PlayerData() { }

    public int turn, ar, stone, fame; //, debt; 빚 추가해야 함.
    public string myName, arcademyName;
    public int cost, maxStudent, maxProfessor;
    public int nineSuccess, sevenSuccess, fiveSuccess;


    /*
     
    재화 시스템에 따라 더 추가하기. (무엇을 저장해야 하는가?) 

    */
    public string[] DataPreview()
    {
        return new string[5]{myName,arcademyName,turn.ToString(),ar.ToString(),fame.ToString()};
    }

}

public struct InGameData
{
    public List<string> professor; // 클래스 정보 받아서 하나의 스트링으로
    public List<string> students;
    public string subject;
}


public class SaveManager : MonoBehaviour
{
    private static ProfessorSystem.Professor professor1;
    private static string path = Application.persistentDataPath + '/';

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
        PlayerInfo.cost = playerData.cost;
        PlayerInfo.maxStudent = playerData.maxStudent;
        PlayerInfo.maxProfessor = playerData.maxProfessor;

        PlayerInfo.nineSuccess = playerData.nineSuccess;
        PlayerInfo.sevenSuccess = playerData.sevenSuccess;
        PlayerInfo.fiveSuccess = playerData.fiveSuccess;
    }

    public static void PlayerDataSave(int i)
    {
        PlayerData newSave = new PlayerData();

        newSave.turn = TurnManager.turn;
        newSave.ar = GoodsManager.goodsAr;
        newSave.stone = GoodsManager.goodsStone;
        newSave.fame = GoodsManager.goodsConstFame;

        newSave.myName = PlayerInfo.playerName;
        newSave.arcademyName = PlayerInfo.arcademyName;
        newSave.cost = PlayerInfo.cost;
        newSave.maxStudent = PlayerInfo.maxStudent;
        newSave.maxProfessor = PlayerInfo.maxProfessor;

        newSave.nineSuccess = PlayerInfo.nineSuccess;
        newSave.sevenSuccess = PlayerInfo.sevenSuccess;
        newSave.fiveSuccess = PlayerInfo.fiveSuccess;

        string json = JsonUtility.ToJson(newSave, true);
        File.WriteAllText(path + "player" + i.ToString(), json);
        Debug.Log("Success");
        return;
    }

    public static void InGameDataSave(int i)
    {
        InGameData inGameData = new InGameData();
        inGameData.professor = new List<string>();
        inGameData.students = new List<string>();
        inGameData.subject = SubjectTree.save();

        ProfessorSave(inGameData);
        StudentSave(inGameData);

        string json = JsonUtility.ToJson(inGameData, true);
        File.WriteAllText(path + "inGameData" + i.ToString(), json);
    }

    public static void InGameDataLoad(int i)
    {
        string json = File.ReadAllText(path + "inGameData" + i.ToString());
        InGameData inGameData = JsonUtility.FromJson<InGameData>(json);
        PlayerInfo.ProfessorList = new List<ProfessorSystem.Professor>();
        PlayerInfo.studentGroups = new List<StudentGroup[]>();

        for (int j = 0; j < inGameData.professor.Count; j++)
        {
            ProfessorSystem.Professor newProfessor
                = JsonUtility.FromJson<ProfessorSystem.Professor>(inGameData.professor[j]);
            PlayerInfo.ProfessorList.Add(newProfessor);
        }

        for (int j = 0; j < inGameData.students.Count; j++)
        {
            StudentGroup[] newStudentGroup = new StudentGroup[3];
            string[] studentsJson = inGameData.students[j].Split("/");
            for (int k = 0; k < 3; k++)
            {
                newStudentGroup[k] = JsonUtility.FromJson<StudentGroup>(studentsJson[k]);
            }
            PlayerInfo.studentGroups.Add(newStudentGroup);
        }
        SubjectTree.initSubjectsAndInfo();
        SubjectTree.initSubjectStates(new List<int>());
        SubjectTree.callOnlyOneTimeWhenGameStart();
        SubjectTree.load(inGameData.subject);
    }

    public static void StudentSave(InGameData data)
    {
        for (int i = 0; i<PlayerInfo.studentGroups.Count; i++)
        {
            string dataJson = "";
            for (int j = 0; j < 3; j++)
            {
                dataJson += JsonUtility.ToJson(PlayerInfo.studentGroups[i][j]);
                Debug.Log(dataJson);
                if (j < 2)
                {
                    dataJson += "/";
                }
            }
            data.students.Add(dataJson);
        }
    }

    private static void ProfessorSave(InGameData data)
    {
        for (int i = 0;i<PlayerInfo.ProfessorList.Count;i++)
        {
            string dataJson = JsonUtility.ToJson(PlayerInfo.ProfessorList[i]);
            Debug.Log(dataJson);
            data.professor.Add(dataJson);
        }
    }

    public static void AchievementLocalStatSave(int i)
    {
        string json = AchievementManager.SaveLocalStat();
        File.WriteAllText(path + "achievementStat" + i.ToString(), json);
    }

    public static void AchievementLocalStatLoad(int i)
    {
        string jsonData = File.ReadAllText(path + "achievementStat" + i.ToString());
        AchievementManager.LoadLocalStat(jsonData);
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
        InGameDataSave(i);
        AchievementLocalStatSave(i);
    }

    public static void LoadProcess()
    {
        int i = PlayerInfo.dataIndex;
        PlayerDataLoad(i);
        InGameDataLoad(i);
        AchievementLocalStatLoad(i);
        TestCheckManager.InitTestInfo();
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

