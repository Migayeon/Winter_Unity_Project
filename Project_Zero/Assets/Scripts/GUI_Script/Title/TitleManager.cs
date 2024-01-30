using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
MainSystem/TitleManager.cs 

* 주요 함수 목록
1. void NewGameStart()
 - 변수 초기화
 - 프롤로그 재생 (있다면)
 - 튜토리얼 함수 연결

2. void ContinueGame()
 - playerprefs 에서 세이브 데이터 존재 여부 조사
 - 존재하지 않을 경우 경고 창 띄우기
 - 존재할 경우 SaveData scene으로 연결

3. void OpenOption()
 - 옵션 창 띄우기 (현재 데이터 초기화 버튼만 구현)

4. void ExitGame()
 - exitMessage 활성화
 - exitMessage에 있는 버튼에 함수 할당
 - yesButton을 누를 경우 게임 종료
 - noButton을 누를 경우 exitMessage 비활성화
 */

public class TitleManager : MonoBehaviour
{
    // UI
    // 스크립트에서 따로 할당하지 않음 유니티에서 직접 할당
    public Button newGame, continueGame, gameOption, exitGame;
    private Button yesButton, noButton, xButton;
    public GameObject exitMessage;
    public GameObject warningMessage;
    [SerializeField]
    private Button achievementButton;

    public void NewGameStart() // 새로운 게임 시작
    {
        SavedataSceneManager.backPath = "Title";
        SavedataSceneManager.workType = "New";
        SceneManager.LoadScene("Savedata");
    }

    public void ContinueGame() // 게임 불러오기
    {
        if (!PlayerPrefs.HasKey("saveFile")) // 세이브 데이터가 존재하지 않을 경우
        {
            warningMessage.SetActive(true);
            xButton = warningMessage.transform.GetChild(1).GetComponent<Button>();
            xButton.onClick.AddListener(CloseWarning);
        }
        else
        {
            SavedataSceneManager.backPath = "Title";
            SavedataSceneManager.workType = "Load";
            SceneManager.LoadScene("Savedata");
        } 
    }

    public void CloseWarning() 
    {
        xButton.onClick.RemoveAllListeners();
        warningMessage.SetActive(false);
    }

    public void OpenOption()
    {
        SettingManager.backPath = "Title";
        SceneManager.LoadScene("Setting");
    }

    public void ExitGame() // 게임 종료 창 띄우기
    {
        exitMessage.SetActive(true);
        yesButton = exitMessage.transform.GetChild(1).GetComponent<Button>(); // 버튼 할당
        noButton = exitMessage.transform.GetChild(2).GetComponent<Button>();
        yesButton.onClick.AddListener(YesExit); // 버튼에 함수 할당
        noButton.onClick.AddListener(NoExit);
    }

    public void YesExit() // 게임 종료
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void NoExit() // 종료 취소
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        exitMessage.SetActive(false);
    }

    private void turnToAchievementAScene()
    {
        SceneManager.LoadScene("Achievement");
    }

    private void Awake()
    {
        // 버튼 별로 함수 할당
        newGame.onClick.AddListener(NewGameStart);
        continueGame.onClick.AddListener(ContinueGame);
        gameOption.onClick.AddListener(OpenOption);
        exitGame.onClick.AddListener(ExitGame);
        //achievementButton.onClick.AddListener(turnToAchievementAScene);

        // UI 기본 설정
        exitMessage.SetActive(false);
        warningMessage.SetActive(false);
    }
}
