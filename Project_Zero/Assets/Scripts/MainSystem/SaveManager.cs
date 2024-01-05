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
}
public class ProfessorData
{
    private string[] professors;
    private int professorNum;

    public ProfessorSystem.Professor GetProfessor(int i) 
    {
        /*
         
            코드 구현 (파싱)

         */
        return null;
    }
}

public class SaveManager : MonoBehaviour
{
    private static ProfessorSystem.Professor professor1;
    public static PlayerData PlayerDataLoad(int i)
    {
        var loadedJson = Resources.Load<TextAsset>($"Data\\player{i}");
        PlayerData saveData = JsonUtility.FromJson<PlayerData>(loadedJson.ToString());
        return saveData;
    }

    public static void PlayerDataSave(int i)
    {
        PlayerData newSave = new PlayerData();
        newSave.turn = TurnManager.turn;
        newSave.ar = GoodsManager.goodsAr;
        newSave.stone = GoodsManager.goodsStone;
        newSave.fame = GoodsManager.goodsFame;
        newSave.studentsNum = GoodsManager.goodsStudent;
        /*
         
        이름 / 학원 저장 스크립트 추가해야 함.
         
         */
        newSave.myName = PlayerInfo.playerName;
        newSave.arcademyName = PlayerInfo.arcademyName;
        string json = JsonUtility.ToJson(newSave, true);
        File.WriteAllText($"Assets/Resources/Data/player{i}.json",json);
        Debug.Log("Success");
    }

    private static string ProfessorSave(ProfessorSystem.Professor professor)
    {
        string information = 
            professor.ProfessorGetID().ToString() + '/' +
            professor.ProfessorGetName() + '/' +
            professor.ProfessorGetTenure().ToString() + '/' +
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

