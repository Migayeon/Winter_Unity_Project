using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ManageCurriculumMod;
using static ProfessorSystem;

public class ManageCurriculumMod : MonoBehaviour
{
    [SerializeField]
    private Transform panel;
    [SerializeField]
    private Transform professorInfoUI;
    [SerializeField]
    private Transform OpenSubjectButton;
    [SerializeField]
    private GameObject subjectGameObject;
    [SerializeField]
    private Transform subjectGetter;
    [SerializeField]
    private GameObject professorSelectButtonPrefab;
    [SerializeField]
    private Transform professorDetailUI;
    [SerializeField]
    private Button goBackButton;
    [SerializeField]
    private ESC_Manager EscManager;
    [SerializeField]
    private Sprite[] professorIllust;
    [SerializeField]
    private Transform subjectInfoUI;

    private Transform professorContents;
    private Transform professorNameUI;
    private List<SelectButton> selectButtons = new List<SelectButton>();

    private Transform detailNameTransform;
    private Transform detailInfoTransform;
    private Transform assignButtonTransform;
    private Transform freeButtonTransform;
    private Transform professorImageTransform;
    private Transform exitDetailButtonTransform;

    private int selectedSubjectId = -1;
    private bool isProfessorDetailOpen = false;
    private bool selectFixed = false;

    private void Awake()
    {
        professorContents = professorInfoUI.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        professorNameUI = professorInfoUI.GetChild(1);
        detailNameTransform = professorDetailUI.GetChild(1).GetChild(0);
        detailInfoTransform = professorDetailUI.GetChild(1).GetChild(1);
        assignButtonTransform = professorDetailUI.GetChild(1).GetChild(2);
        freeButtonTransform = professorDetailUI.GetChild(1).GetChild(3);
        professorImageTransform = professorDetailUI.GetChild(1).GetChild(4);
        exitDetailButtonTransform = professorDetailUI.GetChild(2);

        foreach (Transform subject in subjectGameObject.GetComponentInChildren<Transform>())
            setColor(subject);

        foreach (Button subject in subjectGameObject.transform.GetComponentsInChildren<Button>())
        {
            subject.onClick.RemoveAllListeners();
            subject.onClick.AddListener(
                delegate {
                    SubjectClick(Convert.ToInt32(subject.name));
                }
            );
        }
        goBackButton.onClick.AddListener(
            delegate
            {
                goBack();
            }
        );
    }

    private void Start()
    {
        panel.gameObject.SetActive(false);
        subjectInfoUI.gameObject.SetActive(false);
        professorInfoUI.gameObject.SetActive(false);
        professorDetailUI.gameObject.SetActive(false);
        OpenSubjectButton.gameObject.SetActive(false);
        EscManager.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (EscManager.isPause) return;
        if (isProfessorDetailOpen) return;
        if (!selectFixed)
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray2 = new Ray2D(mp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
            if (hit.collider != null)
            {
                Transform nowTransform = hit.collider.transform;
                int nowTransformId = int.Parse(nowTransform.name);
                setSubjectInfo(nowTransformId);
                setUI(nowTransformId);
            }
            else
            {
                panel.gameObject.SetActive(false);
                subjectInfoUI.gameObject.SetActive(false);
                professorInfoUI.gameObject.SetActive(false);
                OpenSubjectButton.gameObject.SetActive(false);
            }
        }
    }

    public void setColor(Transform subject)
    {
        if (SubjectTree.subjectState[int.Parse(subject.name)] == SubjectTree.State.Open)
        {
            subject.GetComponent<Image>().color = Color.white;
        }
        else if (SubjectTree.subjectState[int.Parse(subject.name)] == SubjectTree.State.ReadyToOpen)
        {
            subject.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
        }
        else
        {
            subject.GetComponent<Image>().color = new Color(0.6f, 0.1f, 0.1f);
        }
    }

