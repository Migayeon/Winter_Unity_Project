using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurriculumSetting : MonoBehaviour
{
    public List<int> CurriculumList;

    public void SubjectClick(int i)
    {
        
        if (CurriculumList.Contains(i))
        {
            CurriculumCancel(i);
        }
        else
        {
            CurriculumList.Add(i);
            Debug.Log(i);
            if (i == 4)//(!SubjectTree.isVaildCurriculum(CurriculumList))
            {
                CurriculumList.Remove(i);
                StartCoroutine(WarningMessage());
                return;
            }
            Image status = transform.GetChild(i).GetComponent<Image>();
            status.color = Color.green;
            GameObject order = status.transform.GetChild(0).gameObject;
            order.GetComponent<TextMeshProUGUI>().text = CurriculumList.Count.ToString();
            order.SetActive(true);
        }
    }

    public void CurriculumCancel(int i)
    {
        int time = CurriculumList.Count - CurriculumList.IndexOf(i);
        for (int j = 0; j < time; j++)
        {
            int subject = CurriculumList[CurriculumList.Count - 1];
            Image status = transform.GetChild(subject).GetComponent<Image>();
            status.color = Color.white;
            status.transform.GetChild(0).gameObject.SetActive(false);
            CurriculumList.RemoveAt(CurriculumList.Count-1);
        }
    }

    IEnumerator WarningMessage()
    {
        Debug.Log("invalid");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("close");
    }

    private void Awake()
    {
        CurriculumList = new List<int>();
        foreach (var subject in GetComponentsInChildren<Button>())
        {
            subject.onClick.AddListener(delegate { SubjectClick(Convert.ToInt32(subject.name)); });
            subject.image.color = Color.white;
            subject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
