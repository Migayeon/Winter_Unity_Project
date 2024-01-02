using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SaveData() 
    {
        
    }

    public int turn;
    public string name;
    public int ar;
    public int debt;
    public string[] tc;

    /*
     
    재화 시스템에 따라 더 추가하기. (무엇을 저장해야 하는가?) 

    */
}

public class testClass
{
    public string name;
    public int id;
}

public class SaveManager : MonoBehaviour
{
    public static SaveData LoadData(int i)
    {
        if (PlayerPrefs.HasKey("save"+i.ToString()))
        {
            return null;
        }
        var loadedJson = Resources.Load<TextAsset>($"Data\\save{i}");
        SaveData saveData = JsonUtility.FromJson<SaveData>(loadedJson.ToString());
        return saveData;
    }

    private void Awake()
    {
        SaveData save = LoadData(1);
        Debug.Log(save.name);
        Debug.Log(save.turn);
        Debug.Log(save.tc[0]);
        Debug.Log(save.tc[1]);
    }
}

