using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentCostUpdate : MonoBehaviour
{
    private void Awake()
    {
        int costSum = 0;
        int i = int.Parse(name);
        foreach (StudentGroup student in PlayerInfo.studentGroups[i - 1])
        {
            costSum += student.GetCost() * student.GetAge();
        }
    }
}
