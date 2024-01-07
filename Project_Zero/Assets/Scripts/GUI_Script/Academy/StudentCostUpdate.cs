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
    public void ContentUpdate(int i)
    {
        int costSum = 0;
        int numSum = 0;
        int cost;
        int age;
        /*
         
        이미 졸업한 학생의 경우 단순 데이터 처리

         */
        cost = PlayerInfo.studentGroups[i - 1][0].GetCost();
        age = PlayerInfo.studentGroups[i - 1][0].GetAge();
        foreach (StudentGroup student in PlayerInfo.studentGroups[i - 1])
        {
            numSum += student.GetNumber();
        }
        costSum = cost*age*numSum;
        periodT.text = i.ToString();
        numT.text = numSum.ToString();
        costT.text = cost.ToString();
        costSumT.text = costSum.ToString();
    }
}
