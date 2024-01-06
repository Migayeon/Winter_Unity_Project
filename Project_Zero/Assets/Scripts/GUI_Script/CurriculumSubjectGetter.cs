using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurriculumSubjectGetter : MonoBehaviour
{
    [SerializeField]
    private int defaultIndex;
    [SerializeField]
    private Transform[] subjectInfoUIList;
    private Transform subjectInfoUI;

    private Transform subjectContentsUI;
    private Transform subjectNameUI;

    private void Awake()
    {
        SubjectTree.initSubjectsAndInfo();

        subjectInfoUI = subjectInfoUIList[defaultIndex];
        subjectContentsUI = subjectInfoUI.GetChild(0).GetChild(0);
        subjectNameUI = subjectInfoUI.GetChild(1);
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
            string tmpText = "";
            for (int i = 0; i < enforceInfo.Count; i++)
            {
                if (enforceInfo[i] != 0)
                    tmpText += SubjectTree.subjectsInfo.enforceTypeName[i] + " + " + enforceInfo[i] + "%\n";
            }
            subjectContentsUI.GetComponent<TMP_Text>().text = tmpText;
        }
        else
        {
            subjectInfoUI.gameObject.SetActive(false);
        }
    }

    public void setSubjectInfoUITransform(int mod)
    {
        subjectInfoUI = subjectInfoUIList[mod];
    }
}
