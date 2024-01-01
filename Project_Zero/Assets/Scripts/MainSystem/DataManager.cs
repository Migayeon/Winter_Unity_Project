using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MainSystem/GameManager.cs 

function
1. void NewGameStart()
 - Initialize variables
 - 

2. bool ContinueGame()
 - Bring data from playerprefs
 - Assign data to variables
 - Call update function on each class

3. void SaveGame()
 - Save variables in playerprefs
 - 한글
*/

public class DataManager : MonoBehaviour
{
    public void NewGameStart()
    {
        TurnManager.turn = 1;
    }

    public bool ContinueGame()
    {
        if (!PlayerPrefs.HasKey("saveFile")) // there is no save data
        {
            return false;  
        }
        TurnManager.turn = PlayerPrefs.GetInt("turn",-1); // 

        return true;
    }
}
