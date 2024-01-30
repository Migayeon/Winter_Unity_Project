using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // 버튼 오브젝트에 스크립트 적용 후 바꿀 씬 이름 + 노래 이름 넣으면 전환됨
    [SerializeField] public string targetScene = "";
    [SerializeField] public string targetSong = "";
    public void ChangeScene()
    {
        BGMManager.SetBGM(targetSong);
        SceneManager.LoadScene(targetScene);
    }
}
