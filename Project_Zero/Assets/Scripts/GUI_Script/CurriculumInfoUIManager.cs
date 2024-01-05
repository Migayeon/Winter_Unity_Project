using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurriculumInfoUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform subjectInfoUI;
    [SerializeField]
    private Transform professorInfoUI;

    private Transform subjectContentsUI;
    private Transform subjectNameUI;
    private Transform professorContentsUI;
    private Transform professorNameUI;

    private void Start()
    {
        SubjectTree.initSubjectsAndInfo();

        subjectContentsUI = subjectInfoUI.GetChild(0).GetChild(0);
        subjectNameUI = subjectInfoUI.GetChild(1);

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
            subjectInfoUI.gameObject.SetActive(true);
            Subject subjectInfo = SubjectTree.getSubject(int.Parse(nowObj.name));
            subjectNameUI.GetComponent<TMP_Text>().text = subjectInfo.name;
            List<int> enforceInfo = subjectInfo.enforceContents;
            subjectContentsUI.GetComponent<TMP_Text>().text =
                $"{enforceInfo[0]}, {enforceInfo[1]}, {enforceInfo[2]}, {enforceInfo[3]}, {enforceInfo[4]}";
        }
        else
        {
            subjectInfoUI.gameObject.SetActive(false);
        }
    }
}
