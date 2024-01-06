using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckStudentCurriculum : MonoBehaviour
{
    public GameObject student;
    public Transform studentContent;

    private void Start()
    {
        for (int i = 0; i < PlayerInfo.studentGroups.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                StudentGroup studentgroup = PlayerInfo.studentGroups[i][j];
                GameObject studentInfo = Instantiate(student, studentContent);
                studentInfo.transform.GetChild(0).GetComponent<Text>().text =
                    $"{studentgroup.GetPeriod()}기 {studentgroup.GetDivision()}분반";
                studentInfo.transform.GetChild(1).GetComponent<Text>().text =
                    $"현재 {studentgroup.GetAge()}턴째 과목 이수 중";
                //studentInfo.GetComponent<Button>().onClick.AddListener(delegate { StudentClicked(studentgroup.GetStat()); });
            }
        }
    }
}
