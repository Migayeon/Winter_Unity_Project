using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManageCurriculumMod : MonoBehaviour
{
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

    private Transform professorContents;
    private Transform professorNameUI;
    private bool isProfessorDetailOpen;
    private List<SelectButton> selectButtons = new List<SelectButton>();

    private Transform detailNameTransform;
    private Transform detailInfoTransform;
    private Transform assignButtonTransform;
    private Transform freeButtonTransform;
    private Transform exitDetailButtonTransform;

    private int selectedSubjectId = -1;

    private void Awake()
    {
        SubjectTree.initSubjectsAndInfo();
        SubjectTree.initSubjectStates(new List<int>());

        professorContents = professorInfoUI.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        professorNameUI = professorInfoUI.GetChild(1);
        detailNameTransform = professorDetailUI.GetChild(0).GetChild(0);
        detailInfoTransform = professorDetailUI.GetChild(0).GetChild(1);
        assignButtonTransform = professorDetailUI.GetChild(0).GetChild(2);
        freeButtonTransform = professorDetailUI.GetChild(0).GetChild(3);
        exitDetailButtonTransform = professorDetailUI.GetChild(1);

        professorDetailUI.gameObject.SetActive(false);

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
    }
    private void Update()
    {
        if (!CurriculumSubjectGetter.selectFixed)
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray2 = new Ray2D(mp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
            if (hit.collider != null)
            {
                Transform nowTransform = hit.collider.transform;
                int nowTransformId = int.Parse(nowTransform.name);
                setUI(nowTransformId);
            }
            else
            {
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
            subject.GetComponent<Image>().color = new Color(0.862f, 0.862f, 0.862f);
        }
        else
        {
            subject.GetComponent<Image>().color = new Color(0.588f, 0.588f, 0.588f);
        }
    }

    public void SubjectClick(int clickedId)
    {
        if (clickedId == selectedSubjectId)
        {
            CurriculumSubjectGetter.selectFixed = false;
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
            CurriculumSubjectGetter.selectFixed = true;
            subjectGetter.GetComponent<CurriculumSubjectGetter>().setUI(clickedId);
            setUI(clickedId);
            subjectGameObject.transform.GetChild(selectedSubjectId).GetComponent<Image>().color = Color.green;
        }
    }

    public void OpenSubjectButtonClicked()
    {
        if (selectedSubjectId != -1)
        {
            SubjectTree.openSubject(selectedSubjectId);
            setUI(selectedSubjectId);
            foreach (Transform subject in subjectGameObject.GetComponentInChildren<Transform>())
                if (int.Parse(subject.name) != selectedSubjectId)
                    setColor(subject);
        }
    }

    public void setUI(int nowTransformId)
    {
        if (SubjectTree.subjectState[nowTransformId] == SubjectTree.State.Open)
        {
            initProfessorScroll(nowTransformId);
            professorInfoUI.gameObject.SetActive(true);
            OpenSubjectButton.gameObject.SetActive(false);
        }
        else if (SubjectTree.subjectState[nowTransformId] == SubjectTree.State.ReadyToOpen)
        {
            professorInfoUI.gameObject.SetActive(false);
            OpenSubjectButton.gameObject.SetActive(true);
        }
        else
        {
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

    public SelectButton addProfessorSelectButton(ProfessorSystem.Professor professor, int subjecctId)
    {
        GameObject newSelectButton = Instantiate(professorSelectButtonPrefab, professorContents);
        Transform buttonTransform = newSelectButton.transform.GetChild(0);
        Transform professorName = buttonTransform.GetChild(0);
        Transform professorBusy = buttonTransform.GetChild(1);
        Debug.Log(professor.ProfessorGetName());
        professorName.GetComponent<TMP_Text>().text = professor.ProfessorGetName();
        Button button = buttonTransform.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(
            delegate
            {
                subjectButtonClick(professor, subjecctId);
            }
        );
        
        //professorBusy.GetComponent<Text>().text = professor.ProfessorGetSubjects().Count.ToString();
        return new SelectButton(professor, newSelectButton);
    }

    public void initProfessorScroll(int subjecctId)
    {
        clearProfessorContents();
        SelectButton tmp;
        foreach (ProfessorSystem.Professor professor in PlayerInfo.ProfessorList)
        {
            tmp = addProfessorSelectButton(professor, subjecctId);
            selectButtons.Add(tmp);
        }
        
    }

    public class SelectButton
    {
        public ProfessorSystem.Professor professor;
        public GameObject button;
        public SelectButton(ProfessorSystem.Professor professor, GameObject button)
        {
            this.professor = professor;
            this.button = button;
        }
    }

    public void openDetail(ProfessorSystem.Professor professor, int subjectId)
    {
        CameraController.canMove = false;
        isProfessorDetailOpen = true;
        professorDetailUI.gameObject.SetActive(true);
        TMP_Text professorNameText = detailNameTransform.GetComponent<TMP_Text>();
        TMP_Text infoText = detailInfoTransform.GetComponent<TMP_Text>();
        professorNameText.text = professor.ProfessorGetName();
        infoText.text = "담당 과목 수 : 0 / 3";
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
                subjectButtonClick(professor, subjectId);
            }
        );
        freeButton.onClick.AddListener(
            delegate
            {
                freeButtonClick(professor, subjectId);
            }
        );
    }

    public void assignButtonClick(ProfessorSystem.Professor professor, int subjectId)
    {
        SubjectTree.addProfessorAt(professor.ProfessorGetID(), subjectId);
        professor.ProfessorAddSubject(subjectId);
    }
    public void freeButtonClick(ProfessorSystem.Professor professor, int subjectId)
    {
        if (SubjectTree.canFreeProfessorInSubject(professor.ProfessorGetID(), subjectId))
        {
            SubjectTree.removeProfessorAt(professor.ProfessorGetID(), subjectId);
            professor.ProfessorRemoveSubject(subjectId);
        }
    }

    public void closeButtonClick()
    {
        CameraController.canMove = true;
        isProfessorDetailOpen = false;
        professorDetailUI.gameObject.SetActive(false);
    }

    public void subjectButtonClick(ProfessorSystem.Professor professor, int subjecctId)
    {
        if (!isProfessorDetailOpen)
            openDetail(professor, subjecctId);
    }
}