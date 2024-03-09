using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckCurriculumManager : MonoBehaviour
{
    public Transform scroll_content;
    public Transform subject;
    public Transform numStat;
    public Transform periodDiv;
    public GameObject s_prefab;
    public Button backButton;
    public CurriculumTreeDrawingManager drawingManager;

    public void InitCurriculum()
    {
        foreach (Transform sub in subject.GetComponent<Transform>())
        {
            Debug.Log(sub);
            sub.GetComponent<Image>().color = Color.gray;
            sub.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ShowCurriculum(List<int> curriculum, int age)
    {
        InitCurriculum();
        for (int i = 0; i < age; i++)
        {
            Debug.Log(curriculum[i]);
            Transform sub = subject.GetChild(curriculum[i]);
            sub.GetComponent<Image>().color = Color.cyan;
            sub.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            sub.GetChild(0).gameObject.SetActive(true);
        }
        for (int i = age; i < 8; i++)
        {
            Debug.Log(curriculum[i]);
            Transform sub = subject.GetChild(curriculum[i]);
            sub.GetComponent<Image>().color = Color.green;
            sub.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            sub.GetChild(0).gameObject.SetActive(true);
        }
        drawingManager.drawTree(curriculum);
    }

    public void ShowStat(int period, int div, int num, List<int> stat)
    {
        periodDiv.parent.gameObject.SetActive(true);

        periodDiv.GetChild(0).GetComponent<Text>().text = period.ToString();
        periodDiv.GetChild(1).GetComponent<Text>().text = div.ToString();
        numStat.GetChild(0).GetComponent<Text>().text = num.ToString();
        for (int i = 1; i<= stat.Count; i++)
        {
            numStat.GetChild(i).GetComponent<Text>().text = stat[i-1].ToString();
        }
    }

    private void Start()
    {
        InitCurriculum();
        periodDiv.parent.gameObject.SetActive(false);
        backButton.onClick.AddListener(delegate
        {
            LoadingSceneManager.LoadScene("Main");
        });
        foreach (Transform button in scroll_content.GetComponentInChildren<Transform>())
        {
            Destroy(button.gameObject);
        }
        foreach (StudentGroup[] studentGroups in PlayerInfo.studentGroups)
        {
            foreach (StudentGroup student in studentGroups)
            {
                if(student.GetNumber() > 0) 
                {
                    GameObject newStudent = Instantiate(s_prefab, scroll_content);
                    newStudent.transform.GetChild(0).GetComponent<Text>().text = $"{student.GetPeriod()}기 {student.GetDivision()}분반";
                    newStudent.GetComponent<Button>().onClick.RemoveAllListeners();
                    newStudent.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        ShowCurriculum(student.GetCurriculum(), student.GetAge());
                        ShowStat(student.GetPeriod(), student.GetDivision(), student.GetNumber(), student.GetStat());
                    });
                }
                
            }
        }
    }


}
