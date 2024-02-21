using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentCostUpdate : MonoBehaviour
{
    public Text periodT;
    public Text numT;
    public Text costT;
    public Text costSumT;
    public void CurrentContentUpdate(int i)
    {
        int period;
        int costSum = 0;
        int numSum = 0;
        int cost;
        int age;

        StudentGroup studentGroup = PlayerInfo.studentGroups[i - 1][0];

        period = studentGroup.GetPeriod();
        cost = studentGroup.GetCost();
        age = studentGroup.GetAge();
        foreach (StudentGroup student in PlayerInfo.studentGroups[i - 1])
        {
            numSum += student.GetNumber();
        }
        costSum = cost*age*numSum;
        periodT.text = period.ToString();
        numT.text = numSum.ToString();
        costT.text = cost.ToString();
        costSumT.text = costSum.ToString();
    }

    public void CurrentContentUpdate(string graduateData)
    {
        string[] studentInfo = graduateData.Split("/");
        periodT.text = studentInfo[0];
        numT.text = studentInfo[1];
        costT.text = studentInfo[2];
        costSumT.text = studentInfo[3];
    }
}
