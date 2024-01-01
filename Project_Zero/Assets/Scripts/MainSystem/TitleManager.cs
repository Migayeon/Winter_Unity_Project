using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
MainSystem/TitleManager.cs 

* 함수 목록
1. void NewGameStart()
 - 변수 초기화
 - 프롤로그 재생 (있다면)
 - 튜토리얼 함수 연결

2. bool ContinueGame()
 - playerprefs 에서 세이브 데이터 존재 여부 조사
 - 존재 하지 않을 경우 return false
 - 존재 할 경우 변수들 업데이트
 - UI/오브젝트 같은 시각적인 변화 업데이트

 */

public class TitleManager : MonoBehaviour
{
    // UI
    // 스크립트에서 따로 할당하지 않음 유니티에서 직접 할당
    public Button newGame, continueGame, gameOption, exitGame;
    public GameObject exitMessage;

    static string type = "common";
    StreamReader reader = new StreamReader($"Asset\\Resources\\Csv\\{type}.csv");

    public void NewGameStart()
    {
        TurnManager.turn = 1;
    }

    public void ContinueGame()
    {
        if (!PlayerPrefs.HasKey("saveFile")) // there is no save data
        {
            //return false;  
        }
        TurnManager.turn = PlayerPrefs.GetInt("turn",-1); // 

        //return true;
    }

    public void OpenOption()
    {

    }

    public void ExitGame()
    {
        exitMessage.SetActive(true);

    }

    private void Awake()
    {
        // 버튼 별로 함수 할당
        newGame.onClick.AddListener(NewGameStart);
        continueGame.onClick.AddListener(ContinueGame);
        gameOption.onClick.AddListener(OpenOption);
        exitGame.onClick.AddListener(ExitGame);

        //UI 기본 설정
        exitMessage.SetActive(false);   
    }
}
