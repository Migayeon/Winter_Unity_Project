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

    private Transform professorContentsUI;
    private Transform professorNameUI;

    private int selectedSubjectId = -1;

    private void Awake()
    {
        SubjectTree.initSubjectsAndInfo();
        SubjectTree.initSubjectStates(new List<int>());

        professorContentsUI = professorInfoUI.GetChild(0).GetChild(0);
        professorNameUI = professorInfoUI.GetChild(1);

        foreach (Transform subject in subjectGameObject.GetComponentInChildren<Transform>())
            setColor(subject);

        foreach (Button subject in subjectGameObject.transform.GetComponentsInChildren<Button>())
        {
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
}
