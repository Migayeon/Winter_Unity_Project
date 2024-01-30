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
    private ESC_Manager ESC_Script;

    [SerializeField]
    private Transform[] subjectInfoUIList;
    private Transform subjectInfoUI;

    [SerializeField]
    private Transform[] panelList;
    private Transform panel;

    private Transform subjectContentsUI;
    private Transform subjectNameUI;

    public static bool selectFixed = false;


    private void Update()
    {
        if (ESC_Script.isPause) return;
        if (subjectInfoUI == null) return;
        if (!selectFixed)
        {
            Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray2 = new Ray2D(mp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
            if (hit.collider != null)
            {
                GameObject nowObj = hit.collider.gameObject;
                panel.gameObject.SetActive(true);
                subjectInfoUI.gameObject.SetActive(true);
                setUI(int.Parse(nowObj.name));
            }
            else
            {
                panel.gameObject.SetActive(false);
                subjectInfoUI.gameObject.SetActive(false);
            }
        }
    }
    public void setMod(int mod)
    {
        if (subjectInfoUIList[mod] != null)
        {
            subjectInfoUI = subjectInfoUIList[mod];
            panel = panelList[mod];
            subjectContentsUI = subjectInfoUI.GetChild(0).GetChild(0);
            subjectNameUI = subjectInfoUI.GetChild(1);
        }
        else
        {
            panel = null;
            subjectInfoUI = null;
            subjectContentsUI = null;
            subjectNameUI = null;
        }
    }

    public void setUI(int id)
    {
        panel.gameObject.SetActive(true);
        subjectInfoUI.gameObject.SetActive(true);
        Subject subjectInfo = SubjectTree.getSubject(id);
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

    public void setSubjectInfoUITransform(int mod)
    {
        subjectInfoUI = subjectInfoUIList[mod];
        panel = panelList[mod];
    }
}