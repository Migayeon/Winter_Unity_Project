using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESC_Manager : MonoBehaviour
{
    [HideInInspector]
    public bool isPause = false;
    [SerializeField]
    private GameObject escMessage;
    [SerializeField]
    private GameObject loading;
    [SerializeField]
    private GameObject toTitleMessage;
    private void Awake()
    {
        escMessage.SetActive(false);
        escMessage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Save);
        escMessage.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Setting);
        escMessage.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(TitleCheckMessage);
        escMessage.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(makeGamePlaying);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            if (isPause)
                makeGamePause();
            else
                makeGamePlaying();
        }
    }
    private void makeGamePause()
    {
        escMessage.SetActive(true);
        loading.SetActive(false);
        toTitleMessage.SetActive(false);
    }

    private void makeGamePlaying()
    {
        escMessage.SetActive(false);
    }

    public void Save()
    {
        loading.SetActive(true);
        SaveManager.SaveProcess();
        loading.SetActive(false);
    }
    public void Setting()
    {
        SettingManager.backPath = "Main";
        SceneManager.LoadScene("Setting");
    }
    public void TitleCheckMessage()
    {
        toTitleMessage.SetActive(true);
        toTitleMessage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(
            delegate
            {
                SceneManager.LoadScene("Title");
            }
        );
        toTitleMessage.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(NoTitleGo);
    }
    public void NoTitleGo()
    {
        toTitleMessage.SetActive(false);
    }
    public void CloseToTitleMessage()
    {
        toTitleMessage.SetActive(false);
    }
}
