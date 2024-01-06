using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfessorInCurriculumManger : MonoBehaviour
{
    [SerializeField]
    private Transform professorInfoUI;

    private Transform professorContentsUI;
    private Transform professorNameUI;

    private void Start()
    {
        SubjectTree.initSubjectsAndInfo();

        professorContentsUI = professorInfoUI.GetChild(0).GetChild(0);
        professorNameUI = professorInfoUI.GetChild(1);
    }
    private void Update()
    {
        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray2 = new Ray2D(mp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
        if (hit.collider != null)
        {
            GameObject nowObj = hit.collider.gameObject;
            if (SubjectTree.subjectState[int.Parse(nowObj.name)] == SubjectTree.State.Open)
            {
                professorInfoUI.gameObject.SetActive(true);
            }
        }
        else
        {
            professorInfoUI.gameObject.SetActive(false);
        }
    }
}
