using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavedataSceneManager : MonoBehaviour
{
    public static string backPath = "Title";
    public static string workType = "Save";

    public Button backButton;
 
    public GameObject[] save = new GameObject[3];
    PlayerData playerData;

    public void NewGame(int i)
    {
        PlayerInfo.dataIndex = i;
        TurnManager.turn = 1;
        GoodsManager.goodsAr = 20000;
        GoodsManager.goodsStone = 0;
        GoodsManager.goodsFame = 0;
        GoodsManager.goodsStudent = 0;

        /*
         
        초기화할 내용 추가. 

         */

        SceneManager.LoadScene("StartSetting");
    }

    public void LoadGame()
    {

    }

    public void ErrorMessage()
    {

    }

    public void BackTracking()
    {
        SceneManager.LoadScene(backPath);
    }

    private void Awake()
    {
        backButton.onClick.AddListener(BackTracking);
        if(workType == "Save")
        {
            save[0].GetComponent<Button>().onClick.AddListener(delegate { NewGame(1); });
            save[1].GetComponent<Button>().onClick.AddListener(delegate { NewGame(2); });
            save[2].GetComponent<Button>().onClick.AddListener(delegate { NewGame(3); });
        }
        else
        {
            save[0].GetComponent<Button>().onClick.AddListener(delegate { NewGame(1); });
        }

        for (int i = 1; i <= 3;i++)
        {
            GameObject tmp = save[i - 1];
            if (PlayerPrefs.HasKey($"save{i}"))
            {
                playerData = SaveManager.PlayerDataLoad(i);
                tmp.transform.GetChild(0).gameObject.SetActive(false);
                tmp.transform.GetChild(1).GetComponent<Text>().text
                    = $"{playerData.myName}의 {playerData.arcademyName} 아카데미\r\n" +
                    $"Turn : {playerData.turn}\r\n아르 : {playerData.ar}\r\n" +
                    $"명성 : {playerData.fame}";
                tmp.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                tmp.transform.GetChild(0).gameObject.SetActive(true);
                tmp.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

}
