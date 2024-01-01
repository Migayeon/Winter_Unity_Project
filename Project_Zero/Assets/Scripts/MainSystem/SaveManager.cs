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

    public int number;
    public int turn;
    public string name;

    /*
     
    재화 시스템에 따라 더 추가하기. (무엇을 저장해야 하는가?) 

    */
}

public class SaveManager : MonoBehaviour
{
    void TryLoad()
    {

    }
}
