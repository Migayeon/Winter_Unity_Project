using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentCostManager : MonoBehaviour
{
    public Slider costSlider;
    public Text currentCost;

    void CostChange()
    {
        PlayerInfo.cost = (int)costSlider.value;
        currentCost.text = ((int)costSlider.value).ToString();
    }

    void Awake()
    {
        costSlider.onValueChanged.AddListener(delegate { CostChange(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