    public void SubjectClick(int clickedId)
    {
        if (EscManager.isPause) return;
        if (isProfessorDetailOpen) return;
        if (clickedId == selectedSubjectId)
        {
            selectFixed = false;
            selectedSubjectId = -1;
            foreach (Transform subject in subjectGameObject.GetComponentInChildren<Transform>())
                setColor(subject);
        }
        else
        {
            if (selectedSubjectId != -1)
                foreach (Transform subject in subjectGameObject.GetComponentInChildren<Transform>())
                    setColor(subject);
            selectedSubjectId = clickedId;
            selectFixed = true;
            setSubjectInfo(clickedId);
            setUI(clickedId);
            subjectGameObject.transform.GetChild(selectedSubjectId).GetComponent<Image>().color = Color.green;
        }
    }

    public void OpenSubjectButtonClicked()
    {
        if (isProfessorDetailOpen) return;
        if (selectedSubjectId != -1)
        {
            int cost = SubjectTree.subjectsInfo.costByTier[SubjectTree.subjects[selectedSubjectId].tier];
            GoodsManager.goodsAr -= cost;
            SubjectTree.openSubject(selectedSubjectId);
            setUI(selectedSubjectId);
            foreach (Transform subject in subjectGameObject.GetComponentInChildren<Transform>())
                if (int.Parse(subject.name) != selectedSubjectId)
                    setColor(subject);
            if (SubjectTree.subjects[selectedSubjectId].subjectGroupId == 1)
                AchievementManager.Achieve(6);
        }
    }

    public void setUI(int nowTransformId)
    {
        if (SubjectTree.subjectState[nowTransformId] == SubjectTree.State.Open)
        {
            initProfessorScroll(nowTransformId);
            panel.gameObject.SetActive(true);
            subjectInfoUI.gameObject.SetActive(true);
            professorInfoUI.gameObject.SetActive(true);
            OpenSubjectButton.gameObject.SetActive(false);
        }
        else if (SubjectTree.subjectState[nowTransformId] == SubjectTree.State.ReadyToOpen)
        {
            panel.gameObject.SetActive(true);
            subjectInfoUI.gameObject.SetActive(true);
            professorInfoUI.gameObject.SetActive(false);
            int cost = SubjectTree.subjectsInfo.costByTier[SubjectTree.subjects[nowTransformId].tier];
            if (GoodsManager.goodsAr >= cost)
            {
                OpenSubjectButton.GetComponent<Button>().interactable = true;
                OpenSubjectButton.GetChild(0).GetComponent<TMP_Text>().text = "구매하기\n( " + cost.ToString() + "Ar )";
                OpenSubjectButton.GetComponent<Image>().color = Color.white;
            }
            else
            {
                OpenSubjectButton.GetComponent<Button>().interactable = false;
                OpenSubjectButton.GetChild(0).GetComponent<TMP_Text>().text = "구매불가\n( " + cost.ToString() + "Ar )";
                OpenSubjectButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            }
            OpenSubjectButton.gameObject.SetActive(true);
        }
        else
        {
            panel.gameObject.SetActive(true);
            subjectInfoUI.gameObject.SetActive(true);
            professorInfoUI.gameObject.SetActive(false);
            OpenSubjectButton.gameObject.SetActive(false);
        }
    }

    public void clearProfessorContents()
    {
        selectButtons.Clear();
        foreach (Transform child in professorContents.GetComponentInChildren<Transform>())
            Destroy(child.gameObject);
    }

    public SelectButton addProfessorSelectButton(Professor professor, int subjectId, int index)
    {
        GameObject newSelectButton = Instantiate(professorSelectButtonPrefab, professorContents);
        Transform buttonTransform = newSelectButton.transform.GetChild(0);
        Transform professorName = buttonTransform.GetChild(0);
        Transform professorBusy = buttonTransform.GetChild(1);
        Transform professorTypeDisplay = buttonTransform.GetChild(2);
        professorName.GetComponent<TMP_Text>().text = professor.ProfessorGetName();
        Button button = buttonTransform.GetComponent<Button>();
        professorTypeDisplay.GetComponent<Image>().sprite = professorIllust[professor.ProfessorGetType()];
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(
            delegate
            {
                openDetail(professor, subjectId, index);
            }
        );
        updateSelectButtonAndColor(professorBusy, buttonTransform, professor, subjectId);
        return new SelectButton(professor, newSelectButton);
    }

