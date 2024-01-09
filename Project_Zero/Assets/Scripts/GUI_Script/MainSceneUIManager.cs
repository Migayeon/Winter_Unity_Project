using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class jsonCheck
{
    public int check;
}

public class MainSceneUIManager : MonoBehaviour
{
    static int index = 0;
    bool isOpen = false;
    float time;
    bool checkSave = false;
    int check;

    public GameObject[] tabCanvas = new GameObject[4];
    string[] tabNameArr = new string[4] { "학원", "교수", "학생", "자산 관리" };

    public Button nextButton;
    public Button prevButton;
    public Button selectOpenButton;
    public GameObject tabList;
    public Text tabName;

    public GameObject escMessage;
    public GameObject loading;
    public GameObject toTitleMessage;

    public Button[] sceneButton = new Button[9];
    string[] sceneName = new string[9] // 연결된 scene 추가
    { "AcademyManagement", 
        "Curriculum",
        "StudentCost",
        "ManageProfessor",
        "GetProfessor",
        "Curriculum2",
        "Test_Section",
        "Stock",
        "manaStoneGamble"
    };



    public void NextTab()
    {
        tabCanvas[index].SetActive(false);
        if(index >= 3)
            index = 0;
        else
            index++;
        tabCanvas[index].SetActive(true);
        if (!isOpen)
        {
            tabName.text = tabNameArr[index];
        }
        SelectTabInitialize();
    }

    public void PrevTab()
    {
        tabCanvas[index].SetActive(false);
        if (index <= 0)
            index = 3;
        else
            index--;
        tabCanvas[index].SetActive(true);
        if (!isOpen)
        {
            tabName.text = tabNameArr[index];
        }
        SelectTabInitialize();
    }

    public void MoveTab(int i)
    {
        index = i;
        for (int j = 0; j < 4; j++)
        {
            tabCanvas[j].SetActive(false);
        }
        tabCanvas[i].SetActive(true);
        tabName.text = tabNameArr[index];
        SelectTabInitialize();
        SelectTab();
    }

    public void SelectTab()
    {
        if (isOpen)
        {
            tabName.text = tabNameArr[index];
            selectOpenButton.transform.rotation = Quaternion.Euler(0, 0, 0);
            tabList.SetActive(false);
            isOpen = false;
        }
        else
        {
            tabName.text = "";
            selectOpenButton.transform.rotation = Quaternion.Euler(0, 0, -90);
            SelectTabInitialize();
            tabList.SetActive(true);
            isOpen = true;
        }
    }

    public void SelectTabInitialize()
    {
        foreach (Transform tab in tabList.GetComponentInChildren<Transform>())
        {
            tab.GetComponent<Image>().color = Color.white;
        }
        tabList.transform.GetChild(index).GetComponent<Image>().color = Color.gray;
    }

    public void MoveScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void MoveToCurriculum(int mod)
    {
        curriculumModManager.loadCurriculumSceneWithMod(mod);
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
        MoveScene("Setting");
    }

    public void TitleCheckMessage()
    {
        toTitleMessage.SetActive(true);
        toTitleMessage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { MoveScene("Title"); });
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

    public void CloseMessage()
    {
        escMessage.SetActive(false);
    }

    private void Awake()
    {
        isOpen = true;
        escMessage.SetActive(false);
        MoveTab(index);
        prevButton.onClick.AddListener(PrevTab);
        nextButton.onClick.AddListener(NextTab);
        selectOpenButton.onClick.AddListener(SelectTab);
        for (int i = 0; i < 4; i++)
        {
            int j = i;
            tabList.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { MoveTab(j); });
        }
        for (int i = 0;i<9;i++)
        {
            if(i==1)
            {
                sceneButton[i].onClick.AddListener(delegate { MoveToCurriculum(1); });
            }
            if(i == 5)
            {
                sceneButton[i].onClick.AddListener(delegate { MoveToCurriculum(2); });
            }
            else
            {
                int j = i;
                sceneButton[i].onClick.AddListener(delegate { MoveScene(sceneName[j]); });
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escMessage.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            escMessage.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            escMessage.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
            escMessage.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
            Debug.Log("hi");
            escMessage.SetActive(true);
            loading.SetActive(false);
            toTitleMessage.SetActive(false);
            escMessage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Save);
            escMessage.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Setting);
            escMessage.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(TitleCheckMessage);
            escMessage.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(CloseMessage);

        }
    }
}
