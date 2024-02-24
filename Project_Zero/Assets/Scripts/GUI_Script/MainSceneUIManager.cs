using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneUIManager : MonoBehaviour
{
    static int index = 0;
    private bool isOpen = false;

    public GameObject[] tabCanvas = new GameObject[4];
    string[] tabNameArr = new string[4] { "학원", "교수", "학생", "자산 관리" };

    public Button nextButton;
    public Button prevButton;
    public Button selectOpenButton;
    public GameObject tabList;
    public Text tabName;
    public Text turn;
    public Text fame;
    [SerializeField]
    private Button assistantBtn;
    private const float interval = 3.0f;
    private float clickedTime = -1.0f;
    private int clickCnt = 0;

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

    private void Awake()
    {
        turn.text = TurnManager.turn.ToString();
        GoodsManager.CalculateEndedFame();
        fame.text = GoodsManager.goodsCalculatedEndedFame.ToString();
        isOpen = true;
        MoveTab(index);
        prevButton.onClick.AddListener(PrevTab);
        nextButton.onClick.AddListener(NextTab);
        selectOpenButton.onClick.AddListener(SelectTab);
        assistantBtn.onClick.RemoveAllListeners();
        assistantBtn.onClick.AddListener(ClickAssistant);
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

    private void ClickAssistant()
    {
        if (Time.time - clickedTime < interval)
        {
            if (++clickCnt == 10)
                AchievementManager.Achieve(3);
        }
        else
        {
            clickCnt = 1;
        }
        clickedTime = Time.time;
    }
}