using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentCostManager : MonoBehaviour
{
    public Slider costSlider;
    public Text currentCost;
    public GameObject infoPrefab;
    public Transform content;

    void CostChange()
    {
        PlayerInfo.cost = (int)costSlider.value;
        currentCost.text = ((int)costSlider.value).ToString();
    }

    void Awake()
    {
        costSlider.value = PlayerInfo.cost;
        currentCost.text = PlayerInfo.cost.ToString();
        costSlider.onValueChanged.AddListener(delegate { CostChange(); });
        foreach (Transform prevInfo in content.GetComponentInChildren<Transform>())
        {
            Destroy(prevInfo.gameObject);
        }
        int div;
        int costSum;
        for (int i = 0; i<PlayerInfo.studentGroups.Count; i++)
        {
            costSum = 0;
            div = i + 1;
            foreach (StudentGroup student in PlayerInfo.studentGroups[i])
            {
                costSum += student.GetCost() * student.GetAge();
            }
            GameObject studentInfo = Instantiate(infoPrefab, content);
        }
    }
}
