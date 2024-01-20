using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckCurriculumManager : MonoBehaviour
{
    public Transform scroll_content;
    public Transform subject;
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

    private void Start()
    {
        InitCurriculum();
        backButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("Main");
        });
        foreach (Transform button in scroll_content.GetComponentInChildren<Transform>())
        {
            Destroy(button.gameObject);
        }
        foreach (StudentGroup[] studentGroups in PlayerInfo.studentGroups)
        {
            foreach (StudentGroup student in studentGroups)
            {
                GameObject newStudent = Instantiate(s_prefab,scroll_content);
                newStudent.transform.GetChild(0).GetComponent<Text>().text = $"{student.GetPeriod()}기 {student.GetDivision()}분반";
                newStudent.GetComponent<Button>().onClick.AddListener(delegate
                {
                    ShowCurriculum(student.GetCurriculum(),student.GetAge());
                });
            }
        }
    }


}
