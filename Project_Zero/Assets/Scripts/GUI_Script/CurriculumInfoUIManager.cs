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
    GameObject subjectInfoUI;

    private Subject subjectInfo;
    void Start()
    {
        SubjectTree.initSubjectsAndInfo();
       // subjectInfoUI.SetActive(false);
    }
    void Update()
    {
        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray2 = new Ray2D(mp, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray2.origin, ray2.direction);
        GameObject testObject = null;
        try
        {
            testObject = hit.collider.gameObject;
        }
        catch { }
        if (testObject != null) 
        {
            subjectInfoUI.SetActive(true);
            subjectInfo = SubjectTree.getSubject(Convert.ToInt32(testObject.name));
            subjectInfoUI.transform.GetChild(0).GetComponent<TMP_Text>().text = subjectInfo.name;
            List<int> enforceInfo = subjectInfo.enforceContents;
            subjectInfoUI.transform.GetChild(1).GetComponent<TMP_Text>().text =
                $"{enforceInfo[0]}, {enforceInfo[1]}, {enforceInfo[2]}, {enforceInfo[3]}, {enforceInfo[4]}";
        }
        else
        {
           subjectInfoUI.SetActive(false);
        }
    }
}
