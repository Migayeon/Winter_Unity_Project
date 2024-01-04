using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudentStatisticsScene : MonoBehaviour
{
    public void MoveToStatistics()
    {
        SceneManager.LoadScene("StudentCost");
    }
}