    public void updateSelectButtonAndColor(Transform professorBusy, Transform buttonTransform, Professor professor, int subjectId)
    {
        Button button = buttonTransform.GetComponent<Button>();
        professorBusy.GetComponent<TMP_Text>().text = $"{professor.ProfessorGetSubjects().Count} / {getProfessorMaxAssignNum(professor)}";
        if (professor.ProfessorGetSubjects().Contains(subjectId))
        {
            buttonTransform.GetComponent<Image>().color = Color.cyan;
        }
        if (professor.ProfessorGetSubjects().Count == getProfessorMaxAssignNum(professor) && !professor.ProfessorGetSubjects().Contains(subjectId))
        {
            buttonTransform.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f);
            button.enabled = false;
        }
        if (!SubjectTree.canFreeProfessorInSubject(professor.ProfessorGetID(), subjectId))
        {
            buttonTransform.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f);
            button.enabled = false;
        }
        if (professor.ProfessorGetAwayStatus() || (professor.ProfessorGetType() == 0 && SubjectTree.getSubject(subjectId).root != 0))
        {
            buttonTransform.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f);
            button.enabled = false;
        }
        if (professor.ProfessorGetType() != 2 && SubjectTree.getSubject(subjectId).tier == 5)
        {
            buttonTransform.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f);
            button.enabled = false;
        }
    }

    public void updateProfessorSelectButton(int index)
    {
        GameObject targetSelectButton = selectButtons[index].buttonObject;
        Transform buttonTransform = targetSelectButton.transform.GetChild(0);
        Transform professorName = buttonTransform.GetChild(0);
        Transform professorBusy = buttonTransform.GetChild(1);
        updateSelectButtonAndColor(professorBusy, buttonTransform, selectButtons[index].professor, selectedSubjectId);
    }

    public void initProfessorScroll(int subjectId)
    {
        clearProfessorContents();
        int index = 0;
        foreach (Professor professor in PlayerInfo.ProfessorList)
        {
            selectButtons.Add(addProfessorSelectButton(professor, subjectId, index++));
        }
    }

    public class SelectButton
    {
        public Professor professor;
        public GameObject buttonObject;
        public SelectButton(Professor professor, GameObject buttonObject)
        {
            this.professor = professor;
            this.buttonObject = buttonObject;
        }
    }
    public int getProfessorMaxAssignNum(Professor professor)
    {
        int ProfessorMaxAssignNum = 3;
        if (professor.ProfessorGetType() == 0)
        {
            ProfessorMaxAssignNum = 3;
        }
        else if (professor.ProfessorGetType() == 1)
        {
            ProfessorMaxAssignNum = 2;
        }
        else if (professor.ProfessorGetType() == 2)
        {
            ProfessorMaxAssignNum = 1;
        }
        return ProfessorMaxAssignNum;
    }
    public void openDetail(Professor professor, int subjectId, int index)
    {
        if (isProfessorDetailOpen) return;
        CameraController.canMove = false;
        isProfessorDetailOpen = true;
        professorDetailUI.gameObject.SetActive(true);
        TMP_Text professorNameText = detailNameTransform.GetComponent<TMP_Text>();
        professorNameText.text = professor.ProfessorGetName();
        changeDetailInfo(professor);
        professorImageTransform.GetComponent<Image>().sprite = professorIllust[professor.ProfessorGetType()];
        Button exitButton = exitDetailButtonTransform.GetComponent<Button>();
        Button assignButton = assignButtonTransform.GetComponent<Button>();
        Button freeButton = freeButtonTransform.GetComponent<Button>();
        exitButton.onClick.RemoveAllListeners();
        assignButton.onClick.RemoveAllListeners();
        freeButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(closeButtonClick);
        assignButton.onClick.AddListener(
            delegate
            {
                assignButtonClick(professor, subjectId, index);
            }
        );
        freeButton.onClick.AddListener(
            delegate
            {
                freeButtonClick(professor, subjectId, index);
            }
        );
        if (professor.ProfessorGetSubjects().Contains(subjectId))
        {
            assignButtonTransform.gameObject.SetActive(false);
            freeButtonTransform.gameObject.SetActive(true);
        }
        else
        {
            assignButtonTransform.gameObject.SetActive(true);
            freeButtonTransform.gameObject.SetActive(false);
        }
    }

    public void changeDetailInfo(Professor professor)
    {
        TMP_Text infoText = detailInfoTransform.GetComponent<TMP_Text>();
        int professorAbleToAssignNum = getProfessorMaxAssignNum(professor);
        infoText.text = $"담당 과목 수 : {professor.ProfessorGetSubjects().Count} / {professorAbleToAssignNum}\n";
        infoText.text += $"분류 : {professor.ProfessorGetTypeInString()} 교수\n";
        infoText.text += $"유지비 : {professor.ProfessorGetSalary()} Ar\n";
        List<int> professorStat = professor.ProfessorGetStats();
        infoText.text += $"강의력 : {professorStat[0]}\n";
        infoText.text += $"마법이론 : {professorStat[1]}\n";
        infoText.text += $"마나감응 : {professorStat[2]}\n";
        infoText.text += $"손재주 : {professorStat[3]}\n";
        infoText.text += $"속성력 : {professorStat[4]}\n";
        infoText.text += $"영창 마법력 : {professorStat[5]}";
    }

    public void assignButtonClick(Professor professor, int subjectId, int index)
    {
        SubjectTree.addProfessorAt(professor.ProfessorGetID(), subjectId);
        professor.ProfessorAddSubject(subjectId);

        changeDetailInfo(professor);
        updateProfessorSelectButton(index);
        SubjectTree.ableToEndTurn =  SubjectTree.checkAvailToCreateCurriculum();

        assignButtonTransform.gameObject.SetActive(false);
        freeButtonTransform.gameObject.SetActive(true);
        closeButtonClick();
    }
    public void freeButtonClick(Professor professor, int subjectId, int index)
    {
        SubjectTree.removeProfessorAt(professor.ProfessorGetID(), subjectId);
        professor.ProfessorRemoveSubject(subjectId);

        changeDetailInfo(professor);
        updateProfessorSelectButton(index);
                SubjectTree.ableToEndTurn =  SubjectTree.checkAvailToCreateCurriculum();

        assignButtonTransform.gameObject.SetActive(true);
        freeButtonTransform.gameObject.SetActive(false);
        closeButtonClick();
        initProfessorScroll(subjectId);
    }

    public void closeButtonClick()
    {
        CameraController.canMove = true;
        isProfessorDetailOpen = false;
        professorDetailUI.gameObject.SetActive(false);
    }

    public void goBack()
    {
        if (isProfessorDetailOpen) return;
        LoadingSceneManager.LoadScene("Main");
    }
    public void setSubjectInfo(int id)
    {
        subjectInfoUI.gameObject.SetActive(true);
        Subject subjectInfo = SubjectTree.getSubject(id);
        subjectInfoUI.GetChild(1).GetComponent<TMP_Text>().text = subjectInfo.name;
        List<int> enforceInfo = subjectInfo.enforceContents;
        string tmpText = "";
        for (int i = 0; i < enforceInfo.Count; i++)
        {
            if (enforceInfo[i] != 0)
                tmpText += SubjectTree.subjectsInfo.enforceTypeName[i] + " + " + enforceInfo[i] + "%\n";
        }
        subjectInfoUI.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = tmpText;
    }
}